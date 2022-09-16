using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputVisualizer : MonoBehaviour
{

    bool active;
    string choice = null;

    [SerializeField]
    TextMeshPro left, right;

    [SerializeField]
    TextMeshPro info;

    [SerializeField]
    string leftvalue, rightvalue;

    [SerializeField]
    Manager myManager;

    [SerializeField]
    int numRequiredInteractions;

    [SerializeField]
    bool control;
    [SerializeField]
    bool next;

    bool[] interactions;
    int numActivatedInteractions;

    [SerializeField]
    string deactivated;
    [SerializeField]
    string choose_first;

    // Start is called before the first frame update
    void Awake()
    {
        if (numRequiredInteractions > 0)
        {
            Deactivate();
        }
        else
        {
            active = true;
        }
        if (control)
        {
            right.text = "";
            left.text = "";
        }
    }

    public void Activate(int i)
    {
        if (interactions[i] == false)
        {
            interactions[i] = true;
            numActivatedInteractions++;

            if (numActivatedInteractions == numRequiredInteractions)
            {
                info.text = "";
                active = true;
            }
        }
    }

    public void Deactivate()
    {
        active = false;
        info.text = deactivated;

        interactions = new bool[numRequiredInteractions];
        for (int i = 0; i < numRequiredInteractions; i++)
        {
            interactions[i] = false;
        }
        numActivatedInteractions = 0;

        if (!control && !next)
        {
            left.color = new Color(0.6352941f, 0.6352941f, 0.6352941f);
            right.color = new Color(0.6352941f, 0.6352941f, 0.6352941f);
        }
    }

    public void select(string choiceinput) // right or left
    {
        if (control || next)
        {
            return;
        }
        if (active == false)
        {
            info.text = deactivated;
            return;
        }
        choice = choiceinput;
        if (choice == "right")
        {
            right.color = new Color(1f, 0.8f, 0.0313f);
            left.color = new Color(0.6352941f, 0.6352941f, 0.6352941f);
        }
        if (choice == "left")
        {
            left.color = new Color(1f, 0.8f, 0.0313f);
            right.color = new Color(0.6352941f, 0.6352941f, 0.6352941f);
        }
    }

    public void submit()
    {
        if (active == false)
        {
            info.text = deactivated;
            return;
        }
        else if (choice == null & !control & !next)
        {
            info.text = choose_first;
        }
        else
        {
            if (numRequiredInteractions > 0)
            {
                Deactivate();
            }
            if (control || next)
            {
                myManager.end_setting(choice);
                choice = null;
            }
            else
            {
                if (choice == "right")
                {
                    myManager.end_setting(rightvalue);
                    choice = null;
                }
                if (choice == "left")
                {
                    myManager.end_setting(leftvalue);
                    choice = null;
                }
            }

        }
    }
}
