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
    int numSettings;
    [SerializeField]
    Glass glass;
    [SerializeField]
    string glassColor;
    [SerializeField]
    GameObject trialno;

    Vector3 glassInitPos;
    Vector3 glassInitRot;
    float glassConditionalTilt;

    int current;

    private void Start()
    {
        current = -1;
        glassInitPos = glass.transform.position;
        glassInitRot = glass.transform.eulerAngles;

        load_next_setting();
    }

    public override void load_next_setting()
    {
        Debug.Log("load next setting");
        //GetComponent<InputDelay>().startDelay();

        current = current + 1;
        if (current == numSettings)
        {
            current = -1;
            Handler.GetInstance().load_next_experiment();
        }
        else
        {
            TextMeshPro t = trialno.GetComponent<TextMeshPro>();
            t.text = "trial: " + (current+1).ToString() + "/" + numSettings.ToString();
            Debug.Log("laoding next in Judging manager");
            glassConditionalTilt = glassSettings[current];
            glass.setConditionalTilt(glassConditionalTilt);
            glass.transform.position = glassInitPos;
            glass.transform.eulerAngles = glassInitRot;
        }
    }

    public override void end_trial(string choice)
    {

        Debug.Log("end_trial");
        Debug.Log("Judging choice is: " + choice);
        Info ti = new Info();
        GlassData glassInfo = new GlassData();

        glassInfo.color = glassColor;
        glassInfo.conditionalTilt = glassConditionalTilt;

        ti.type = experimentType;
        ti.glass1 = glassInfo;

        Result tr = new Result();
        tr.answer = choice;

        TrialData ed = new TrialData();
        ed.trialInfo = ti;
        ed.trialResult = tr;
        ed.headPoses = GetComponent<HeadPosManager>().getPosData();
        GetComponent<HeadPosManager>().restart();
        Handler.GetInstance().add_trial_data(ed);

        load_next_setting();
    }

}
