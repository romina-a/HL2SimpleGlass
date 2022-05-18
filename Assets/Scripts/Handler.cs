using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;

[System.Serializable]
public class Info
{
    public string type;

    public GlassData glass1;
    public GlassData glass2;

    public Vector3 initialGlassRotation;
    public Vector2 initialSurfaceTilt;
}

public class Result
{
    public string answer;
    public Vector2 finallSurfaceTilt;
}

[System.Serializable]
public class GlassData
{
    public float conditionalTilt;
    public string color;
}

[System.Serializable]
public class TrialData
{
    public string type;
    public List<Vector3> headPoses = new List<Vector3>();
    public List<Vector3> headRots = new List<Vector3>();
    public Info trialInfo;
    public Result trialResult;
}

[System.Serializable]
public class JsonDummy
{
    public List<TrialData> trials;
}

public class Handler
{
    private List<string> experiments;
    private int current;

    private List<TrialData> trialsData;
    private string userId;

    private static Handler instance = null;

    public static Handler GetInstance()
    {
        Debug.Log("getinstance");
        if (instance == null)
        {
            Debug.Log("created");
            instance = new Handler();
        }
        return instance;
    }

    private Handler()
    {
        current = 0;
        trialsData = new List<TrialData>();
        experiments = new List<string>() {"A-Start", "B-Next", "B-Next 1", "B-Next 2", "B-Next 3", "Test1", "B-Next", "Test2", "B-Next", "Control1", "B-Next" };
    }

    public void set_user_id_and_load_next(string UID)
    {
        userId = UID;
        load_next_experiment();
    }

    public void add_trial_data(TrialData trialData)
    {
        trialsData.Add(trialData);
    }
    public void load_next_experiment()
    {
        Debug.Log("here");
        current = (current + 1) % experiments.Count;
        if (current == experiments.Count - 1)
        {
            Debug.Log("saving");
            SaveData();
            //Application.Quit();
        }
        Debug.Log("load scene");
        Debug.Log(experiments[current]);
        SceneManager.LoadScene(experiments[current]);
    }

    public void SaveData()
    {
        DateTime now = DateTime.Now;
        string datetime = now.ToString("yyyyMMddHHmmss");
        datetime = "datetime";
        string path = string.Format("{0}/{1}-{2}.json", Application.persistentDataPath, userId, datetime);
        JsonDummy jd = new JsonDummy();
        jd.trials = trialsData;
        string data = JsonUtility.ToJson(jd);
        Debug.Log("data is:" + data);
        Debug.Log("path is:" + path);

        //byte[] databytes = System.Text.Encoding.ASCII.GetBytes(data);
        //UnityEngine.Windows.File.WriteAllBytes(path, databytes);
        //System.IO.FileInfo file = new System.IO.FileInfo(path);
        //file.Directory.Create();
        System.IO.File.WriteAllText(path, data);
    }

}
