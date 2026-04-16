using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

public class CodeRunnerService
{
    private readonly string _root;

    public CodeRunnerService()
    {
        _root = Path.Combine(Path.GetTempPath(), "CodeWaveRunner");
        if (!Directory.Exists(_root))
            Directory.CreateDirectory(_root);
    }

    // ---------------- PYTHON ----------------
    public async Task<string> RunPythonAsync(string code)
    {
        string folder = CreateTempFolder();
        string file = Path.Combine(folder, "script.py");

        await File.WriteAllTextAsync(file, code);

        // Try python3 first (macOS/Linux default), fallback to python (Windows)
        string pythonCommand = GetPythonCommand();
        return await RunProcessAsync(pythonCommand, $"\"{file}\"", folder);
    }

    private string GetPythonCommand()
    {
        // On macOS and Linux, Python 3 is typically 'python3'
        // On Windows, it's usually 'python'
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            return "python";
        }
        
        // macOS and Linux: use python3
        return "python3";
    }


    // ---------------- JAVA ----------------
    public async Task<string> RunJavaAsync(string code, string stdin = "")
    {
        string folder = CreateTempFolder();
        string file = Path.Combine(folder, "Main.java");

        await File.WriteAllTextAsync(file, code);

        // compile
        string compileOutput = await RunProcessAsync("javac", "Main.java", folder);
        if (!string.IsNullOrWhiteSpace(compileOutput))
            return "Compilation Error:\n" + compileOutput;

        // run with optional stdin
        return await RunJavaProcessWithStdinAsync("java", "Main", folder, stdin);
    }

    private async Task<string> RunJavaProcessWithStdinAsync(string exe, string args, string workingDir, string stdin)
    {
        try
        {
            var psi = new ProcessStartInfo(exe, args)
            {
                RedirectStandardOutput = true,
                RedirectStandardError  = true,
                RedirectStandardInput  = true,
                WorkingDirectory       = workingDir,
                CreateNoWindow         = true,
                UseShellExecute        = false
            };

            using var process = Process.Start(psi);
            if (process == null)
                return "Execution Error: Failed to start process.";

            if (!string.IsNullOrEmpty(stdin))
            {
                await process.StandardInput.WriteAsync(stdin);
                process.StandardInput.Close();
            }

            string stdout = await process.StandardOutput.ReadToEndAsync();
            string stderr = await process.StandardError.ReadToEndAsync();

            process.WaitForExit(3000);

            if (!process.HasExited)
            {
                process.Kill();
                return "⛔ Timeout: Program took too long to run.";
            }

            // Prefer stdout; fall back to stderr only when stdout is empty (e.g. uncaught exceptions)
            return !string.IsNullOrWhiteSpace(stdout) ? stdout : stderr;
        }
        catch (Exception ex)
        {
            return "Execution Error: " + ex.Message;
        }
    }


    // ---------------- UTILITIES ----------------
    private async Task<string> RunProcessAsync(string exe, string args, string workingDir)
    {
        try
        {
            var psi = new ProcessStartInfo(exe, args)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = workingDir,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            using var process = Process.Start(psi);
            if (process == null)
            {
                return "Execution Error: Failed to start process.";
            }

            string stdout = await process.StandardOutput.ReadToEndAsync();
            string stderr = await process.StandardError.ReadToEndAsync();

            process.WaitForExit(3000); // 3-second timeout

            if (!process.HasExited)
            {
                process.Kill();
                return "⛔ Timeout: Program took too long to run.";
            }

            return string.IsNullOrWhiteSpace(stderr) ? stdout : stderr;
        }
        catch (Exception ex)
        {
            return "Execution Error: " + ex.Message;
        }
    }

    private string CreateTempFolder()
    {
        string folder = Path.Combine(_root, Guid.NewGuid().ToString());
        Directory.CreateDirectory(folder);
        return folder;
    }
}

