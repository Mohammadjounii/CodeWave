using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CodeWave.Web
{
    public static class JavaRunner
    {
        public static async Task<string> RunAsync(string code)
        {
            // Create temp directory
            var tempDir = Path.Combine(Path.GetTempPath(), "CodeWaveJava");
            Directory.CreateDirectory(tempDir);

            var className = "Main"; // assume public class Main
            var javaFile = Path.Combine(tempDir, $"{className}.java");
            await File.WriteAllTextAsync(javaFile, code, Encoding.UTF8);

            // Compile
            var compileOutput = await RunProcessAsync("javac", $"{className}.java", tempDir);
            if (!string.IsNullOrWhiteSpace(compileOutput.stderr))
                return "Compile error:\n" + compileOutput.stderr;

            // Run
            var runOutput = await RunProcessAsync("java", className, tempDir);
            if (!string.IsNullOrWhiteSpace(runOutput.stderr))
                return "Runtime error:\n" + runOutput.stderr;

            return runOutput.stdout;
        }

        private static async Task<(string stdout, string stderr)> RunProcessAsync(
            string fileName, string arguments, string workingDirectory)
        {
            var psi = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            var process = new Process { StartInfo = psi };
            var stdout = new StringBuilder();
            var stderr = new StringBuilder();

            process.OutputDataReceived += (s, e) =>
            {
                if (e.Data != null) stdout.AppendLine(e.Data);
            };
            process.ErrorDataReceived += (s, e) =>
            {
                if (e.Data != null) stderr.AppendLine(e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            await process.WaitForExitAsync();

            return (stdout.ToString(), stderr.ToString());
        }
    }
}
