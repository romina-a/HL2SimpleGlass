using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour
{
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

    [SerializeField, Range(-1, 1)]
    public float conditionalTilt = 0;
    [SerializeField, Range(-180, 180)]
    public float constantTiltX = 0;
    [SerializeField, Range(-180, 180)]
    public float constantTiltZ = 0;

    [SerializeField]
    float MaxWobble = 0.003f;
    [SerializeField]
    float WobbleSpeed = 3f;
    [SerializeField]
    float Recovery = 2f;
    [SerializeField]
    float angularCoef = 0.0000002f;

    [SerializeField]
    float pulse;
    float time = 0f;

    Vector3 waterNormal;

    float xWobble = 0, xWobbleV = 0;
    float zWobble = 0, zWobbleV = 0;

    float theta = 0, thetaV = 0;
    float phi = 0, phiV = 0;

    Vector3 acceleration, velocity, pos;
    Vector3 angular_acceleration, angular_velocity, rot;
    [SerializeField, Range(0, 1)]
    float acceleration_memory = 0;

    float elapsed_time_pos, elapsed_time_rot;

    private void InitMovementVariables()
    {
        pos = transform.position;
        rot = transform.rotation.eulerAngles;
        velocity = Vector3.zero;
        angular_velocity = Vector3.zero;
        acceleration = Vector3.zero;
        angular_acceleration = Vector3.zero;
        elapsed_time_pos = 0;
        elapsed_time_rot = 0;

    }

    private void UpdateMovementVariables(float deltaTime)
    {
        Vector3 newPos = transform.position;
        Vector3 newV = (newPos - pos) / deltaTime;
        Vector3 newA = (newV - velocity) / deltaTime;
        acceleration = (acceleration_memory * acceleration + (1f - acceleration_memory) * newA);
        velocity = newV;
        pos = newPos;
        if (acceleration == Vector3.zero)
        {
            elapsed_time_pos += deltaTime;
        }
        else
        {
            elapsed_time_pos = 0;
        }
        Vector3 newRot = transform.rotation.eulerAngles;
        Vector3 newAV = (newRot - rot) / deltaTime;
        angular_acceleration = (newAV - angular_velocity) / deltaTime;
        angular_velocity = newAV;
        rot = newRot;
        if (angular_acceleration == Vector3.zero)
        {
            elapsed_time_rot += deltaTime;
        }
        else
        {
            elapsed_time_rot = 0;
        }
        Debug.DrawLine(transform.position, transform.position + acceleration, new Color(1f, 1f, 1f));
        Debug.DrawLine(transform.position, transform.position + velocity, new Color(1f, 1f, 0f));

    }

    private void ApplyTilt()
    {
        float xrot = transform.rotation.eulerAngles.x;
        float zrot = transform.rotation.eulerAngles.z;
        xrot = xrot > 180 ? xrot - 360 : xrot;
        xrot = xrot > 90 ? xrot - 180 : xrot < -90 ? xrot + 180 : xrot;
        zrot = zrot > 180 ? zrot - 360 : zrot;
        zrot = zrot > 90 ? zrot - 180 : zrot < -90 ? zrot + 180 : zrot;

        xrot = xrot < -45 ? -90 - xrot : xrot > 45 ? 90 - xrot : xrot;
        zrot = zrot < -45 ? -90 - zrot : zrot > 45 ? 90 - zrot : zrot;

        waterNormal = Quaternion.AngleAxis(constantTiltX + xrot * conditionalTilt, Vector3.right) * waterNormal;
        waterNormal = Quaternion.AngleAxis(constantTiltZ + zrot * conditionalTilt, Vector3.forward) * waterNormal;
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

        xWobble += Mathf.Clamp(-velocity.x + angular_velocity.z * angularCoef, -MaxWobble, MaxWobble);
        zWobble += Mathf.Clamp(-velocity.z + angular_velocity.x * angularCoef, -MaxWobble, MaxWobble);

        pulse = 2 * Mathf.PI * WobbleSpeed;

        float wobbleAmountX = xWobble * Mathf.Sin(pulse * time);
        float wobbleAmountZ = zWobble * Mathf.Sin(pulse * time);

        waterNormal = new Vector3(wobbleAmountX, 1, wobbleAmountZ);
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
        pulse = 2 * Mathf.PI * WobbleSpeed;

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

    void Start()
    {
        rend = GetComponent<Renderer>();
        waterNormal = Vector3.up;
        InitMovementVariables();
    }

    private void Update()
    {
        SimpleUpdateNormal();//actual normal
        ApplyTilt();//after the gravity change
        UpdateMovementVariables(Time.deltaTime);

        rend.material.SetVector("_WaterNormal", waterNormal);

        Debug.DrawLine(transform.position, transform.position + waterNormal, new Color(0f, 1f, 0f));
    }

}