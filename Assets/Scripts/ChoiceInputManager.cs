using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;


public class ChoiceInputManager:MonoBehaviour
{
    float time;
    GameObject[] UIelements;
    protected List<Coord> headPosData;
    [SerializeField]
    float stillTime = 0.5f;

    private void Start()
    {
        time = Time.time;
        UIelements = GameObject.FindGameObjectsWithTag("UIElement");
        headPosData = new List<Coord>();
        foreach (GameObject ue in UIelements)
        {
            ue.SetActive(false);
        }
    }
    private void Update()
    {
        float elapsedTime = Time.time - time;
        if (elapsedTime > stillTime)
        {
            foreach (GameObject ue in UIelements)
            {
                ue.SetActive(true);
            }
        }

        Coord pos = new Coord();
        pos.x = 0;
        pos.y = 0;
        pos.z = 0;
        //headPosData.Add(pos);
    }
    public void end_scene(string choice)
    {
        TrialData ed = new TrialData();
        ed.choice = choice;
        ed.headPoses = headPosData;
        DataHandler.GetInstance().end_experiment_scene(ed);
    }

}
