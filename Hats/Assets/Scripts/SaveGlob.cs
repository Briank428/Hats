using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SaveGlob
{
    public int totalAnvilsFallen;
    public List<Achievements> completedAchievements;
    public List<Leaderboard> leaderboard;
    public int totalDoctorsHats;

    public SaveGlob()
    {
        completedAchievements = new List<Achievements>();
        totalAnvilsFallen = 0;
        leaderboard = new List<Leaderboard>();
        totalDoctorsHats = 0;
    }
}
