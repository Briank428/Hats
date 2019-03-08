using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Achievements
{
    public string name, description;
    public Achievements(string n, string d)
    {
        name = n;
        description = d;
    }

    public static bool operator ==(Achievements a, Achievements b)
    {
        if (a.name == b.name) return true;
        return false;
    }

    public static bool operator !=(Achievements a, Achievements b)
    {
        if (a.name != b.name) return true;
        return false;
    }
    #region ignore
    public override bool Equals(object obj)
    {
        var achievements = obj as Achievements;
        return achievements != null &&
               name == achievements.name &&
               description == achievements.description;
    }

    public override int GetHashCode()
    {
        var hashCode = 1243218751;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(description);
        return hashCode;
    }
    #endregion
}
