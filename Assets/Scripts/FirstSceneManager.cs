using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FirstSceneManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro typedText = null;

    public void end_scene()
    {
        DataHandler.GetInstance().end_first_scene(typedText.text);
    }
}
