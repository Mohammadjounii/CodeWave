using CodeWave.Application.DTOs;

namespace CodeWave.Application.Interfaces
{
    public interface ISearchService
    {
        Task<List<SearchResultDto>> SearchAsync(string query, Guid? userId = null);
        Task<List<SearchResultDto>> SuggestAsync(string query, Guid? userId = null);
    }
}
