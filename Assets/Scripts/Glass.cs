using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExampleScript : MonoBehaviour
{
    AudioSource audioData;

    void Start()
    {
        //audioData = GetComponent<AudioSource>();
        //audioData.Play(0);
        Debug.Log("started");
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 70, 150, 30), "Pause"))
        {
            audioData.Pause();
            Debug.Log("Pause: " + audioData.time);
        }

        if (GUI.Button(new Rect(10, 170, 150, 30), "Continue"))
        {
            audioData.UnPause();
        }
    }
}
public class Glass : MonoBehaviour
{
    Wobble myWobble;
    [SerializeField]
    bool useCover = false;
    [SerializeField]
    GameObject cover;
    [SerializeField]
    float threshold = 0.01f;

    //[SerializeField]
    //bool limitRotation = false;
    //[SerializeField]
    //float rotationLimit = 90;
    //[SerializeField]
    //AudioSource audio;

    void Awake()
    {
        myWobble = GetComponentInChildren<Wobble>();
        //GetComponent<AudioSource>().loop = true;
        //GetComponent<AudioSource>().Pause();
        cover.SetActive(false);
    }

    public void setUseCover(bool value)
    {
        useCover = value;
    }

    public void showCover(bool value)
    {
        cover.SetActive(value && useCover);
    }


    private void Update()
    {
        /*
        if (useCover && (Mathf.Abs(myWobble.getXWobble())> threshold || Mathf.Abs(myWobble.getZWobble())> threshold))
        {
            cover.SetActive(true);
        }
        else
        {
            cover.SetActive(false);
        }
        */
        /*
        if (limitRotation)
        {
            float x = transform.eulerAngles.x;
            float z = transform.eulerAngles.z;
            if (x < -180) x += 360;
            if (x > 180) x -= 360;
            if (z < -180) z += 360;
            if (z > 180) z -= 360;
            
            x = Mathf.Clamp(x, -rotationLimit, rotationLimit);
            if (x == -rotationLimit) x += 3;
            if (x == rotationLimit) x -= 3;
            z = Mathf.Clamp(z, -rotationLimit, rotationLimit);
            if (z == -rotationLimit) z += 3;
            if (z == rotationLimit) z -= 3;
            

            if (Mathf.Abs(x)> rotationLimit || Mathf.Abs(z)> rotationLimit)
            {
                audio.Play();
            }
            else
            {
                audio.Pause();
            }

            //transform.eulerAngles = new Vector3(x, transform.eulerAngles.y, z);
        }
        */
    }

    public void setConstantTiltX(float constantTiltX)
    {
        myWobble.constantTiltX = constantTiltX;
    }   
    public void setConstantTiltZ(float constantTiltZ)
    {
        myWobble.constantTiltZ = constantTiltZ;
    }    

    public float getConstantTiltZ()
    {
        return myWobble.constantTiltZ;
    }
    public float getConstantTiltX()
    {
        return myWobble.constantTiltX;
    }

    public void setConditionalTilt(float conditionalTilt)
    {
        myWobble.conditionalTilt = conditionalTilt;
    }

    public void restart()
    {
        myWobble.restart();
    }
}
