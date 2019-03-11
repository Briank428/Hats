using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SaveManager
{
    public SaveGlob saveGlob;    // the Dictionary used to save and load data to/from disk
    protected string savePath;

    public SaveManager()
    {
        this.savePath = Application.persistentDataPath + "/save.dat";
        Debug.Log(savePath);
        this.saveGlob = new SaveGlob();
        this.LoadDataFromDisk();   
    }
    /**
     * Saves the save data to the disk
     */
    public void SaveDataToDisk()
    {
        if (PlayerPrefs.GetString("Name") == "USE_4_TEST") return;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath);
        bf.Serialize(file, saveGlob);
        file.Close();
        Debug.Log("Saved");
    }

    /**
     * Loads the save data from the disk
     */
    public void LoadDataFromDisk()
    {
        if (PlayerPrefs.GetString("Name") == "USE_4_TEST")
        {
            this.saveGlob.completedAchievements = new List<Achievements>
            {
                new Achievements("Achievemnt 1", "this is a test"),
                new Achievements("Achievement 2", "also a test"),
                new Achievements("Achievement 3", "take a guess, its a test"),
                new Achievements("Achievement 4", "this here is an exam"),
                new Achievements("Achievement the seventeenth", "Hi")
                };
            this.saveGlob.leaderboard = new List<Leaderboard>
            {
                new Leaderboard("RealPlayer", 900),
                new Leaderboard("RealerPlayer",100),
                new Leaderboard("Ms Pacman", 85),
                new Leaderboard("Superman", 69),
                new Leaderboard("John Doe", 3)
            };
            return;
        }
        if (File.Exists(savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.Open);
            this.saveGlob = (SaveGlob)bf.Deserialize(file);
            file.Close();
            Debug.Log("Retrieved");
        }
        else { Debug.Log("New"); }
    }
}