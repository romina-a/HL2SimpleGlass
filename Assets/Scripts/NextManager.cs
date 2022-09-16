using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;


public class NextManager : Manager
{
    bool doneWork;
    public void Awake()
    {
        doneWork = false;
    }
    public override void end_setting(string s)
    {
        load_next_setting();
    }

    protected override void load_next_setting()
    {
        if (doneWork == false) { 
            Handler.GetInstance().load_next_experiment();
            doneWork = true;
        }
    }

}
