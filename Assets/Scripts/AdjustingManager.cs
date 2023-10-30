using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class InitSettings{
    public Vector3 initialGlassRotation;
    public Vector2 initialSurfaceTilt;
}

public class AdjustingManager : Manager
{

    [SerializeField]
    string experimentType;
    [SerializeField]
    List<InitSettings> experimentSettings;
    [SerializeField]
    int repeat;
    [SerializeField]
    Glass glass;
    [SerializeField]
    GameObject rotator;
    [SerializeField]
    TextMeshPro trialNum;
    [SerializeField]
    bool random_order;

    Vector3 glassInitPos;

    int numSettings;

    bool doneWork;

    bool[] settingEnded;

    int current;

    private void Awake()
    {
        doneWork = false;
        current = -1;
        int n = experimentSettings.Count;
        print("adjusting manager, init:" + experimentSettings.ToString());

        for (int i = 0; i < repeat; i++)
        {
            for (int j = 0; j < n; j++)
            {
                InitSettings s = new InitSettings();
                s.initialGlassRotation = experimentSettings[j].initialGlassRotation;
                s.initialSurfaceTilt = experimentSettings[j].initialSurfaceTilt;
                experimentSettings.Add(s);
            }
        }
        numSettings = experimentSettings.Count;
        print("adjusting manager, after: " + experimentSettings.ToString() + " count: " + numSettings.ToString());

        settingEnded = new bool[numSettings];
        for (int i = 0; i < numSettings; i++)
        {
            settingEnded[i] = false;
        }
        glassInitPos = glass.transform.position;

        if (random_order == true)
        {
            experimentSettings.Shuffle();
        }

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
            glass.transform.position = glassInitPos;
            glass.transform.eulerAngles = experimentSettings[current].initialGlassRotation;

            rotator.transform.eulerAngles = new Vector3(experimentSettings[current].initialSurfaceTilt.x, 0, experimentSettings[current].initialSurfaceTilt.y);
        }
    }

    public override void end_setting(string z)
    {
        Info ti = new Info();

        ti.initialGlassRotation = experimentSettings[current].initialGlassRotation;
        ti.initialSurfaceTilt = experimentSettings[current].initialSurfaceTilt;

        ti.type = experimentType;

        Result tr = new Result();
        tr.finallSurfaceTilt = new Vector2(glass.getConstantTiltX(), glass.getConstantTiltZ());

        TrialData ed = new TrialData();
        ed.type = experimentType;
        ed.trialInfo = ti;
        ed.trialResult = tr;
        ed.headPoses = GetComponent<InteractionHistoryManager>().getHeadPosData();
        ed.headRots = GetComponent<InteractionHistoryManager>().getHeadRotData();
        GetComponent<InteractionHistoryManager>().restart();

        if (settingEnded[current] == false)
        {
            Handler.GetInstance().add_trial_data(ed);
            settingEnded[current] = true;
        }

        load_next_setting();
    }

}
