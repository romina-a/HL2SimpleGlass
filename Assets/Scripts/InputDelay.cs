using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDelay : MonoBehaviour
{

    float time;
    bool active;
    GameObject[] UIelements;
    [SerializeField]
    float stillTime = 0.5f;

    // Start is called before the first frame update
    void Awake()
    {
        UIelements = GameObject.FindGameObjectsWithTag("UIElement");
        startDelay();
    }

    // Update is called once per frame
    void Update()
    {
        float elapsedTime = Time.time - time;
        if (elapsedTime > stillTime && !active)
        {
            Debug.Log("active");
            active = true;
            foreach (GameObject ue in UIelements)
            {
                ue.SetActive(true);
            }
        }
    }
    public void startDelay()
    {
        Debug.Log("startDelay");
        active = false;
        time = Time.time;
        foreach (GameObject ue in UIelements)
        {
            ue.SetActive(false);
        }
    }
}
