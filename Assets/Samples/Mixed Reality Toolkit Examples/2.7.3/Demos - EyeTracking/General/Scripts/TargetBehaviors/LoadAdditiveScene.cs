// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Microsoft.MixedReality.Toolkit.Examples.Demos.EyeTracking
{
    /// <summary>
    /// When the button is selected, it triggers starting the specified type.
    /// </summary>
    [RequireComponent(typeof(EyeTrackingTarget))]
    [AddComponentMenu("Scripts/MRTK/Examples/LoadAdditiveScene")]
    public class LoadAdditiveScene : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Name of the type to be loaded when the button is selected.")]
        private string SceneToBeLoaded = "";

        [SerializeField]
        [Tooltip("Optional AudioClip which is played when the button is selected.")]
        private AudioClip audio_OnSelect = null;

        [SerializeField]
        [Tooltip("Timeout in seconds before new type is loaded.")]
        private float waitTimeInSecBeforeLoading = 0.25f;


        public void LoadScene()
        {
            LoadScene(SceneToBeLoaded);
        }

        public void LoadScene(string sceneName)
        {
            if (!string.IsNullOrWhiteSpace(sceneName))
            {
                StartCoroutine(LoadNewScene(sceneName));
            }
            else
            {
                Debug.Log($"Unsupported type name: {sceneName}");
            }
        }

        public static string lastSceneLoaded = "";
        private IEnumerator LoadNewScene(string sceneName)
        {
            AudioFeedbackPlayer.Instance.PlaySound(audio_OnSelect);

            // Let's find out the name of the currently loaded additive type to unload
            if (SceneManager.sceneCount > 1)
            {
                lastSceneLoaded = SceneManager.GetSceneAt(1).name;

                Debug.Log($"Last type name: {lastSceneLoaded}");

                // Let's wait in case we don't want to switch scenes too abruptly 
                yield return new WaitForSeconds(waitTimeInSecBeforeLoading);

                SceneManager.UnloadSceneAsync(lastSceneLoaded);
            }

            Debug.Log($"New type name: {SceneToBeLoaded}");
            lastSceneLoaded = SceneToBeLoaded;
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
}