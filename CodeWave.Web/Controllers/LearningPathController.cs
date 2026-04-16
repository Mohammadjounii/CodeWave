using CodeWave.Application.DTOs;
using CodeWave.Application.ViewModels;
using CodeWave.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using System.Security.Claims;
using System.Text;
using System.Text.Json;


namespace CodeWave.Web.Controllers
{
    [Authorize]
    public class LearningPathController : Controller
    {
        private readonly ILearningPathService _learningPathService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HttpClient _httpClient;
        private readonly ILessonCompletionRepository _lessonCompletionRepository;
        private readonly string _geminiApiKey;

        public LearningPathController(ILearningPathService learningPathService, UserManager<ApplicationUser> userManager, IHttpClientFactory httpClientFactory, ILessonCompletionRepository lessonCompletionRepository, IConfiguration configuration)
        {
            _learningPathService = learningPathService;
            _userManager = userManager;
            _httpClient = httpClientFactory.CreateClient();
            _lessonCompletionRepository = lessonCompletionRepository;
            _geminiApiKey = configuration["Gemini:ApiKey"] ?? string.Empty;
        }

        // GET: /LearningPath/Course/{courseId}
        [HttpGet]
        public async Task<IActionResult> Course(Guid courseId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid.TryParse(userIdStr, out var userId);

            var vm = await _learningPathService.GetCourseAsync(courseId, userId);
            if (vm == null)
            {
                return NotFound("Course not found.");
            }

            // Populate ViewBag with user data for sidebar and navbar
            if (!string.IsNullOrEmpty(userIdStr) && Guid.TryParse(userIdStr, out var userGuid))
            {
                var user = await _userManager.FindByIdAsync(userIdStr);
                if (user != null)
                {
                    // Build full name
                    var fullName = user.FirstName;
                    if (!string.IsNullOrEmpty(user.LastName))
                    {
                        fullName += " " + user.LastName;
                    }

                    // Get level or default to "Developer"
                    var level = !string.IsNullOrEmpty(user.Level) ? user.Level + " Developer" : "Developer";

                    ViewBag.UserName = fullName;
                    ViewBag.UserLevel = level;
                    ViewBag.ProfilePictureUrl = user.ProfilePictureUrl;
                    
                    // Default profile picture if none exists
                    var defaultPictureUrl = "data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='40' height='40'%3E%3Ccircle cx='20' cy='20' r='20' fill='%234a5568'/%3E%3Cpath d='M20 12c-2.21 0-4 1.79-4 4s1.79 4 4 4 4-1.79 4-4-1.79-4-4-4zm0 10c-3.31 0-6 1.69-6 4v2h12v-2c0-2.31-2.69-4-6-4z' fill='white'/%3E%3C/svg%3E";
                    ViewBag.DisplayPictureUrl = !string.IsNullOrEmpty(user.ProfilePictureUrl) ? user.ProfilePictureUrl : defaultPictureUrl;
                }
            }

            return View("Course", vm);
        }

        // AJAX: GET /LearningPath/Lesson?lessonId=...
       
        [HttpGet]
        public async Task<IActionResult> Lesson(Guid lessonId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid.TryParse(userIdStr, out var userId);

            var lesson = await _learningPathService.GetLessonAsync(lessonId, userId);
            if (lesson == null)
                return NotFound();

            return Json(new
            {
                id = lesson.Id,
                title = lesson.Title,
                content = lesson.Content,
                videoUrl = lesson.VideoUrl,
                imageUrl = lesson.ImageUrl,
                exercises = lesson.Exercises.Select(e => new {
                    e.Id, e.Title, e.Description, e.StarterCode, e.IsSolved
                })
            });
        }

        // AJAX: POST /LearningPath/CompleteLesson?lessonId=...










        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteLesson(Guid lessonId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var result = await _learningPathService.CompleteLessonAsync(lessonId, userId);
            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(new { success = true });
        }







        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitExercise([FromBody] SubmitExerciseRequest model)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var result = await _learningPathService.SubmitExerciseAsync(new SubmitExerciseRequestDto
            {
                ExerciseId = model.ExerciseId,
                SubmittedCode = model.SubmittedCode,
                Output = model.Output
            }, userId);

            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(new
            {
                success = true,
                isCorrect = result.IsCorrect,
                message = result.Message
            });
        }







