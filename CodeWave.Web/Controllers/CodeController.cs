using CodeWave.Application.DTOs;
using CodeWave.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeWave.Web.Controllers
{
    [ApiController]
    [Route("api/code")]
    public class CodeController : ControllerBase
    {
        private readonly ICodeService _codeService;

        public CodeController(ICodeService codeService)
        {
            _codeService = codeService;
        }

        [HttpPost("run")]
        public async Task<IActionResult> RunCode([FromBody] RunCodeRequest req)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var result = await _codeService.RunAndSaveAsync(new RunCodeRequestDto
            {
                Language = req.Language,
                Code = req.Code,
                ExerciseId = req.ExerciseId
            }, userId);

            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(new
            {
                output = result.Output,
                isCorrect = result.IsCorrect,
                message = result.Message,
                testCaseResults = result.TestCaseResults,
                passedTests = result.PassedTests,
                totalTests = result.TotalTests
            });
        }
    }

    public class RunCodeRequest
    {
        public string Language { get; set; }
        public string Code { get; set; }
        public Guid? ExerciseId { get; set; } // Made nullable to allow running code without an exercise
    }
}
