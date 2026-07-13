namespace CodeWave.Domain.Entities;

public class Achievement
{
    public Guid   Id          { get; set; } = Guid.NewGuid();
    public string Key         { get; set; } = string.Empty;  // unique slug e.g. "first_lesson"
    public string Name        { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon        { get; set; } = "🏅";           // emoji displayed on the badge
    public string GradientFrom { get; set; } = "#6366f1";
    public string GradientTo   { get; set; } = "#8b5cf6";
    public string Category    { get; set; } = "Learning";     // Learning | Quizzes | Career | Milestones
    public int    XPValue     { get; set; } = 50;

    public ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
}
