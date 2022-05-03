using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;


[System.Serializable]
public class TrialData
{
    public List<Coord> headPoses = new List<Coord>();
    public string choice;
    public Coord orientation;
    public float tiltDegree;
}

[System.Serializable]
public class Coord
{
    public float x;
    public float y;
    public float z;
}

[System.Serializable]
public class Trial
{
    public string scene;
    public TrialData trialData;
}

[System.Serializable]
public class JsonDummy
{
    public List<Trial> trials;
}

public class DataHandler
{
    [SerializeField]
    private List<string> Scenes;
    private int current;

    private List<Trial> trials;
    private string userId;

    private static DataHandler instance = null;

    public static DataHandler GetInstance()
    {
        Debug.Log("getinstance");
        if (instance == null)
        {
            Debug.Log("created");
            instance = new DataHandler();
        }
        return instance;
    }

    private DataHandler()
    {
        //Scenes = new List<string>() { "A-Start", "Control1", "B-Next" };//{ "A-Start", "Test1", "B-Next" , "Test2", "B-Next", "Control1", "B-Next", "Control2", "B-Next", "C-End" }; //TODO: how can I change the code so that I don't have to add all new scenes here?
        Scenes = new List<string>() 
        { 
            "A-Start", 
            "Test1", 
            "B-Next" , 
            "Test2", 
            "B-Next", 
            "Control1", 
            "B-Next", 
            "Control2", 
            "B-Next",
            "Test1 1",
            "B-Next" ,
            "Test1 2",
            "B-Next" ,
            "Test1 3",
            "B-Next" ,
            "Test2 1",
            "B-Next",
            "Test2 2",
            "B-Next",
            "Test2 3",
            "B-Next",
            "Control1 1",
            "B-Next",
            "Control2 1",
            "B-Next",
            "Control2 2",
            "B-Next",
            "Control2 3",
            "B-Next",
        }; //TODO: how can I change the code so that I don't have to add all new scenes here?
        current = 0;

        trials = new List<Trial>();
    }

    public void end_first_scene(string UID)
    {
        userId = UID;
        load_next_scene();
    }

    public void end_experiment_scene(TrialData data)
    {
        Debug.Log("endscene called");
        if (Scenes[current] != "Next")
        {
            Trial t = new Trial();
            t.scene = Scenes[current];
            t.trialData = data;
            trials.Add(t);
        }
        load_next_scene();
    }

    private void load_next_scene()
    {
        current = (current + 1) % Scenes.Count;
        if (current == Scenes.Count - 1)
        {
            SaveData();
            Application.Quit();
        }
        SceneManager.LoadScene(Scenes[current]);
    }

    public void SaveData()
    {
        string path = string.Format("{0}/{1}-{2}.json", Application.persistentDataPath, userId, "date-time");
        JsonDummy jd = new JsonDummy();
        jd.trials = trials;
        string data = JsonUtility.ToJson(jd);
        Debug.Log("data is:"+data);

        //byte[] databytes = System.Text.Encoding.ASCII.GetBytes(data);
        //UnityEngine.Windows.File.WriteAllBytes(path, databytes);
        //System.IO.FileInfo file = new System.IO.FileInfo(path);
        //file.Directory.Create();
        System.IO.File.WriteAllText(path, data);
    }

}
