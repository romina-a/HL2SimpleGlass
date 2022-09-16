using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class TestSettings{
    public float tilt1;
    public float tilt2;
    public float zRotation1;
    public float zRotation2;
    public bool cover;
}

public class ComparisonManager : Manager
{

    [SerializeField]
    string experimentType;
    [SerializeField]
    List<TestSettings> glassSettings;
    [SerializeField]
    int repeat;
    [SerializeField]
    Glass glass1;
    [SerializeField]
    Glass glass2;
    [SerializeField]
    string glass1color;
    [SerializeField]
    string glass2color;
    [SerializeField]
    TextMeshPro trialNum;
    [SerializeField]
    bool fixedRotation, covered, limitedRotation;


    Vector3 glass1InitPos;
    Vector3 glass2InitPos;
    Vector3 glass1InitRot;
    Vector3 glass2InitRot;

    int numSettings;

    bool doneWork;

    bool[] settingEnded;

    float glass1ConditionalTilt;
    float glass2ConditionalTilt;
    Vector3 glass1StartRot;
    Vector3 glass2StartRot;

    int current;

    private void Awake()
    {

        Debug.Log("comparison manager awakened");
        doneWork = false;
        current = -1;
        int n = glassSettings.Count;
        print("compartison manager, init:" + glassSettings.ToString());
        for (int i = 0; i < repeat; i++)
        {
            for (int j = 0; j < n; j++)
            {
                TestSettings s = new TestSettings();
                s.tilt1 = glassSettings[j].tilt1;
                s.tilt2 = glassSettings[j].tilt2;
                s.cover = glassSettings[j].cover;
                s.zRotation1 = glassSettings[j].zRotation1;
                s.zRotation2 = glassSettings[j].zRotation2;
                glassSettings.Add(s);
            }
        }
        numSettings = glassSettings.Count;
        print("compartiosn manager, after: " + glassSettings.ToString() + " count: " + numSettings.ToString());
        settingEnded = new bool[numSettings];
        for (int i = 0; i < numSettings; i++)
        {
            settingEnded[i] = false;
        }
        glass1InitPos = glass1.transform.position;
        glass2InitPos = glass2.transform.position;
        glass1InitRot = glass1.transform.eulerAngles;
        glass2InitRot = glass2.transform.eulerAngles;

        glassSettings.Shuffle();

        load_next_setting();
    }

    protected override void load_next_setting()
    {
        current = current + 1;
        if (current == numSettings)
        {
            current = -1;
            if (doneWork == false)
            {
                Handler.GetInstance().load_next_experiment();
                doneWork = true;
            }
        }
        else
        {
            trialNum.text = "trial: " + (current + 1).ToString() + "/" + numSettings.ToString();
            Debug.Log("laoding next in Comparison manager");
            int coin = Random.Range(0, 2);
            if (coin == 0)
            {
                glass1ConditionalTilt = glassSettings[current].tilt1;
                glass2ConditionalTilt = glassSettings[current].tilt2;
                glass1StartRot = new Vector3(glass1InitRot.x, glass1InitRot.y, glassSettings[current].zRotation1);
                glass2StartRot = new Vector3(glass1InitRot.x, glass1InitRot.y, glassSettings[current].zRotation2);
            }
            else 
            {
                glass1ConditionalTilt = glassSettings[current].tilt2;
                glass2ConditionalTilt = glassSettings[current].tilt1;
                glass1StartRot = new Vector3(glass2InitRot.x, glass2InitRot.y, glassSettings[current].zRotation2);
                glass2StartRot = new Vector3(glass2InitRot.x, glass2InitRot.y, glassSettings[current].zRotation1);
            }

            glass1.setUseCover(glassSettings[current].cover);
            glass2.setUseCover(glassSettings[current].cover);
            covered = glassSettings[current].cover;

            glass1.setConditionalTilt(glass1ConditionalTilt);
            glass1.transform.position = glass1InitPos;
            glass1.transform.eulerAngles = glass1StartRot;

            glass2.setConditionalTilt(glass2ConditionalTilt);
            glass2.transform.position = glass2InitPos;
            glass2.transform.eulerAngles = glass2StartRot;

            glass1.restart();
            glass2.restart();
        }
    }

    public override void end_setting(string choice)
    {
        Info ti = new Info();
        GlassData glass1info = new GlassData();
        GlassData glass2info = new GlassData();

        glass1info.color = glass1color;
        glass1info.conditionalTilt = glass1ConditionalTilt;
        glass1info.fixedRotation = fixedRotation;
        glass1info.initialRotation = glass1StartRot;

        glass2info.color = glass2color;
        glass2info.conditionalTilt = glass2ConditionalTilt;
        glass2info.fixedRotation = fixedRotation;
        glass2info.initialRotation = glass2StartRot;

        ti.type = experimentType;
        ti.glass1 = glass1info;
        ti.glass2 = glass2info;
        ti.fixedRotation = fixedRotation;
        ti.covered = covered;
        ti.limitedRotation = limitedRotation;

        Result tr = new Result();
        tr.answer = choice;
        tr.finalGlass1Pos = glass1.transform.position;
        tr.finalGlass1Rot = glass1.transform.eulerAngles;
        tr.finalGlass2Pos = glass2.transform.position;
        tr.finalGlass2Rot = glass2.transform.eulerAngles;


        TrialData ed = new TrialData();
        ed.type = experimentType;
        ed.trialInfo = ti;
        ed.trialResult = tr;
        ed.headPoses = GetComponent<InteractionHistoryManager>().getHeadPosData();
        ed.headRots = GetComponent<InteractionHistoryManager>().getHeadRotData();
        ed.glass1Poses = GetComponent<InteractionHistoryManager>().getGlass1PosData();
        ed.glass1Rots = GetComponent<InteractionHistoryManager>().getGlass1RotData();
        ed.glass2Poses = GetComponent<InteractionHistoryManager>().getGlass2PosData();
        ed.glass2Rots = GetComponent<InteractionHistoryManager>().getGlass2RotData();
        GetComponent<InteractionHistoryManager>().restart(); 

        if (settingEnded[current] == false)
        {
            Handler.GetInstance().add_trial_data(ed);
            settingEnded[current] = true;
        }

        load_next_setting();
    }

}
