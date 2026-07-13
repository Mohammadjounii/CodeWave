using CodeWave.Application.Assessments.DTOs;
using System;
using System.Threading.Tasks;

namespace CodeWave.Application.Interfaces
{
    public interface IAssessmentService
    {
        Task<TakeAssessmentDto> GetAssessmentAsync(Guid assessmentId);
        Task<SubmitAssessmentResultDto> SubmitAsync(SubmitAssessmentDto dto);
    }
}
