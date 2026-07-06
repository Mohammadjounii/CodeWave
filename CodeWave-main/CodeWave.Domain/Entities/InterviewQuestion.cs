namespace CodeWave.Domain.Entities;

public class InterviewQuestion
{
    public Guid   Id          { get; set; } = Guid.NewGuid();
    public string Question    { get; set; } = string.Empty;
    public string ModelAnswer { get; set; } = string.Empty;
    public string Category    { get; set; } = string.Empty;  // Python | Java | Web Dev | General CS | Algorithms | Behavioral
    public string Difficulty  { get; set; } = string.Empty;  // Beginner | Intermediate | Advanced
    public string Tags        { get; set; } = string.Empty;  // comma-separated
    public int    OrderNumber { get; set; } = 0;

    public ICollection<UserInterviewPractice> Practices { get; set; } = new List<UserInterviewPractice>();
}
