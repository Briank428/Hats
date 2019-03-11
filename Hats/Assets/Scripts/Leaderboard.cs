
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

    public static bool operator >(Leaderboard a, Leaderboard b)
    {
        if (a.score > b.score) return true;
        if (b == null) return true;
        return false;
    }
    public static bool operator <(Leaderboard a, Leaderboard b)
    {
        if (a.score < b.score) return true;
        if (a == null) return true;
        return false;
    }
}
