using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour
{

    private void drawLine (Vector3 start, Vector3 end, Color color)
    {
        Vector3 dir = end - start;
        dir = new Vector3(-dir.y, dir.x, 0);
        dir = dir.normalized/5000;
        Debug.DrawLine(start, end, color, 0, true );
        Debug.DrawLine(start+dir, end+dir, color, 0, true);
        Debug.DrawLine(start+2*dir, end+2*dir, color, 0, false);
        Debug.DrawLine(start-dir, end-dir, color, 0, true);
        Debug.DrawLine(start-2*dir, end-2*dir, color, 0, false);
    }

    Renderer rend;

    // [SerializeField]
    //Vector3 gravity = new Vector3(0, -10, 0);
    //[SerializeField]
    //float waterHeight = 0; //todo: apply changes to container position based on height
    //[SerializeField]
    //float density = 1f;
    //[SerializeField, Range(0.01f, 100)]
    //float acceleration_coef = 1;
    //[SerializeField, Range(0.01f, 100)]
    //float angular_acceleration_coef = 1;
    enum TiltVersion // your custom enumeration
    {
        Triangle,
        Sine,
        Identity
    };


    [SerializeField, Range(-1, 1)]
    public float conditionalTilt = 0;
    [SerializeField, Range(-180, 180)]
    public float wrongPointOnRim = 0;
    [SerializeField, Range(-180, 180)]
    public float constantTiltX = 0;
    [SerializeField, Range(-180, 180)]
    public float constantTiltZ = 0;

    [SerializeField]
    float MaxWobble = 1f;
    [SerializeField]
    float WobbleFrequency = 3f;
    [SerializeField]
    float Recovery = 1f;
    [SerializeField]
    float angularCoef = 0.0005f;
    [SerializeField]
    float linearCoef = 0.004f;
    [SerializeField]
    TiltVersion tiltVersion;

    float pulse;
    float time = 0f;

    Vector3 waterNormal;

    float xWobble = 0, xWobbleV = 0, xPhase = 0;
    float zWobble = 0, zWobbleV = 0, zPhase = 0;

    float theta = 0, thetaV = 0;

    Vector3 acceleration, velocity, pos;
    Vector3 angular_acceleration, angular_velocity, rot;
    Quaternion rotQ;
    [SerializeField, Range(0, 1)]
    float acceleration_memory = 0;

    bool rstrt = false;


    public void setAngularCoef(float input)
    {
        angularCoef = input;
    }

    public void setLinearCoef(float input)
    {
        linearCoef = input;
    }

    public void setMaxWobble(float input)
    {
        MaxWobble = input;
    }

    public void setWobbleFrequency(float input)
    {
        WobbleFrequency = input;
    }

    public void setRecovery(float input)
    {
        Recovery = input;
    }

    public float getXWobble()
    {
        return xWobble;
    }

    public float getZWobble()
    {
        return zWobble;
    }

    private void InitMovementVariables()
    {
        pos = transform.position;
        rot = transform.rotation.eulerAngles;
        velocity = Vector3.zero;
        angular_velocity = Vector3.zero;
        acceleration = Vector3.zero;
        angular_acceleration = Vector3.zero;

    }

    private void UpdateMovementVariables(float deltaTime)
    {
        Vector3 newPos = transform.position;
        Vector3 newV = (newPos - pos) / deltaTime;
        //Vector3 newA = (newV - velocity) / deltaTime;
        Vector3 newA = 2 * ((newPos - pos) / (deltaTime * deltaTime));// - 2*(velocity/deltaTime) ;
        acceleration = (acceleration_memory * acceleration + (1f - acceleration_memory) * newA);
        velocity = newV;
        pos = newPos;

        Quaternion newRotQ = transform.rotation;
        Quaternion deltaRotQ = newRotQ * Quaternion.Inverse(rotQ);
        float angle; Vector3 axis;
        deltaRotQ.ToAngleAxis(out angle, out axis);
        if (angle > 180)
        {
            Debug.Log("bigger");
            deltaRotQ = Quaternion.AngleAxis(360 - angle, -axis);
        }
        Vector3 deltaRotQea = deltaRotQ.eulerAngles;
        if (Math.Abs(deltaRotQea.z) > 180)
            deltaRotQea.z = -Math.Sign(deltaRotQea.z) * (360 - Math.Abs(deltaRotQea.z));
        if (Math.Abs(deltaRotQea.x) > 180)
            deltaRotQea.x = -Math.Sign(deltaRotQea.x) * (360 - Math.Abs(deltaRotQea.x));
        if (Math.Abs(deltaRotQea.y) > 180)
            deltaRotQea.y = -Math.Sign(deltaRotQea.y) * (360 - Math.Abs(deltaRotQea.y));

        Vector3 newRot = transform.rotation.eulerAngles;
        Vector3 deltaRot = (newRot - rot);
        if (Math.Abs(deltaRot.z) > 180)
            deltaRot.z = -Math.Sign(deltaRot.z) * (360 - Math.Abs(deltaRot.z));
        if (Math.Abs(deltaRot.x) > 180)
            deltaRot.x = -Math.Sign(deltaRot.x) * (360 - Math.Abs(deltaRot.x));
        if (Math.Abs(deltaRot.y) > 180)
            deltaRot.y = -Math.Sign(deltaRot.y) * (360 - Math.Abs(deltaRot.y));

        //if (deltaRot != Vector3.zero)
        if (false)
        {
            Debug.Log("----");
            Debug.Log("rot: " + rot);
            Debug.Log("newRot: " + newRot);

            Debug.Log("deltaQ ea: "+deltaRotQ.eulerAngles);
            Debug.Log("deltaQ aa: "+angle + " " + axis);
            Debug.Log("deltaQ ea adjusted: " + deltaRotQea);
            
            Debug.Log("delta: "+deltaRot);
        }
        Vector3 newAV = deltaRotQea / deltaTime;
        angular_acceleration = (newAV - angular_velocity) / deltaTime;
        angular_velocity = newAV;
        //Debug.DrawLine(newPos, newPos + angular_velocity, new Color(1f, 0f, 0f), 1);
        rot = newRot;
        rotQ = newRotQ;
        //Debug.DrawLine(transform.position, transform.position + acceleration, new Color(1f, 1f, 1f));
        //Debug.DrawLine(transform.position, transform.position + velocity, new Color(1f, 1f, 0f));

        if (rstrt == true)
        {
            rstrt = false;
            acceleration = Vector3.zero;
            velocity = Vector3.zero;
            angular_acceleration = Vector3.zero;
            angular_velocity = Vector3.zero;
        }

    }

    private void ApplyTilt_depricated()
    {
        float xrot = transform.rotation.eulerAngles.x;
        float zrot = transform.rotation.eulerAngles.z;
        // translate to -90 to 90
        xrot = xrot > 180 ? xrot - 360 : xrot;
        xrot = xrot > 90 ? xrot - 180 : xrot < -90 ? xrot + 180 : xrot;
        zrot = zrot > 180 ? zrot - 360 : zrot;
        zrot = zrot > 90 ? zrot - 180 : zrot < -90 ? zrot + 180 : zrot;

        if (tiltVersion == TiltVersion.Sine)
        {
            // sin 
            xrot = 45 * Mathf.Sin(xrot * Mathf.PI / 90);
            zrot = 45 * Mathf.Sin(zrot * Mathf.PI / 90);
        }
        else if (tiltVersion == TiltVersion.Triangle)
        {
            // triangle 
            xrot = xrot < -45 ? -90 - xrot : xrot > 45 ? 90 - xrot : xrot;
            zrot = zrot < -45 ? -90 - zrot : zrot > 45 ? 90 - zrot : zrot;
        }
        else if (tiltVersion == TiltVersion.Identity)
        {
            xrot = -xrot;
            zrot = -zrot;
        }

        waterNormal = Quaternion.AngleAxis(constantTiltX + xrot * conditionalTilt, Vector3.right) * waterNormal;
        waterNormal = Quaternion.AngleAxis(constantTiltZ + zrot * conditionalTilt, Vector3.forward) * waterNormal;
    }

    private void ApplyTilt()
    {
        Vector3 object_y = transform.rotation * Vector3.up;
        Vector3 object_y_up = object_y.y > 0 ? object_y : -object_y;
        Debug.DrawLine(transform.position, transform.position+ Vector3.Normalize(object_y_up) *0.12f, new Color(1f, 1f, 1f), 0, false);
        drawLine(transform.position, transform.position+ Vector3.Normalize(object_y_up) *0.12f, new Color(1f, 1f, 1f));
        Vector3 axis = Vector3.Normalize(Vector3.Cross(Vector3.up, object_y));
        //Debug.DrawLine(transform.position, transform.position + axis, new Color(0f, 1f, 0f));
        float angle = Mathf.Rad2Deg*Mathf.Acos(Vector3.Dot(Vector3.up, object_y));
        angle = angle > 180 ? angle - 360 : angle;
        angle = angle > 90 ? angle - 180 : angle < -90 ? angle + 180 : angle;
        if (tiltVersion == TiltVersion.Sine)
        {
            // sin 
            angle = 45 * Mathf.Sin(angle * Mathf.PI / 90);
        }
        else if (tiltVersion == TiltVersion.Triangle)
        {
            // triangle 
            angle = angle < -45 ? -90 - angle : angle > 45 ? 90 - angle : angle;
        }
        else if (tiltVersion == TiltVersion.Identity)
        {
            angle = angle;
        }
        waterNormal= Quaternion.AngleAxis(conditionalTilt* angle, axis) * waterNormal;
        waterNormal = Quaternion.AngleAxis(constantTiltX, Vector3.right) * waterNormal;
        waterNormal = Quaternion.AngleAxis(constantTiltZ, Vector3.forward) * waterNormal;
        waterNormal = Quaternion.AngleAxis(wrongPointOnRim, object_y) * waterNormal;
    }

    private void drawWaterDebugLines()
    {
        // calculate x and z forces based on where water normal is now
        Vector3 axis = Vector3.Cross(Vector3.up, waterNormal);
        float deg = Mathf.Rad2Deg * Vector3.Magnitude(axis);
        Vector3 newx = Quaternion.AngleAxis(deg, axis) * Vector3.right;
        Vector3 newz = Quaternion.AngleAxis(deg, axis) * Vector3.forward;
        float accx = Vector3.Dot(acceleration, newx);
        float accz = Vector3.Dot(acceleration, newz);

        Debug.DrawLine(transform.position, transform.position + waterNormal, new Color(0f, 1f, 0f));
        Debug.DrawLine(transform.position, transform.position + newx, new Color(1f, 0f, 0f));
        Debug.DrawLine(transform.position, transform.position + newz, new Color(0f, 0f, 1f));
    }

    private void SimpleUpdateNormal()
    {
        time += Time.deltaTime;

        // decrease previous wobble over time
        xWobble = Mathf.Lerp(xWobble, 0, Time.deltaTime * (Recovery));
        zWobble = Mathf.Lerp(zWobble, 0, Time.deltaTime * (Recovery));

        xWobble += Mathf.Clamp(-velocity.x* linearCoef + angular_velocity.z * angularCoef, -MaxWobble, MaxWobble);
        zWobble += Mathf.Clamp(-velocity.z* linearCoef + angular_velocity.x * angularCoef, -MaxWobble, MaxWobble);

        pulse = 2 * Mathf.PI * WobbleFrequency;

        float wobbleAmountX = xWobble * Mathf.Sin(pulse * time);
        float wobbleAmountZ = zWobble * Mathf.Sin(pulse * time);

        waterNormal = new Vector3(wobbleAmountX, 1, wobbleAmountZ);
    }

    private void SimpleUpdateNormal2()
    {
        time += Time.deltaTime;

        // decrease previous wobble over time
        xWobble = Mathf.Lerp(xWobble, 0, Time.deltaTime * (Recovery));
        zWobble = Mathf.Lerp(zWobble, 0, Time.deltaTime * (Recovery));

        //float newXWobble = Mathf.Clamp(-acceleration.x * linearCoef + angular_velocity.z * angularCoef, -MaxWobble, MaxWobble);
        float newXWobble = -acceleration.x * linearCoef + angular_velocity.z * angularCoef;
        float newXPhase = -time * 2 * Mathf.PI * WobbleFrequency;// - Mathf.PI/2;
        //float newZWobble = Mathf.Clamp(-acceleration.z * linearCoef + angular_velocity.x * angularCoef, -MaxWobble, MaxWobble);
        float newZWobble = -acceleration.z * linearCoef + angular_velocity.x * angularCoef;
        float newZPhase = -time * 2 * Mathf.PI * WobbleFrequency;// - Mathf.PI/2;


        float tempXWobble = Mathf.Sqrt((Mathf.Pow(xWobble * Mathf.Cos(xPhase) + newXWobble * Mathf.Cos(newXPhase), 2) + Mathf.Pow(xWobble * Mathf.Sin(xPhase) + newXWobble * Mathf.Sin(newXPhase), 2)));
        xPhase = Mathf.Atan2(xWobble * Mathf.Sin(xPhase) + newXWobble * Mathf.Sin(newXPhase), xWobble * Mathf.Cos(xPhase) + newXWobble * Mathf.Cos(newXPhase));
        xWobble = Mathf.Clamp(tempXWobble, -MaxWobble, MaxWobble); 
        float tempZWobble = Mathf.Sqrt((Mathf.Pow(zWobble * Mathf.Cos(zPhase) + newZWobble * Mathf.Cos(newZPhase), 2) + Mathf.Pow(zWobble * Mathf.Sin(zPhase) + newZWobble * Mathf.Sin(newZPhase), 2)));
        zPhase = Mathf.Atan2(zWobble * Mathf.Sin(zPhase) + newZWobble * Mathf.Sin(newZPhase), zWobble * Mathf.Cos(zPhase) + newZWobble * Mathf.Cos(newZPhase));
        zWobble = Mathf.Clamp(tempZWobble, -MaxWobble, MaxWobble);

        pulse = 2 * Mathf.PI * WobbleFrequency;

        float wobbleAmountX = xWobble * Mathf.Cos(pulse * time + xPhase);
        float wobbleAmountZ = zWobble * Mathf.Cos(pulse * time + zPhase);

        waterNormal = new Vector3(wobbleAmountX, 1, wobbleAmountZ);
        if (acceleration != Vector3.zero)
        {
            Debug.Log("acceleration was: " + acceleration);
        }
    }


    private void UpdateNormal()
    {
        // max wobble = 3
        // wobble speed = 3
        // recovery = 2

        time += Time.deltaTime;

        // decrease previous wobble over time
        xWobble = Mathf.Lerp(xWobble, 0, Time.deltaTime * (Recovery));
        zWobble = Mathf.Lerp(zWobble, 0, Time.deltaTime * (Recovery));
        xWobbleV = Mathf.Lerp(xWobbleV, 0, Time.deltaTime * (Recovery));
        zWobbleV = Mathf.Lerp(zWobbleV, 0, Time.deltaTime * (Recovery));

        // add acceleration to wobble speed
        xWobbleV += Mathf.Clamp(-acceleration.x + angular_velocity.z * 0.0002f, -MaxWobble, MaxWobble);
        zWobbleV += Mathf.Clamp(-acceleration.z + angular_velocity.x * 0.0002f, -MaxWobble, MaxWobble);

        // add wobble speed to wobble
        xWobble += xWobbleV * Time.deltaTime;
        zWobble += zWobbleV * Time.deltaTime;

        // make a sine wave of the decreasing wobble
        pulse = 2 * Mathf.PI * WobbleFrequency;

        float wobbleAmountX = xWobble * Mathf.Sin(pulse * time);
        float wobbleAmountZ = zWobble * Mathf.Sin(pulse * time);

        waterNormal = new Vector3(wobbleAmountX, 1, wobbleAmountZ);
    }

    private void UpdateNormalPendulum()
    {
        time += Time.deltaTime;
        thetaV = Mathf.Lerp(thetaV, 0, Time.deltaTime * Recovery);

        float thetaA = ((acceleration.x) * Mathf.Cos(theta) + (9.8f * 250f - acceleration.y) * Mathf.Sin(theta)) / -10f;
        thetaA += angular_velocity.z * 0.01f;

        thetaV += thetaA * Time.deltaTime;
        theta += thetaV * Time.deltaTime;
        if (theta > Mathf.PI / 3 || theta < -Mathf.PI / 3)
        {
            thetaV = -thetaV;
        }

        waterNormal = new Vector3(Mathf.Sin(theta), Mathf.Cos(theta), 0);
    }

    public void restart()
    {
        zWobble = 0;
        xWobble = 0;
        rstrt = true;
    }

    void Start()
    {
        rend = GetComponent<Renderer>();
        waterNormal = Vector3.up;
        InitMovementVariables();
    }

    private void Update()
    {
        SimpleUpdateNormal2();//actual normal
        ApplyTilt();//after the gravity change
        UpdateMovementVariables(Time.deltaTime);

        rend.material.SetVector("_WaterNormal", waterNormal);

        Debug.DrawLine(transform.position, transform.position + waterNormal*0.12f, new Color(0, 0.5f, 1f), 0, false);
        drawLine(transform.position, transform.position + waterNormal*0.12f, new Color(0, 0.5f, 1f));
        Debug.DrawLine(transform.position, transform.position + Vector3.up*0.12f, new Color(0, 1f, 0), 0, false);
        drawLine(transform.position, transform.position + Vector3.up*0.12f, new Color(0, 1f, 0));
    }

}