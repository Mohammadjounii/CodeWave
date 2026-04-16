using CodeWave.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeWave.Application.Interfaces
{
    public interface IProgressService
    {
        Task<UserProgressDto> GetUserProgressAsync(Guid userId);
        Task<List<SkillProgressDto>> GetUserSkillsAsync(Guid userId, string learningPath);
        Task<List<WeaknessDto>> GetUserWeaknessesAsync(Guid userId, string learningPath);
        Task<int> GetTotalStudyTimeMinutesAsync(Guid userId, string learningPath);
        Task<Dictionary<string, int>> GetStudyTimeByTopicAsync(Guid userId, string learningPath);
    }
}

