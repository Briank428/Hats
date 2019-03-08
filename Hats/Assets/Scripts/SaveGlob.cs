using System.Collections.Generic;

[System.Serializable]
public class SaveGlob
{
    public int totalAnvilsFallen;
    public int highscore;
    public List<Achievements> completedAchievements;
    public List<Leaderboard> leaderboard;

    public SaveGlob()
    {
        completedAchievements = new List<Achievements>();
        highscore = 0;
        totalAnvilsFallen = 0;
        leaderboard = new List<Leaderboard>();
    }
}
