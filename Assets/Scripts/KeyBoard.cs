using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class KeyBoard : MonoBehaviour
{

    private MixedRealityKeyboard wmrKeyboard;
    [SerializeField, Tooltip("Whether disable user's interaction with other UI elements while typing. Use this option to decrease the chance of keyboard getting accidentally closed.")]
    private bool disableUIInteractionWhenTyping = false;
    [SerializeField]
    private TextMeshPro typedText = null;

    private void Start()
    {
        Debug.Log("KeyBoard: Start");
        // Windows mixed reality keyboard initialization goes here
        wmrKeyboard = gameObject.AddComponent<MixedRealityKeyboard>();
        wmrKeyboard.DisableUIInteractionWhenTyping = disableUIInteractionWhenTyping;
    }

    private void Update()
    {
        if (wmrKeyboard.Visible)
        {
            if (typedText != null)
            {
                typedText.text = wmrKeyboard.Text;
            }
        }
        else
        {
            var keyboardText = wmrKeyboard.Text;

            if (string.IsNullOrEmpty(keyboardText))
            {
                if (typedText != null)
                {
                    typedText.text = "Participant's ID";
                }
            }
            else
            {
                if (typedText != null)
                {
                    typedText.text = keyboardText;
                }
            }
        }

    }

    public void StartKeyBoard()
    {
        Debug.Log("KeyBoard: StartKeyBoard");
        wmrKeyboard.ShowKeyboard(wmrKeyboard.Text, false);
    }
}
