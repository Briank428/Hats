
[System.Serializable]
public class Leaderboard
{
    public string name;
    public int score;

    public Leaderboard(string n, int s)
    {
        name = n;
        score = s;
    }
    public string GetName() { return name; }
    public int GetScore() { return score;  }
}
