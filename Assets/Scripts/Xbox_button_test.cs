using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Xbox_button_test : MonoBehaviour
{

    [SerializeField]
    TextMeshPro tm;
    string b = null;

    private void updateClicked()
    {
        if (Input.GetButtonUp("JS_Y"))
        {
            b = "JS_Y";
        }
        if (Input.GetButtonUp("JS_A"))
        {
            b = "JS_A";
        }
        if (Input.GetButtonUp("JS_B"))
        {
            b = "JS_B";
        }
        if (Input.GetButtonUp("JS_X"))
        {
            b = "JS_X";
        }
        if (Input.GetButtonUp("JS_LB"))
        {
            b = "JS_LB";
        }
        if (Input.GetButtonUp("JS_RB"))
        {
            b = "JS_RB";
        }        
 //       if (Input.GetButtonUp("Fire1"))
 //       {
 //           b = "Fire1";
 //       }
    }

    // Update is called once per frame
    void Update()
    {
        updateClicked();
        if (tm != null)
            tm.text = b;
    }
}
