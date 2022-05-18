using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
    Wobble myWobble;

    void Awake()
    {
        myWobble = GetComponentInChildren<Wobble>();
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
        Debug.Log(myWobble);
        myWobble.conditionalTilt = conditionalTilt;
    }
}
