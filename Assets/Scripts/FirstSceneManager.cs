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
        Handler.GetInstance().set_user_id_and_load_next(typedText.text);
    }
}
