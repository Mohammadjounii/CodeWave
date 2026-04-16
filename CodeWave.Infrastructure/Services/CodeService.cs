using CodeWave.Application.DTOs;
using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;

namespace CodeWave.Application.Services;

public class CodeService : ICodeService
{
    private readonly CodeRunnerService _runner;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IExerciseSubmissionRepository _exerciseSubmissionRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CodeService(
        CodeRunnerService runner,
        IExerciseRepository exerciseRepository,
        IExerciseSubmissionRepository exerciseSubmissionRepository,
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork)
    {
        _runner = runner;
        _exerciseRepository = exerciseRepository;
        _exerciseSubmissionRepository = exerciseSubmissionRepository;
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SubmitExerciseResultDto> RunAndSaveAsync(RunCodeRequestDto request, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return new SubmitExerciseResultDto { Success = false, Message = "Invalid request." };
        }

        string detectedLanguage;
        Course? course = null;

        // If no exercise ID, run code directly without validation
        if (!request.ExerciseId.HasValue)
        {
            // Use language from request or default to python
            detectedLanguage = !string.IsNullOrWhiteSpace(request.Language) 
                ? request.Language.ToLower() 
                : "python";
            
            Console.WriteLine($"[CodeService] Running code without exercise. Language: {detectedLanguage}");
            
            // Just run the code and return output
            string result;
            try
            {
                // Wrap Python code if it's a function definition
                string codeToRun = request.Code;
                if (detectedLanguage.Equals("python", StringComparison.OrdinalIgnoreCase))
                {
                    codeToRun = WrapPythonCodeWithTestExecution(request.Code, "");
                }
                
                if (detectedLanguage.Equals("python", StringComparison.OrdinalIgnoreCase))
                {
                    result = await _runner.RunPythonAsync(codeToRun);
                }
                else if (detectedLanguage.Equals("java", StringComparison.OrdinalIgnoreCase))
                {
                    result = await _runner.RunJavaAsync(codeToRun);
                }
                else
                {
                    return new SubmitExerciseResultDto { Success = false, Message = $"Unsupported language: {detectedLanguage}" };
                }

                return new SubmitExerciseResultDto
                {
                    Success = true,
                    IsCorrect = false, // Not an exercise, so no correctness check
                    Output = result,
                    Message = "Code executed successfully!"
                };
            }
            catch (Exception ex)
            {
                return new SubmitExerciseResultDto
                {
                    Success = false,
                    Output = $"Error: {ex.Message}",
                    Message = "Error executing code."
                };
            }
        }

        // Get exercise with its lesson to access course
        var exercise = await _exerciseRepository.GetWithLessonAsync(request.ExerciseId.Value);
        if (exercise == null || exercise.Lesson == null)
        {
            return new SubmitExerciseResultDto { Success = false, Message = "Exercise not found." };
        }

        // Get the course to detect the programming language
        course = await _courseRepository.GetByIdAsync(exercise.Lesson.CourseId);
        if (course == null)
        {
            return new SubmitExerciseResultDto { Success = false, Message = "Course not found." };
        }

        // Detect language from course (ignore frontend language parameter for security)
        detectedLanguage = DetectLanguageFromCourse(course);
        
        // Log for debugging
        Console.WriteLine($"[CodeService] Exercise ID: {request.ExerciseId}");
        Console.WriteLine($"[CodeService] Course ID: {course.Id}");
        Console.WriteLine($"[CodeService] Course ProgrammingLanguage: {course.ProgrammingLanguage}");
        Console.WriteLine($"[CodeService] Course LearningPath: {course.LearningPath}");
        Console.WriteLine($"[CodeService] Detected Language: {detectedLanguage}");
        Console.WriteLine($"[CodeService] Frontend Language (ignored): {request.Language}");

        // Load test cases for this exercise
        var testCases = exercise.TestCases
            .Where(tc => !tc.IsDeleted)
            .OrderBy(tc => tc.OrderNumber)
            .ToList();

        var testCaseResults = new List<TestCaseResultDto>();
        bool allTestsPassed = true;
        string lastOutput = string.Empty;

        // If exercise has test cases, run each test case
        if (testCases.Any())
        {
            foreach (var testCase in testCases)
            {
                // Prepare code with input if needed
                string codeToRun = request.Code;
                
                // Always try to inject input/test execution (even if Input is empty, we might need to wrap function calls)
                codeToRun = InjectInputIntoCode(request.Code, testCase.Input ?? "", detectedLanguage);

                string result;
                try
                {
                    if (detectedLanguage.Equals("python", StringComparison.OrdinalIgnoreCase))
                    {
                        result = await _runner.RunPythonAsync(codeToRun);
                    }
                    else if (detectedLanguage.Equals("java", StringComparison.OrdinalIgnoreCase))
                    {
                        var javaStdin = ParseJavaStdin(testCase.Input ?? "");
                        result = await _runner.RunJavaAsync(codeToRun, javaStdin);
                    }
                    else
                    {
                        return new SubmitExerciseResultDto { Success = false, Message = $"Unsupported language: {detectedLanguage}" };
                    }
                }
                catch (Exception ex)
                {
                    testCaseResults.Add(new TestCaseResultDto
                    {
                        TestCaseId = testCase.Id,
                        Description = testCase.Description,
                        Passed = false,
                        ExpectedOutput = testCase.ExpectedOutput,
                        ActualOutput = $"Error: {ex.Message}",
                        Input = testCase.Input
                    });
                    allTestsPassed = false;
                    continue;
                }

                lastOutput = result;
                var expected = NormalizeLineEndings((testCase.ExpectedOutput ?? string.Empty).Trim());
                var actual = NormalizeLineEndings((result ?? string.Empty).Trim());
                // Full output match OR expected matches any single output line exactly (handles multi-println exercises)
                bool testPassed = string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase)
                    || actual.Split('\n').Any(line => string.Equals(line.Trim(), expected, StringComparison.OrdinalIgnoreCase));

                testCaseResults.Add(new TestCaseResultDto
                {
                    TestCaseId = testCase.Id,
                    Description = testCase.Description,
                    Passed = testPassed,
                    ExpectedOutput = expected,
                    ActualOutput = actual,
                    Input = testCase.Input
                });

                if (!testPassed)
                {
                    allTestsPassed = false;
                }
            }
        }
        else
        {
            // Fallback to old behavior: single expected output check
            // But still wrap Python code if it's a function definition
            string codeToRun = request.Code;
            if (detectedLanguage.Equals("python", StringComparison.OrdinalIgnoreCase))
            {
                codeToRun = WrapPythonCodeWithTestExecution(request.Code, "");
            }
            
            string result;
            if (detectedLanguage.Equals("python", StringComparison.OrdinalIgnoreCase))
            {
                result = await _runner.RunPythonAsync(codeToRun);
            }
            else if (detectedLanguage.Equals("java", StringComparison.OrdinalIgnoreCase))
            {
                result = await _runner.RunJavaAsync(codeToRun);
            }
            else
            {
                return new SubmitExerciseResultDto { Success = false, Message = $"Unsupported language: {detectedLanguage}" };
            }

            lastOutput = result;
            var expected = NormalizeLineEndings((exercise.ExpectedOutput ?? string.Empty).Trim());
            var actual = NormalizeLineEndings((result ?? string.Empty).Trim());
            allTestsPassed = string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase);

