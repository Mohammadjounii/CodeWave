using AutoMapper;
using CodeWave.Domain.Entities;

namespace CodeWave.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Basic entity mappings - can be extended with DTOs as needed
        // This provides a foundation for AutoMapper usage in the application
        
        // Example: If you have DTOs, you can add mappings here
        // CreateMap<Course, CourseDto>();
        // CreateMap<CourseDto, Course>();
    }
}
