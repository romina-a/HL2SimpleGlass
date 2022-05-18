using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    bool active;
    string choice = null;

    [SerializeField]
    GameObject left, right, leftselected, rightselected;

    [SerializeField]
    GameObject selectSomethingText;

    [SerializeField]
    GameObject deactiveText;    

    [SerializeField]
    string leftvalue, rightvalue;

    [SerializeField]
    Manager myManager; 

    // Start is called before the first frame update
    void Awake()
    {
        Deactivate();
    }

    public void Activate()
    {
        active = true;
        deactiveText.SetActive(false);
    }

    public void Deactivate()
    {
        active = false;

        left.gameObject.SetActive(true);
        right.gameObject.SetActive(true);
        leftselected.gameObject.SetActive(false);
        rightselected.gameObject.SetActive(false);

        deactiveText.SetActive(true);
        selectSomethingText.SetActive(false);
    }

    public void select(string choiceinput) // right or left
    {
        Debug.Log("select: " + choiceinput);
        if (active == false)
        {
            deactiveText.SetActive(true);
            selectSomethingText.SetActive(false);
            return;
        }
        selectSomethingText.SetActive(false);
        choice = choiceinput;
        if (choice == "right")
        {
            left.gameObject.SetActive(true);
            right.gameObject.SetActive(false);
            leftselected.gameObject.SetActive(false);
            rightselected.gameObject.SetActive(true);
        }
        if (choice == "left")
        {
            left.gameObject.SetActive(false);
            right.gameObject.SetActive(true);
            leftselected.gameObject.SetActive(true);
            rightselected.gameObject.SetActive(false);
        }
    }

    public void submit()
    {
        if (active == false)
        {
            deactiveText.SetActive(true);
            selectSomethingText.SetActive(false);
            return;
        }
        else if (choice == null)
        {
            selectSomethingText.SetActive(true);
        }
        else
        {
            Debug.Log("submit: " + choice);
            if (choice == "right")
            {
                myManager.end_trial(rightvalue);
            }
            if (choice == "left")
            {
                myManager.end_trial(leftvalue);
            }
        }
    }
}