            // Create a single test case result for backward compatibility
            testCaseResults.Add(new TestCaseResultDto
            {
                TestCaseId = Guid.Empty,
                Description = "Output validation",
                Passed = allTestsPassed,
                ExpectedOutput = expected,
                ActualOutput = actual,
                Input = null
            });
        }

        var isCorrect = allTestsPassed;

        // Only save submission if there's an exercise ID
        if (request.ExerciseId.HasValue)
        {
            var submission = await _exerciseSubmissionRepository.GetAsync(userId, request.ExerciseId.Value);
            if (submission == null)
            {
                submission = new ExerciseSubmission
                {
                    Id = Guid.NewGuid(),
                    ExerciseId = request.ExerciseId.Value,
                    UserId = userId,
                    SubmittedCode = request.Code,
                    Output = lastOutput,
                    IsCorrect = isCorrect,
                    SubmissionDate = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };
                await _exerciseSubmissionRepository.AddAsync(submission);
            }
            else
            {
                submission.SubmittedCode = request.Code;
                submission.Output = lastOutput;
                submission.IsCorrect = submission.IsCorrect || isCorrect; // never regress from correct → wrong
                submission.SubmissionDate = DateTime.UtcNow;
            }

            await _unitOfWork.SaveChangesAsync();
        }

        var passedTests = testCaseResults.Count(tc => tc.Passed);
        var totalTests = testCaseResults.Count;

        return new SubmitExerciseResultDto
        {
            Success = true,
            Output = lastOutput,
            IsCorrect = isCorrect,
            TestCaseResults = testCaseResults,
            PassedTests = passedTests,
            TotalTests = totalTests,
            Message = isCorrect 
                ? $"All tests passed! ({passedTests}/{totalTests})" 
                : $"Some tests failed. Passed: {passedTests}/{totalTests}"
        };
    }

    // Converts "weight=70, height=1.75" or "70 1.75" to "70\n1.75\n" for Java Scanner stdin
    private string ParseJavaStdin(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return "";

        if (input.Contains("="))
        {
            // key=value, key2=value2  →  extract values only
            var values = input.Split(',')
                .Select(part =>
                {
                    var idx = part.IndexOf('=');
                    return idx >= 0 ? part.Substring(idx + 1).Trim() : part.Trim();
                })
                .Where(v => !string.IsNullOrWhiteSpace(v));
            return string.Join("\n", values) + "\n";
        }

        // Plain space-separated values → one per line
        return input.Trim().Replace(" ", "\n") + "\n";
    }

    private string InjectInputIntoCode(string code, string input, string language)
    {
        // Basic input injection - for simple cases where input is passed as arguments
        // This is a simplified version. For production, you'd want more sophisticated handling
        
        if (language.Equals("java", StringComparison.OrdinalIgnoreCase))
        {
            // For Java, try to inject input as command-line arguments or modify System.in
            // This is a basic implementation - you might need to adjust based on your needs
            if (code.Contains("String[] args"))
            {
                // Try to set args - this is simplified
                return code; // For now, return as-is. You'd need a more sophisticated approach
            }
        }
        else if (language.Equals("python", StringComparison.OrdinalIgnoreCase))
        {
            // For Python, detect function definitions and call them with test inputs
            return WrapPythonCodeWithTestExecution(code, input);
        }

        return code;
    }

    private string WrapPythonCodeWithTestExecution(string code, string input)
    {
        // If code already has print statements or execution, return as-is
        if (code.Contains("print(") && !code.Trim().EndsWith(":"))
        {
            return code;
        }

        // Try to detect function definitions
        var functionMatches = System.Text.RegularExpressions.Regex.Matches(
            code, 
            @"def\s+(\w+)\s*\(([^)]*)\)\s*:",
            System.Text.RegularExpressions.RegexOptions.Multiline
        );

        if (functionMatches.Count > 0)
        {
            var functionName = functionMatches[0].Groups[1].Value;
            var paramString = functionMatches[0].Groups[2].Value;
            var parameters = paramString.Split(',')
                .Select(p => p.Trim().Split('=')[0].Trim())
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .ToList();

            // Parse input if provided (format: "param1=value1, param2=value2" or "value1, value2")
            var testValues = ParseTestInput(input, parameters.Count);
            
            // Build function call
            string functionCall;
            if (testValues.Count == parameters.Count && testValues.Count > 0)
            {
                // Use parsed values with parameter names
                var paramValuePairs = new List<string>();
                for (int i = 0; i < parameters.Count && i < testValues.Count; i++)
                {
                    paramValuePairs.Add($"{parameters[i]}={testValues[i]}");
                }
                functionCall = $"{functionName}({string.Join(", ", paramValuePairs)})";
            }
            else if (parameters.Count == 2)
            {
                // Default test values for common cases
                // For calculate_area: length=4, width=5 -> Area: 20
                if (functionName.ToLower().Contains("area") || functionName.ToLower().Contains("calculate"))
                {
                    functionCall = $"{functionName}(4, 5)";
                }
                else
                {
                    functionCall = $"{functionName}(4, 5)"; // Default to 4, 5 for 2-param functions
                }
            }
            else if (parameters.Count > 0)
            {
                // Generic: use 1, 2, 3... as default values
                functionCall = $"{functionName}({string.Join(", ", parameters.Select((p, i) => $"{i + 1}"))})";
            }
            else
            {
                // No parameters
                functionCall = $"{functionName}()";
            }

            // Determine output format based on function name or use generic format
            string outputFormat = "Area: {result}";
            if (functionName.ToLower().Contains("sum"))
                outputFormat = "Sum: {result}";
            else if (functionName.ToLower().Contains("area"))
                outputFormat = "Area: {result}";
            else if (functionName.ToLower().Contains("calculate"))
                outputFormat = "Result: {result}";
            else
                outputFormat = "{result}";

            // Wrap code to call function and print result
            return $"{code}\n\n# Test execution\nresult = {functionCall}\nprint(f\"{outputFormat}\")";
        }

        // No function detected, return as-is
        return code;
    }

    private List<string> ParseTestInput(string input, int expectedParamCount)
    {
        var values = new List<string>();
        
        if (string.IsNullOrWhiteSpace(input))
            return values;

        // Try to parse formats like:
        // "length=4, width=5"
        // "4, 5"
        // "4 5"
        
        // Check for key=value format
        if (input.Contains("="))
        {
            var pairs = input.Split(',');
            foreach (var pair in pairs)
            {
                var parts = pair.Trim().Split('=');
                if (parts.Length == 2)
                {
                    values.Add(parts[1].Trim());
                }
            }
        }
        else
        {
            // Try comma or space separated values
            var separators = new[] { ',', ' ', '\t' };
            values.AddRange(input.Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.Trim())
                .Where(v => !string.IsNullOrWhiteSpace(v)));
        }

        return values;
    }

    private static string NormalizeLineEndings(string s) =>
        s.Replace("\r\n", "\n").Replace("\r", "\n");

    private string DetectLanguageFromCourse(Course course)
    {
        // First, check ProgrammingLanguage enum
        if (course.ProgrammingLanguage.HasValue)
        {
            var lang = course.ProgrammingLanguage.Value.ToString().ToLower();
            Console.WriteLine($"[DetectLanguageFromCourse] Using ProgrammingLanguage enum: {lang}");
            return lang;
        }

        // Fallback to LearningPath
        var learningPath = course.LearningPath?.ToLower() ?? "";
        Console.WriteLine($"[DetectLanguageFromCourse] Checking LearningPath: '{learningPath}'");
        
        if (learningPath.Contains("python"))
        {
            Console.WriteLine($"[DetectLanguageFromCourse] Detected Python from LearningPath");
            return "python";
        }
        else if (learningPath.Contains("java"))
        {
            Console.WriteLine($"[DetectLanguageFromCourse] Detected Java from LearningPath");
            return "java";
        }

        // Default to Java if nothing matches (for backward compatibility)
        Console.WriteLine($"[DetectLanguageFromCourse] WARNING: No language detected, defaulting to Java");
        return "java";
    }
}

