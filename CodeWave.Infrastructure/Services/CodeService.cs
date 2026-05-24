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
        if (request == null || string.IsNullOrWhiteSpace(request.Code))
        {
            return new SubmitExerciseResultDto
            {
                Success = false,
                Message = "Invalid request."
            };
        }

        string detectedLanguage;
        Course? course = null;

        if (!request.ExerciseId.HasValue)
        {
            detectedLanguage = !string.IsNullOrWhiteSpace(request.Language)
                ? request.Language.Trim().ToLower()
                : "python";

            try
            {
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
                    return new SubmitExerciseResultDto
                    {
                        Success = false,
                        Message = $"Unsupported language: {detectedLanguage}"
                    };
                }

                return new SubmitExerciseResultDto
                {
                    Success = true,
                    IsCorrect = false,
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

        var exercise = await _exerciseRepository.GetWithLessonAsync(request.ExerciseId.Value);
        if (exercise == null || exercise.Lesson == null)
        {
            return new SubmitExerciseResultDto
            {
                Success = false,
                Message = "Exercise not found."
            };
        }

        course = await _courseRepository.GetByIdAsync(exercise.Lesson.CourseId);
        if (course == null)
        {
            return new SubmitExerciseResultDto
            {
                Success = false,
                Message = "Course not found."
            };
        }

        detectedLanguage = DetectLanguageFromCourse(course);

        var testCases = exercise.TestCases
            .Where(tc => !tc.IsDeleted)
            .OrderBy(tc => tc.OrderNumber)
            .ToList();

        var testCaseResults = new List<TestCaseResultDto>();
        bool allTestsPassed = true;
        string lastOutput = string.Empty;

        if (testCases.Any())
        {
            foreach (var testCase in testCases)
            {
                // Code-text keyword check — no execution needed
                if (string.Equals(testCase.Input?.Trim(), "__code__", StringComparison.OrdinalIgnoreCase))
                {
                    var keyword = (testCase.ExpectedOutput ?? string.Empty).Trim();
                    bool found = request.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase);
                    testCaseResults.Add(new TestCaseResultDto
                    {
                        TestCaseId = testCase.Id,
                        OrderNumber = testCase.OrderNumber,
                        Description = testCase.Description,
                        Passed = found,
                        ExpectedOutput = keyword,
                        ActualOutput = found ? $"Found \"{keyword}\" in your code" : $"Missing \"{keyword}\" in your code",
                        Input = testCase.Input
                    });
                    if (!found) allTestsPassed = false;
                    continue;
                }

                string codeToRun = request.Code;

                if (detectedLanguage.Equals("python", StringComparison.OrdinalIgnoreCase))
                {
                    codeToRun = InjectInputIntoCode(request.Code, testCase.Input ?? "", detectedLanguage);
                }

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
                        return new SubmitExerciseResultDto
                        {
                            Success = false,
                            Message = $"Unsupported language: {detectedLanguage}"
                        };
                    }
                }
                catch (Exception ex)
                {
                    testCaseResults.Add(new TestCaseResultDto
                    {
                        TestCaseId = testCase.Id,
                        OrderNumber = testCase.OrderNumber,
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

                bool testPassed = actual.Trim().Contains(expected.Trim(), StringComparison.OrdinalIgnoreCase);

                testCaseResults.Add(new TestCaseResultDto
                {
                    TestCaseId = testCase.Id,
                    OrderNumber = testCase.OrderNumber,
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
                return new SubmitExerciseResultDto
                {
                    Success = false,
                    Message = $"Unsupported language: {detectedLanguage}"
                };
            }

            lastOutput = result;

            var expected = NormalizeLineEndings((exercise.ExpectedOutput ?? string.Empty).Trim());
            var actual = NormalizeLineEndings((result ?? string.Empty).Trim());

            allTestsPassed = string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase);

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
        var passedTests = testCaseResults.Count(tc => tc.Passed);
        var totalTests = testCaseResults.Count;
        var outputSummary = $"Passed {passedTests}/{totalTests} tests. Last output: {lastOutput}";

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
                    Output = outputSummary,
                    IsCorrect = isCorrect,
                    SubmissionDate = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };

                await _exerciseSubmissionRepository.AddAsync(submission);
            }
            else
            {
                submission.SubmittedCode = request.Code;
                submission.Output = outputSummary;
                submission.IsCorrect = submission.IsCorrect || isCorrect;
                submission.SubmissionDate = DateTime.UtcNow;
            }

            await _unitOfWork.SaveChangesAsync();
        }

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

    private string ParseJavaStdin(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "";

        if (input.Contains("="))
        {
            var values = input.Split(',')
                .Select(part =>
                {
                    var idx = part.IndexOf('=');
                    return idx >= 0 ? part[(idx + 1)..].Trim() : part.Trim();
                })
                .Where(v => !string.IsNullOrWhiteSpace(v));

            return string.Join("\n", values) + "\n";
        }

        return input.Trim().Replace(" ", "\n") + "\n";
    }

    private string InjectInputIntoCode(string code, string input, string language)
    {
        if (language.Equals("python", StringComparison.OrdinalIgnoreCase))
        {
            return WrapPythonCodeWithTestExecution(code, input);
        }

        return code;
    }

    private string WrapPythonCodeWithTestExecution(string code, string input)
    {
        var functionMatches = System.Text.RegularExpressions.Regex.Matches(
            code,
            @"^def\s+(\w+)\s*\(([^)]*)\)\s*:",
            System.Text.RegularExpressions.RegexOptions.Multiline
        );

        if (functionMatches.Count == 0)
        {
            return code;
        }

        var functionName = functionMatches[0].Groups[1].Value;
        var paramString = functionMatches[0].Groups[2].Value;

        var parameters = paramString.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Trim().Split('=')[0].Trim())
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .ToList();

        var testValues = ParseTestInput(input, parameters.Count);

        string functionCall;
        if (testValues.Count == parameters.Count && testValues.Count > 0)
        {
            functionCall = $"{functionName}({string.Join(", ", testValues)})";
        }
        else if (parameters.Count > 0)
        {
            functionCall = $"{functionName}({string.Join(", ", Enumerable.Repeat("1", parameters.Count))})";
        }
        else
        {
            functionCall = $"{functionName}()";
        }

        return $"{code}\n\nresult = {functionCall}\nprint(result)";
    }

    private List<string> ParseTestInput(string input, int expectedParamCount)
    {
        var values = new List<string>();

        if (string.IsNullOrWhiteSpace(input))
            return values;

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
        if (course.ProgrammingLanguage.HasValue)
        {
            return course.ProgrammingLanguage.Value.ToString().ToLower();
        }

        var learningPath = course.LearningPath?.ToLower() ?? "";

        if (learningPath.Contains("python"))
            return "python";

        if (learningPath.Contains("java"))
            return "java";

        return "java";
    }
}