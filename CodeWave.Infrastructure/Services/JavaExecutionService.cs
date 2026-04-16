using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CodeWave.Infrastructure.Services
{
    public class JavaExecutionResult
    {
        public string Stdout { get; set; } = string.Empty;
        public string Stderr { get; set; } = string.Empty;
        public bool TimedOut { get; set; }
        public int ExitCode { get; set; }
        public bool CompileError { get; set; }
    }

    public interface ICodeExecutionService
    {
        /// <summary>
        /// Compiles and runs Java code. The provided code must include a public class named Main.
        /// </summary>
        Task<JavaExecutionResult> CompileAndRunJavaAsync(string javaSource, TimeSpan timeout, int maxSourceLength = 5000);
    }

    public class JavaExecutionService : ICodeExecutionService
    {
        // NOTE: ensure 'javac' and 'java' are available on the server PATH
        public async Task<JavaExecutionResult> CompileAndRunJavaAsync(string javaSource, TimeSpan timeout, int maxSourceLength = 5000)
        {
            var result = new JavaExecutionResult();

            if (string.IsNullOrWhiteSpace(javaSource))
            {
                result.Stderr = "No code provided.";
                return result;
            }

            if (javaSource.Length > maxSourceLength)
            {
                result.Stderr = $"Source too long. Max allowed: {maxSourceLength} characters.";
                return result;
            }

            // Simple safety check: require a public class Main (you can change)
            if (!javaSource.Contains("class Main"))
            {
                result.Stderr = "Source must contain a `class Main` declaration.";
                return result;
            }

            var tmpDir = Path.Combine(Path.GetTempPath(), "codewave_java_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tmpDir);

            var sourceFile = Path.Combine(tmpDir, "Main.java");
            await File.WriteAllTextAsync(sourceFile, javaSource);

            using var cts = new CancellationTokenSource();
            cts.CancelAfter(timeout);

            try
            {
                // 1) Compile
                var javacInfo = new ProcessStartInfo
                {
                    FileName = "javac",
                    Arguments = $"\"{sourceFile}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = tmpDir
                };

                using (var compileProc = Process.Start(javacInfo)!)
                {
                    var compileStdout = await compileProc.StandardOutput.ReadToEndAsync();
                    var compileStderr = await compileProc.StandardError.ReadToEndAsync();

                    var compileTask = compileProc.WaitForExitAsync(cts.Token);

                    try { await compileTask; }
                    catch (OperationCanceledException)
                    {
                        compileProc.Kill(true);
                        result.TimedOut = true;
                        result.Stderr = "Compilation timed out.";
                        return result;
                    }

                    if (compileProc.ExitCode != 0)
                    {
                        result.CompileError = true;
                        result.Stderr = compileStderr + (string.IsNullOrWhiteSpace(compileStderr) ? compileStdout : string.Empty);
                        return result;
                    }
                }

                // 2) Run
                var runInfo = new ProcessStartInfo
                {
                    FileName = "java",
                    Arguments = $"-cp \"{tmpDir}\" Main",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = tmpDir
                };

                using (var runProc = Process.Start(runInfo)!)
                {
                    var stdoutTask = runProc.StandardOutput.ReadToEndAsync();
                    var stderrTask = runProc.StandardError.ReadToEndAsync();

                    var waitTask = runProc.WaitForExitAsync(cts.Token);

                    try
                    {
                        await waitTask;
                    }
                    catch (OperationCanceledException)
                    {
                        try { runProc.Kill(true); } catch { }
                        result.TimedOut = true;
                        result.Stderr = "Execution timed out.";
                        return result;
                    }

                    result.ExitCode = runProc.ExitCode;
                    result.Stdout = await stdoutTask;
                    result.Stderr = await stderrTask;
                }

                return result;
            }
            finally
            {
                // Clean up the temp directory (best-effort)
                try
                {
                    Directory.Delete(tmpDir, true);
                }
                catch { /* swallow cleanup exceptions */ }
            }
        }
    }
}