using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BottonListener : MonoBehaviour
{
    [SerializeField]
    InputVisualizer inputViz;
    // Start is called before the first frame update

    [SerializeField]
    TextMeshPro tm;

    private string getClicked()
    {
        string b = null;
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
        return b;
    }

    private void Start()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        tm.text += "devices:";
        foreach (var device in inputDevices)
        {
            tm.text += string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()) + "\n";
            Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
        }
    }

    // Update is called once per frame
    void Update()
    {
        string button = getClicked();
        tm.text = button;
        if (/*button == "JS_RB" || */button == "JS_B")
        {
            inputViz.select("right");
        }
        if (/*button == "JS_LB" || */button == "JS_X")
        {
            inputViz.select("left");
        }
        if (button == "JS_A")
        {
            inputViz.submit();
        }

    }
}
