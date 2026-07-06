namespace CodeWave.Domain.Entities;

public class UserInterviewPractice
{
    public Guid      Id         { get; set; } = Guid.NewGuid();
    public Guid      UserId     { get; set; }
    public Guid      QuestionId { get; set; }
    public int       SelfRating { get; set; } = 0;  // 1-5, 0 = not rated
    public string?   Notes      { get; set; }
    public DateTime  PracticedAt { get; set; } = DateTime.UtcNow;

    public ApplicationUser      User     { get; set; } = null!;
    public InterviewQuestion    Question { get; set; } = null!;
}