        [Authorize]
        [HttpGet]
        public async Task<IActionResult> FocusMode(Guid lessonId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid.TryParse(userIdStr, out var userId);

            var vm = await _learningPathService.GetFocusModeAsync(lessonId, userId);

            if (vm == null)
                return NotFound();

            // Enforce sequential access: previous lesson must be completed first
            if (vm.PreviousLessonId.HasValue)
            {
                var prevCompletion = await _lessonCompletionRepository.GetAsync(userId, vm.PreviousLessonId.Value);
                if (prevCompletion?.IsCompleted != true)
                    return Forbid();
            }

            return View("FocusModeLesson", vm);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AIHelper([FromBody] AIHelperRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Question))
            {
                return BadRequest(new { success = false, response = "Please provide a question." });
            }

            try
            {
                var response = await GenerateAIResponse(request);
                return Ok(new { success = true, response = response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, response = "An error occurred. Please try again." });
            }
        }

        private async Task<string> GenerateAIResponse(AIHelperRequest request)
        {
            var language = request.Language?.ToLower() ?? "python";
            var exerciseTitle = request.ExerciseTitle ?? "";
            var exerciseDescription = request.ExerciseDescription ?? "";

            // Build context-aware prompt for the AI
            var systemPrompt = $"You are a helpful coding tutor for {language} programming. ";
            if (!string.IsNullOrEmpty(exerciseTitle))
            {
                systemPrompt += $"The student is working on an exercise titled: '{exerciseTitle}'. ";
            }
            if (!string.IsNullOrEmpty(exerciseDescription))
            {
                systemPrompt += $"Exercise description: {exerciseDescription}. ";
            }
            systemPrompt += "Provide clear, educational responses. If asked for solutions, give hints and guidance rather than complete answers to encourage learning.";

            var fullPrompt = $"{systemPrompt}\n\nStudent's question: {request.Question}";

            try
            {
                // First, try to get available models (optional - if this fails, we'll use fallback models)
                List<string> availableModels = new List<string>();
                try
                {
                    var listModelsUrl = $"https://generativelanguage.googleapis.com/v1beta/models?key={_geminiApiKey}";
                    var listResponse = await _httpClient.GetAsync(listModelsUrl);
                    if (listResponse.IsSuccessStatusCode)
                    {
                        var listContent = await listResponse.Content.ReadAsStringAsync();
                        var listJson = JsonSerializer.Deserialize<JsonElement>(listContent);
                        if (listJson.TryGetProperty("models", out var models))
                        {
                            foreach (var model in models.EnumerateArray())
                            {
                                if (model.TryGetProperty("name", out var name))
                                {
                                    var modelName = name.GetString();
                                    if (!string.IsNullOrEmpty(modelName) && modelName.Contains("gemini"))
                                    {
                                        // Extract just the model name part (e.g., "models/gemini-1.5-flash" -> "gemini-1.5-flash")
                                        var parts = modelName.Split('/');
                                        if (parts.Length > 0)
                                        {
                                            availableModels.Add(parts[parts.Length - 1]);
                                        }
                                    }
                                }
                            }
                            Console.WriteLine($"Found {availableModels.Count} available Gemini models: {string.Join(", ", availableModels)}");
                        }
                    }
                }
                catch (Exception listEx)
                {
                    Console.WriteLine($"Could not list models (this is optional): {listEx.Message}");
                }

                // Prepare the request payload for Gemini API
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = fullPrompt }
                            }
                        }
                    }
                };

                var jsonContent = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Use available models if we found any, otherwise use fallback list
                // Include the model from the Java example (gemini-3-pro-preview) and try various naming conventions
                var modelsToTry = availableModels.Count > 0 
                    ? availableModels.ToArray() 
                    : new[] { 
                        "gemini-3-pro-preview",  // From the Java example
                        "gemini-1.5-flash-latest", 
                        "gemini-1.5-pro-latest", 
                        "gemini-1.5-flash-002",
                        "gemini-1.5-pro-002",
                        "gemini-pro",
                        "gemini-1.5-flash", 
                        "gemini-1.5-pro",
                        "gemini-pro-vision"  // Sometimes available
                    };
                
                // Try both v1beta and v1 API versions
                var apiVersions = new[] { "v1beta", "v1" };
                
                foreach (var apiVersion in apiVersions)
                {
                    foreach (var model in modelsToTry)
                    {
                        try
                        {
                            var apiUrl = $"https://generativelanguage.googleapis.com/{apiVersion}/models/{model}:generateContent?key={_geminiApiKey}";
                            Console.WriteLine($"Trying Gemini API: {apiVersion}/{model}");
                            
                            var response = await _httpClient.PostAsync(apiUrl, content);
                            var responseContent = await response.Content.ReadAsStringAsync();

                            if (response.IsSuccessStatusCode)
                            {
                                var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);

                                // Extract the generated text from Gemini's response
                                if (jsonResponse.TryGetProperty("candidates", out var candidates) && 
                                    candidates.GetArrayLength() > 0)
                                {
                                    var firstCandidate = candidates[0];
                                    
                                    // Check for finishReason to see if response was blocked
                                    if (firstCandidate.TryGetProperty("finishReason", out var finishReason))
                                    {
                                        var reason = finishReason.GetString();
                                        if (reason == "SAFETY" || reason == "RECITATION")
                                        {
                                            return "I couldn't provide a response due to content safety filters. Please rephrase your question.";
                                        }
                                    }
                                    
                                    if (firstCandidate.TryGetProperty("content", out var contentObj) &&
                                        contentObj.TryGetProperty("parts", out var parts) &&
                                        parts.GetArrayLength() > 0)
                                    {
                                        var firstPart = parts[0];
                                        if (firstPart.TryGetProperty("text", out var text))
                                        {
                                            var responseText = text.GetString();
                                            if (!string.IsNullOrEmpty(responseText))
                                            {
                                                return responseText;
                                            }
                                        }
                                    }
                                }
                                
                                // If we got here, the response structure might be different
                                Console.WriteLine($"Unexpected response structure from {model}: {responseContent}");
                                continue; // Try next model
                            }
                            else
                            {
                                // Log the actual error for debugging
                                Console.WriteLine($"Gemini API Error ({model}) - Status: {response.StatusCode}, Response: {responseContent}");
                                
                                // Try to extract error message from response
                                try
                                {
                                    var errorResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
                                    if (errorResponse.TryGetProperty("error", out var errorObj))
                                    {
                                        if (errorObj.TryGetProperty("message", out var errorMessage))
                                        {
                                            var errorMsg = errorMessage.GetString();
                                            Console.WriteLine($"API Error Message ({model}): {errorMsg}");
                                            
                                            // If it's a model not found error, try next model
                                            if (errorMsg?.Contains("not found") == true || errorMsg?.Contains("404") == true || 
                                                errorMsg?.Contains("is not found") == true || errorMsg?.Contains("NOT_FOUND") == true)
                                            {
                                                Console.WriteLine($"Model {model} not available, trying next model...");
                                                continue; // Try next model
                                            }
                                            
                                            // If it's an API key error, don't try other models
                                            if (errorMsg?.Contains("API key") == true || errorMsg?.Contains("permission") == true)
                                            {
                                                return $"API key error: {errorMsg}. Please verify your API key is valid and has Gemini API enabled.";
                                            }
                                        }
                                    }
                                }
                                catch { }
                                
                                // If it's not a 404, don't try other models in this API version
                                if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
                                {
                                    // If it's an authentication error, don't try other models
                                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || 
                                        response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                                    {
                                        return $"Authentication failed (Status: {response.StatusCode}). Please verify your API key is valid and has Gemini API enabled.";
                                    }
                                    continue; // Try next model in same API version
                                }
                            }
                        }
                        catch (Exception modelEx)
                        {
                            Console.WriteLine($"Error trying model {model} with {apiVersion}: {modelEx.Message}");
                            continue; // Try next model
                        }
                    }
                }
                
                // If all models failed, provide helpful error message
                Console.WriteLine("All Gemini models failed. This might indicate:");
                Console.WriteLine("1. API key doesn't have access to Gemini API");
                Console.WriteLine("2. Gemini API is not enabled in Google Cloud Console");
                Console.WriteLine("3. API key restrictions are blocking access");
                return "Unable to connect to the AI service. All available models returned errors. Please verify:\n" +
                       "1. Your API key is valid and has Gemini API enabled\n" +
                       "2. Gemini API is enabled in your Google Cloud project\n" +
                       "3. There are no API key restrictions blocking access";
            }
            catch (Exception ex)
            {
                // Log error and return fallback response
                Console.WriteLine($"Gemini API Error: {ex.Message}");
                return "I encountered an error while processing your request. Please try again.";
            }
        }

        private string GenerateExplanation(AIHelperRequest request, string language, string exerciseTitle, string exerciseDescription)
        {
            var response = $"Let me explain the concept behind '{exerciseTitle}':\n\n";
            
            if (exerciseDescription.Contains("function") || exerciseTitle.ToLower().Contains("function"))
            {
                response += language == "python" 
                    ? "In Python, functions are reusable blocks of code that perform a specific task. They're defined using the `def` keyword followed by the function name and parameters. Functions help organize code and make it more maintainable.\n\nExample:\n```python\ndef greet(name):\n    return f\"Hello, {name}!\"\n```"
                    : "In Java, methods are functions defined within a class. They're declared with an access modifier, return type, method name, and parameters. Methods encapsulate behavior and can be called on objects or classes.\n\nExample:\n```java\npublic static String greet(String name) {\n    return \"Hello, \" + name + \"!\";\n}\n```";
            }
            else if (exerciseDescription.Contains("loop") || exerciseTitle.ToLower().Contains("loop"))
            {
                response += language == "python"
                    ? "Loops allow you to repeat code multiple times. Python has `for` loops (iterate over sequences) and `while` loops (repeat while condition is true).\n\nExample:\n```python\nfor i in range(5):\n    print(i)\n```"
                    : "Loops in Java include `for` loops (with initialization, condition, increment) and `while` loops. They're essential for repeating operations.\n\nExample:\n```java\nfor (int i = 0; i < 5; i++) {\n    System.out.println(i);\n}\n```";
            }
            else if (exerciseDescription.Contains("variable") || exerciseTitle.ToLower().Contains("variable"))
            {
                response += language == "python"
                    ? "Variables in Python store data values. They're created by assigning a value using `=`. Python is dynamically typed, so you don't need to declare the type.\n\nExample:\n```python\nname = \"CodeWave\"\nage = 5\n```"
                    : "Variables in Java store data and must be declared with a type. Java is statically typed, requiring explicit type declarations.\n\nExample:\n```java\nString name = \"CodeWave\";\nint age = 5;\n```";
            }
            else
            {
                response += $"This exercise focuses on: {exerciseDescription}\n\n";
                response += language == "python"
                    ? "Think about the problem step by step. Break it down into smaller parts, and use Python's built-in functions and data structures to solve it efficiently."
                    : "Break down the problem into logical steps. Use Java's object-oriented features, collections, and control structures to implement your solution.";
            }

            return response;
        }

        private string GenerateHint(AIHelperRequest request, string language, string exerciseTitle, string exerciseDescription)
        {
            var response = $"Here's a hint for '{exerciseTitle}':\n\n";
            
            if (exerciseDescription.ToLower().Contains("calculate") || exerciseTitle.ToLower().Contains("calculate"))
            {
                response += "Think about what mathematical operations you need. Break the problem into steps: input → process → output.";
            }
            else if (exerciseDescription.ToLower().Contains("loop") || exerciseTitle.ToLower().Contains("loop"))
            {
                response += language == "python"
                    ? "Consider using a `for` loop with `range()` or iterating over a collection. Remember to indent your loop body correctly!"
                    : "Think about using a `for` loop with proper initialization, condition, and increment. Don't forget the curly braces!";
            }
            else if (exerciseDescription.ToLower().Contains("function") || exerciseTitle.ToLower().Contains("function"))
            {
                response += language == "python"
                    ? "Start by defining your function with `def`. Think about what parameters it needs and what it should return. Use `return` to send back a value."
                    : "Define a method with the appropriate return type and parameters. Remember to use `return` if the method should return a value.";
            }
            else
            {
                response += "Read the problem carefully. Identify the inputs, the process needed, and the expected output. Start with a simple approach and refine it.";
            }

            response += "\n\n💡 Tip: Try writing pseudocode first, then translate it to code!";
            return response;
        }

        private string GenerateSolution(AIHelperRequest request, string language, string exerciseTitle, string exerciseDescription)
        {
            var response = $"Here's a solution approach for '{exerciseTitle}':\n\n";
            
            if (exerciseDescription.ToLower().Contains("area") || exerciseTitle.ToLower().Contains("area"))
            {
                response += language == "python"
                    ? "```python\ndef calculate_area(length, width):\n    return length * width\n\n# Example usage\nresult = calculate_area(4, 5)\nprint(f\"Area: {result}\")\n```"
                    : "```java\npublic static int calculateArea(int length, int width) {\n    return length * width;\n}\n```";
            }
            else if (exerciseDescription.ToLower().Contains("sum") || exerciseTitle.ToLower().Contains("sum"))
            {
                response += language == "python"
                    ? "```python\n# Using a loop\nsum = 0\nfor i in range(1, 11):\n    sum += i\nprint(f\"Sum: {sum}\")\n```"
                    : "```java\nint sum = 0;\nfor (int i = 1; i <= 10; i++) {\n    sum += i;\n}\nSystem.out.println(\"Sum: \" + sum);\n```";
            }
            else
            {
                response += "I can't provide the exact solution, but here's a general approach:\n\n";
                response += "1. Understand what the exercise is asking\n";
                response += "2. Identify the inputs and outputs\n";
                response += "3. Break down the logic into steps\n";
                response += "4. Write the code step by step\n";
                response += "5. Test with different inputs\n\n";
                response += "Try working through it yourself - that's how you learn! 💪";
            }

            return response;
        }

        private string GenerateErrorHelp(AIHelperRequest request, string language)
        {
            return $"Common issues and fixes:\n\n" +
                   $"1. **Syntax Errors**: Check for missing brackets, parentheses, or semicolons (in Java)\n" +
                   $"2. **Indentation**: Make sure your code is properly indented\n" +
                   $"3. **Variable Names**: Ensure variables are defined before use\n" +
                   $"4. **Type Errors**: Check that you're using the correct data types\n\n" +
                   $"Share your error message for more specific help!";
        }

        private string GenerateGeneralResponse(AIHelperRequest request, string language, string exerciseTitle, string exerciseDescription)
        {
            return $"I understand you're asking about: {request.Question}\n\n" +
                   $"For the exercise '{exerciseTitle}', here's some guidance:\n\n" +
                   $"{exerciseDescription}\n\n" +
                   $"Try breaking the problem into smaller steps. If you need more specific help, you can:\n" +
                   $"- Ask me to 'explain' the concept\n" +
                   $"- Request a 'hint'\n" +
                   $"- Ask for the 'solution' approach\n\n" +
                   $"What would you like to know more about?";
        }
    }

    public class AIHelperRequest
    {
        public string Question { get; set; }
        public Guid? LessonId { get; set; }
        public string? ExerciseId { get; set; }
        public string? Language { get; set; }
        public string? ExerciseTitle { get; set; }
        public string? ExerciseDescription { get; set; }
    }

    public class SubmitExerciseRequest
    {
        public Guid ExerciseId { get; set; }
        public string? SubmittedCode { get; set; }
        public string? Output { get; set; }
    }
}

