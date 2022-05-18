using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneTiltController : MonoBehaviour
{
    [SerializeField]
    Glass glass;

    private void Start()
    {
        transform.eulerAngles= new Vector3( -glass.getConstantTiltX(), 0, -glass.getConstantTiltZ());
    }

    void Update()
    {
        float xrot = transform.rotation.eulerAngles.x;
        float zrot = transform.rotation.eulerAngles.z;
        xrot = xrot > 180 ? xrot - 360 : xrot;
        xrot = xrot > 90 ? xrot - 180 : xrot < -90 ? xrot + 180 : xrot;
        zrot = zrot > 180 ? zrot - 360 : zrot;
        zrot = zrot > 90 ? zrot - 180 : zrot < -90 ? zrot + 180 : zrot;

        //xrot = xrot < -45 ? -90 - xrot : xrot > 45 ? 90 - xrot : xrot;
        //zrot = zrot < -45 ? -90 - zrot : zrot > 45 ? 90 - zrot : zrot;
        glass.setConstantTiltX(-xrot);
        glass.setConstantTiltZ(-zrot);
    }
}
