using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;


public class NextManager : MonoBehaviour
{
    public void end_trial()
    {
        Handler.GetInstance().load_next_experiment();
    }

}
