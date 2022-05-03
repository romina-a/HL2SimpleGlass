using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;


public class RotationInputManager : ChoiceInputManager
{
    [SerializeField]
    GameObject trackedObject = null;
    public void end_scene()
    {
        TrialData cd = new TrialData();
        Coord c = new Coord();
        c.x = trackedObject.transform.rotation.eulerAngles.x;
        c.y = trackedObject.transform.rotation.eulerAngles.y;
        c.z = trackedObject.transform.rotation.eulerAngles.z;

        Vector3 n = Vector3.up;
        n = Quaternion.AngleAxis(c.x, Vector3.right) * n;
        n = Quaternion.AngleAxis(c.z, Vector3.forward) * n;
        float deg = Mathf.Rad2Deg*Vector3.Magnitude(Vector3.Cross(Vector3.up, n));


        cd.orientation = c;
        cd.headPoses = headPosData;
        cd.tiltDegree = deg;
        DataHandler.GetInstance().end_experiment_scene(cd);
        Debug.Log("controll manager orientation is:");
        Debug.Log(c.x);
        Debug.Log(c.y);
        Debug.Log(c.z);
    }

}
