using CodeWave.Application.Assessments.DTOs;
using CodeWave.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeWave.Web.Controllers
{

    [Authorize]
    public class AssessmentController : Controller
    {
        private readonly IAssessmentService _assessmentService;

        public AssessmentController(IAssessmentService assessmentService)
        {
            _assessmentService = assessmentService;
        }

        public async Task<IActionResult> Start()
        {
            var assessmentId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var assessment = await _assessmentService.GetAssessmentAsync(assessmentId);

            if (assessment == null)
                return RedirectToAction("Index", "Home");

            HttpContext.Session.SetString("TotalQuestions", assessment.Questions.Count.ToString());
            HttpContext.Session.SetString("AssessmentJson", System.Text.Json.JsonSerializer.Serialize(assessment));
            HttpContext.Session.SetString("CurrentIndex", "0");
            HttpContext.Session.SetString("UserAnswers", "{}");

            return RedirectToAction("Question");
        }

        public IActionResult Question()
        {
            var assessmentJson = HttpContext.Session.GetString("AssessmentJson");
            if (string.IsNullOrEmpty(assessmentJson))
            {
                return RedirectToAction("Start");
            }

            var assessment = System.Text.Json.JsonSerializer.Deserialize<TakeAssessmentDto>(assessmentJson);
            if (assessment == null)
                return RedirectToAction("Start");

            var indexStr = HttpContext.Session.GetString("CurrentIndex");
            int index = int.TryParse(indexStr, out var parsed) ? parsed : 0;

            if (index >= assessment.Questions.Count)
                return RedirectToAction("Finish");

            var question = assessment.Questions[index];

            ViewBag.Index = index + 1;
            ViewBag.Total = assessment.Questions.Count;
            ViewBag.Progress = ((double)index / assessment.Questions.Count) * 100;
            ViewBag.HasPrevious = index > 0;

            return View("TakeSingle", question);
        }

        [HttpPost]
        public IActionResult Next(Guid questionId, Guid? selectedOptionId)
        {
            var json = HttpContext.Session.GetString("UserAnswers") ?? "{}";
            var answers = System.Text.Json.JsonSerializer.Deserialize<Dictionary<Guid, Guid>>(json) ?? new Dictionary<Guid, Guid>();

            if (selectedOptionId.HasValue)
                answers[questionId] = selectedOptionId.Value;

            HttpContext.Session.SetString("UserAnswers",
                System.Text.Json.JsonSerializer.Serialize(answers));

            var indexStr = HttpContext.Session.GetString("CurrentIndex");
            int index = int.TryParse(indexStr, out var parsed) ? parsed : 0;
            index++;
            HttpContext.Session.SetString("CurrentIndex", index.ToString());

            return RedirectToAction("Question");
        }

        [HttpGet]
        public IActionResult Previous()
        {
            var indexStr = HttpContext.Session.GetString("CurrentIndex");
            int index = int.TryParse(indexStr, out var parsed) ? parsed : 0;
            if (index > 0)
            {
                index--;
                HttpContext.Session.SetString("CurrentIndex", index.ToString());
            }
            return RedirectToAction("Question");
        }

        public async Task<IActionResult> Finish()
        {
            var assessmentJson = HttpContext.Session.GetString("AssessmentJson");
            if (string.IsNullOrEmpty(assessmentJson))
                return RedirectToAction("Start");

            var assessment = System.Text.Json.JsonSerializer.Deserialize<TakeAssessmentDto>(assessmentJson);
            if (assessment == null)
                return RedirectToAction("Start");

            var answersJson = HttpContext.Session.GetString("UserAnswers") ?? "{}";
            var answers = System.Text.Json.JsonSerializer.Deserialize<Dictionary<Guid, Guid>>(answersJson) ?? new Dictionary<Guid, Guid>();

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return RedirectToAction("Login", "User");
            }

            var result = await _assessmentService.SubmitAsync(new SubmitAssessmentDto
            {
                AssessmentId = assessment.AssessmentId,
                UserId = userId,
                Answers = answers
            });

            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }

}
