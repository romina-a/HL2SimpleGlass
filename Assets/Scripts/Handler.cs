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

    public bool fixedRotation;
    public bool covered;
    public bool limitedRotation;

    public Vector3 initialGlassRotation;
    public Vector2 initialSurfaceTilt;
}


[System.Serializable]
public class Result
{
    public string answer;
    public Vector3 finalGlass1Pos;
    public Vector3 finalGlass1Rot;

    public Vector3 finalGlass2Pos;
    public Vector3 finalGlass2Rot;

    public Vector2 finallSurfaceTilt;
}

[System.Serializable]
public class GlassData
{
    public float conditionalTilt;
    public string color;
    public bool fixedRotation;
    public bool fixedTranslation;
    public Vector3 initialRotation;
}

[System.Serializable]
public class TrialData
{
    public string type;
    public List<Vector3> headPoses = new List<Vector3>();
    public List<Vector3> headRots = new List<Vector3>();
    public List<Vector3> glass1Poses = new List<Vector3>();
    public List<Vector3> glass1Rots = new List<Vector3>();
    public List<Vector3> glass2Poses = new List<Vector3>();
    public List<Vector3> glass2Rots = new List<Vector3>();
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
        if (instance == null)
        {
            instance = new Handler();
        }
        return instance;
    }

    private Handler()
    {
        current = 0;
        trialsData = new List<TrialData>();
        //experiments = new List<string>() {"A-Start", "A-StartInfo", "Test2A", "Test2Mixed", "Z-ControlInfo", "Z-Control", "Z-End" }; //Original
        //experiments = new List<string>() {"A-Start", "Test2A", "Test2Mixed", "Z-End"}; //for SUS
        experiments = new List<string>() {"Test2Mixed", "Z-Control", "Z-End" }; //for catalyst
    }

    public void set_user_id_and_load_next(string UID)
    {
        userId = UID;
        load_next_experiment();
    }

    public void add_trial_data(TrialData trialData)
    {
        //Debug.Log("handler added this data:");
        //Debug.Log("info: "+JsonUtility.ToJson(trialData.trialInfo));
        //Debug.Log("type: " + trialData.type);
        //Debug.Log("result: " + JsonUtility.ToJson(trialData.trialResult));
        trialsData.Add(trialData);
    }
    public void load_next_experiment()
    {
        Debug.Log("Handler, load_next_experiment, current-1 is: "+ current.ToString());
        current = (current + 1) % experiments.Count;
        //if (current == experiments.Count - 1)
        //{
        //    Debug.Log("saving");
        //    SaveData();

        //    Debug.Log("quitting");
        //    Application.Quit();
        //} commented for Catalyst
        Debug.Log("handler is loading this scene: "+ experiments[current].ToString());
        SceneManager.LoadSceneAsync(experiments[current], LoadSceneMode.Single);

    }

    public void SaveData()
    {
        Debug.Log("Handler, SaveData");
        DateTime now = DateTime.Now;
        string datetime = now.ToString("yyyy-MM-dd_HH-mm");
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
