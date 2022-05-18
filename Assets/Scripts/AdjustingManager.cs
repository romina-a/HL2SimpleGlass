using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

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
    int numSettings;
    [SerializeField]
    Glass glass;

    Vector3 glassInitPos;

    int current;

    private void Start()
    {
        current = -1;
        glassInitPos = glass.transform.position;
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
            glass.transform.position = glassInitPos;
            glass.transform.eulerAngles = experimentSettings[current].initialGlassRotation;
            glass.setConstantTiltX(experimentSettings[current].initialSurfaceTilt.x);
            glass.setConstantTiltZ(experimentSettings[current].initialSurfaceTilt.y);
        }
    }

    public override void end_trial(string z)
    {
        Info ti = new Info();

        ti.initialGlassRotation = experimentSettings[current].initialGlassRotation;
        ti.initialSurfaceTilt = experimentSettings[current].initialSurfaceTilt;

        ti.type = experimentType;

        Result tr = new Result();
        tr.finallSurfaceTilt = new Vector2(glass.getConstantTiltX(), glass.getConstantTiltZ());

        TrialData ed = new TrialData();
        ed.trialInfo = ti;
        ed.trialResult = tr;
        ed.headPoses = GetComponent<HeadPosManager>().getPosData();
        ed.headRots = GetComponent<HeadPosManager>().getRotData();
        GetComponent<HeadPosManager>().restart();
        Handler.GetInstance().add_trial_data(ed);

        load_next_setting();
    }

}
