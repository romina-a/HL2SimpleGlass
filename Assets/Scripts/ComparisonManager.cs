using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class conditionalTilts{
   public float tilt1;
   public float tilt2;
}

public class ComparisonManager : Manager
{

    [SerializeField]
    string experimentType;
    [SerializeField]
    List<conditionalTilts> glassSettings;
    [SerializeField]
    int numSettings;
    [SerializeField]
    Glass glass1;
    [SerializeField]
    Glass glass2;
    [SerializeField]
    string glass1color;
    [SerializeField]
    string glass2color;

    Vector3 glass1InitPos;
    Vector3 glass2InitPos;
    Vector3 glass1InitRot;
    Vector3 glass2InitRot;

    float glass1ConditionalTilt;
    float glass2ConditionalTilt;

    int current;

    private void Start()
    {
        current = -1;
        glass1InitPos = glass1.transform.position;
        glass2InitPos = glass2.transform.position;
        glass1InitRot = glass1.transform.eulerAngles;
        glass2InitRot = glass2.transform.eulerAngles;
        load_next_setting();
    }

    public override void load_next_setting()
    {

        current = current + 1;
        if (current == numSettings)
        {
            current = -1;
            Handler.GetInstance().load_next_experiment();
        }
        else
        {
            int coin = Random.Range(0, 2);
            if (coin == 0)
            {
                glass1ConditionalTilt = glassSettings[current].tilt1;
                glass2ConditionalTilt = glassSettings[current].tilt2;
            }
            else 
            {
                glass1ConditionalTilt = glassSettings[current].tilt2;
                glass2ConditionalTilt = glassSettings[current].tilt1;
            }

            glass1.setConditionalTilt(glass1ConditionalTilt);
            glass1.transform.position = glass1InitPos;
            glass1.transform.eulerAngles = glass1InitRot;

            glass2.setConditionalTilt(glass2ConditionalTilt);
            glass2.transform.position = glass2InitPos;
            glass2.transform.eulerAngles = glass2InitRot;
        }
    }

    public override void end_trial(string choice)
    {
        Info ti = new Info();
        GlassData glass1info = new GlassData();
        GlassData glass2info = new GlassData();

        glass1info.color = glass1color;
        glass1info.conditionalTilt = glass1ConditionalTilt;

        glass2info.color = glass2color;
        glass2info.conditionalTilt = glass2ConditionalTilt;

        ti.type = experimentType;
        ti.glass1 = glass1info;
        ti.glass2 = glass2info;

        Result tr = new Result();
        tr.answer = choice;

        TrialData ed = new TrialData();
        ed.trialInfo = ti;
        ed.trialResult = tr;
        ed.headPoses = GetComponent<HeadPosManager>().getPosData();
        GetComponent<HeadPosManager>().restart(); ;
        Handler.GetInstance().add_trial_data(ed);

        load_next_setting();
    }

}
