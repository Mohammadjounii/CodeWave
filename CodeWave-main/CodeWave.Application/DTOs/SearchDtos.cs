namespace CodeWave.Application.DTOs
{
    public class SearchResultDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Excerpt { get; set; } = string.Empty;
        public string? LearningPath { get; set; }
        public string? DifficultyLevel { get; set; }
        public string Url { get; set; } = string.Empty;
        public string TypeIcon { get; set; } = string.Empty;
        public string TypeColor { get; set; } = string.Empty;
    }
}
