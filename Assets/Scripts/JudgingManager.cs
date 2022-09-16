using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class JudgingManager : Manager
{

    [SerializeField]
    string experimentType;
    [SerializeField]
    List<float> glassSettings;
    [SerializeField]
    int repeat;
    [SerializeField]
    Glass glass;
    [SerializeField]
    string glassColor;
    [SerializeField]
    TextMeshPro trialNum;

    Vector3 glassInitPos;
    Vector3 glassInitRot;
    float glassConditionalTilt;

    int numSettings;

    bool doneWork;

    bool[] settingEnded;

    int current;

    private void Awake()
    {
        doneWork = false;
        current = -1;
        int n = glassSettings.Count;
        print("judgin manager, init:" + glassSettings);
        for (int i = 0; i < repeat; i++)
        {
            for (int j = 0; j < n; j++)
            {
                glassSettings.Add(glassSettings[j]);
            }
        }
        numSettings = glassSettings.Count;
        print("judgin manager, after: " + glassSettings+" count: "+ numSettings.ToString());
        settingEnded = new bool[numSettings];
        for (int i =0; i < numSettings; i++)
        {
            settingEnded[i] = false;
        }
        glassInitPos = glass.transform.position;
        glassInitRot = glass.transform.eulerAngles;

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
            trialNum.text = "trial: " + (current+1).ToString() + "/" + numSettings.ToString();
            glassConditionalTilt = glassSettings[current];
            glass.setConditionalTilt(glassConditionalTilt);
            glass.transform.position = glassInitPos;
            glass.transform.eulerAngles = glassInitRot;
        }
    }

    public override void end_setting(string choice)
    {
        Debug.Log("end_setting");
        Debug.Log("Judging choice is: " + choice);
        Info ti = new Info();
        GlassData glassInfo = new GlassData();

        glassInfo.color = glassColor;
        glassInfo.conditionalTilt = glassConditionalTilt;

        ti.type = experimentType;
        ti.glass1 = glassInfo;

        Result tr = new Result();
        tr.answer = choice;
        tr.finalGlass1Pos = glass.transform.position;
        tr.finalGlass1Rot = glass.transform.eulerAngles;

        TrialData ed = new TrialData();
        ed.type = experimentType;
        ed.trialInfo = ti;
        ed.trialResult = tr;
        ed.headPoses = GetComponent<InteractionHistoryManager>().getHeadPosData();
        ed.headRots = GetComponent<InteractionHistoryManager>().getHeadRotData();
        ed.glass1Poses = GetComponent<InteractionHistoryManager>().getGlass1PosData();
        ed.glass1Rots = GetComponent<InteractionHistoryManager>().getGlass1RotData();

        GetComponent<InteractionHistoryManager>().restart();

        if (settingEnded[current] == false)
        {
            Debug.Log("judgin manager added data, current is: "+current.ToString());
            Handler.GetInstance().add_trial_data(ed);
            settingEnded[current] = true;
        }

        load_next_setting();
    }

}
