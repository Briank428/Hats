using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SaveGlob
{
    public int totalAnvilsFallen;
    public int highscore;
    public Hashtable completedAchievements;
    public List<Leaderboard> leaderboard;
    public int totalDoctorsHats;
    public SaveGlob()
    {
        completedAchievements = new Hashtable();
        highscore = 0;
        totalAnvilsFallen = 0;
        leaderboard = new List<Leaderboard>();
        totalDoctorsHats = 0;
    }
}
