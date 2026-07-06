using System;
using System.Threading.Tasks;
using CodeWave.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeWave.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeRunnerController : ControllerBase
    {
        private readonly ICodeExecutionService _exec;

        public CodeRunnerController(ICodeExecutionService exec)
        {
            _exec = exec;
        }

        public class RunRequest
        {
            public string Code { get; set; } = string.Empty;
            public int TimeoutSeconds { get; set; } = 5;
        }

        [HttpPost("run-java")]
        public async Task<IActionResult> RunJava([FromBody] RunRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Code))
                return BadRequest(new { error = "Code is required." });

            var timeout = TimeSpan.FromSeconds(Math.Clamp(req.TimeoutSeconds, 1, 30));

            var result = await _exec.CompileAndRunJavaAsync(req.Code, timeout);

            return Ok(new
            {
                stdout = result.Stdout,
                stderr = result.Stderr,
                timedOut = result.TimedOut,
                exitCode = result.ExitCode,
                compileError = result.CompileError
            });
        }
    }
}
