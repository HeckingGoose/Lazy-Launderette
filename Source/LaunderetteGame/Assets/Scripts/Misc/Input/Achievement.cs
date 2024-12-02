public class AchievementData
{
    // Variables
    public int AchievementID { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    // Constructor
    public AchievementData(
        int achievementID,
        string title,
        string description
        )
    {
        // Pass in values
        AchievementID = achievementID;
        Title = title;
        Description = description;
    }
}