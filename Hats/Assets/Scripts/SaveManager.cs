using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager
{
    public SaveGlob saveGlob;    // the Dictionary used to save and load data to/from disk
    protected string savePath;

    public SaveManager()
    {
        this.savePath = Application.persistentDataPath + "/save.dat";
        this.saveGlob = new SaveGlob();
        this.loadDataFromDisk();   
    }
    /**
     * Saves the save data to the disk
     */
    public void saveDataToDisk()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath);
        bf.Serialize(file, saveGlob);
        file.Close();
        Debug.Log("Saved");
    }

    /**
     * Loads the save data from the disk
     */
    public void loadDataFromDisk()
    {
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