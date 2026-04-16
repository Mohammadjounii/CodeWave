using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Services;

public class CVService : ICVService
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public CVService(ApplicationDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<CV?> GetCVByUserIdAsync(Guid userId)
    {
        return await _context.CVs
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<CV> CreateOrUpdateCVAsync(CV cv)
    {
        var existingCv = await GetCVByUserIdAsync(cv.UserId);
        
        if (existingCv == null)
        {
            cv.Id = Guid.NewGuid();
            cv.CreatedAt = DateTime.UtcNow;
            cv.LastUpdated = DateTime.UtcNow;
            _context.CVs.Add(cv);
        }
        else
        {
            // Update existing CV
            existingCv.FullName = cv.FullName;
            existingCv.Age = cv.Age;
            existingCv.Location = cv.Location;
            existingCv.Email = cv.Email;
            existingCv.Phone = cv.Phone;
            existingCv.LinkedInUrl = cv.LinkedInUrl;
            existingCv.GitHubUrl = cv.GitHubUrl;
            existingCv.Education = cv.Education;
            existingCv.EducationDetails = cv.EducationDetails;
            existingCv.ProgrammingLanguages = cv.ProgrammingLanguages;
            existingCv.SpokenLanguages = cv.SpokenLanguages;
            existingCv.Summary = cv.Summary;
            existingCv.Experience = cv.Experience;
            existingCv.Certifications = cv.Certifications;
            existingCv.Projects = cv.Projects;
            existingCv.Template = cv.Template;
            existingCv.Skills = cv.Skills;
            existingCv.CVPictureUrl = cv.CVPictureUrl;
            existingCv.UploadedCVFilePath = cv.UploadedCVFilePath;
            existingCv.UpgradedCVFilePath = cv.UpgradedCVFilePath;
            existingCv.GeneratedPDFPath = cv.GeneratedPDFPath;
            existingCv.LastUpdated = DateTime.UtcNow;
            
            if (existingCv.CreatedAt == default(DateTime))
            {
                existingCv.CreatedAt = DateTime.UtcNow;
            }
            
            _context.CVs.Update(existingCv);
            cv = existingCv;
        }

        await _unitOfWork.SaveChangesAsync();
        return cv;
    }
}

