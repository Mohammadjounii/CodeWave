using CodeWave.Application.Assessments.DTOs;
using CodeWave.Application.Interfaces;
using CodeWave.Infrastructure.Data;
using CodeWave.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace CodeWave.Application.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly ApplicationDbContext _context;

        public AssessmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TakeAssessmentDto> GetAssessmentAsync(Guid assessmentId)
        {
            var assessment = await _context.Assessments
                .Include(a => a.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .FirstOrDefaultAsync(a => a.Id == assessmentId);

            if (assessment == null)
            {
                throw new InvalidOperationException($"Assessment with ID {assessmentId} not found.");
            }

            return new TakeAssessmentDto
            {
                AssessmentId = assessment.Id,
                Title = assessment.Title,
                Questions = assessment.Questions.Select(q => new QuestionDto
                {
                    QuestionId = q.Id,
                    Text = q.Text,
                    Options = q.AnswerOptions
                        .Select(o => new AnswerOptionDto
                        {
                            AnswerOptionId = o.Id,
                            Text = o.Text
                        }).ToList()
                }).ToList()
            };
        }

        public async Task<SubmitAssessmentResultDto> SubmitAsync(SubmitAssessmentDto dto)
        {
            var questions = await _context.Questions
                .Where(q => q.AssessmentId == dto.AssessmentId)
                .Include(q => q.AnswerOptions)
                .ToListAsync();

            int correct = 0;
            int total = questions.Count;

            var userAssessment = new UserAssesment
            {
                Id = Guid.NewGuid(),
                AssessmentId = dto.AssessmentId,
                UserId = dto.UserId,
                DateTaken = DateTime.UtcNow,
                UserAnswers = new List<UserAnswer>()
            };

            foreach (var q in questions)
            {
                var selectedOptionId = dto.Answers[q.Id];
                var selectedOption = q.AnswerOptions.First(o => o.Id == selectedOptionId);

                if (selectedOption.IsCorrect)
                    correct++;

                userAssessment.UserAnswers.Add(new UserAnswer
                {
                    Id = Guid.NewGuid(),
                    QuestionId = q.Id,
                    AnswerOptionId = selectedOption.Id,
                    IsCorrect = selectedOption.IsCorrect,
                    UserAssessmentId = userAssessment.Id
                });
            }

            double score = (double)correct / total * 100;

            string level =
                score >= 80 ? "Advanced" :
                score >= 50 ? "Intermediate" :
                "Beginner";

            string learningPath =
                level switch
                {
                    "Beginner" => "Python",
                    "Intermediate" => "Web Development",
                    "Advanced" => "Java",
                    _ => "Python"
                };

            userAssessment.Score = score;
            userAssessment.ResultLevel = level;
            userAssessment.AssignedLearningPath = learningPath;

            // Save the result
            await _context.UserAssessments.AddAsync(userAssessment);

            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
            {
                throw new InvalidOperationException($"User with ID {dto.UserId} not found.");
            }
            user.Level = level;
            user.LearningPath = learningPath;

            await _context.SaveChangesAsync();

            return new SubmitAssessmentResultDto
            {
                Score = score,
                Level = level,
                LearningPath = learningPath
            };
        }
    }
}
