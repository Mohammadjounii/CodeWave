using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
namespace CodeWave.Infrastructure.Services
{
  

    public class CodeExecutionService
    {
        public async Task<ExecutionResult> RunCode(string language, string code)
        {
            string temp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(temp);

            string fileName = language switch
            {
                "java" => "Main.java",
                "python" => "main.py",
                _ => throw new Exception("Unsupported language")
            };

            string filePath = Path.Combine(temp, fileName);
            File.WriteAllText(filePath, code);

            string image = language switch
            {
                "java" => "codewave-java",
                "python" => "codewave-python",
                _ => throw new Exception("Invalid language")
            };

            string command =
                $"docker run --rm -v \"{temp}:/sandbox\" --memory=256m --cpus=0.5 {image}";

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {command}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            string output = await proc.StandardOutput.ReadToEndAsync();
            string error = await proc.StandardError.ReadToEndAsync();
            await proc.WaitForExitAsync();

            Directory.Delete(temp, true);

            return new ExecutionResult
            {
                Output = output,
                Error = error,
                ExitCode = proc.ExitCode
            };
        }
    }

    public class ExecutionResult
    {
        public string Output { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public int ExitCode { get; set; }
    }

}
