using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMeWtihAD : MonoBehaviour
{
    private GameObject me;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("g"))
        {
            transform.rotation = Quaternion.AngleAxis(0.1f, Vector3.forward) * transform.rotation;
        }
        if (Input.GetKey("j"))
        {
            transform.rotation = Quaternion.AngleAxis(-0.1f, Vector3.forward) * transform.rotation;
        }
        if (Input.GetKey("h"))
        {
            transform.rotation = Quaternion.AngleAxis(0.1f, Vector3.left) * transform.rotation;
        }
        if (Input.GetKey("y"))
        {
            transform.rotation = Quaternion.AngleAxis(-0.1f, Vector3.left) * transform.rotation;
        }
        if (Input.GetKey("b"))
        {
            transform.rotation = Quaternion.AngleAxis(0.1f, Vector3.up) * transform.rotation;
        }
        if (Input.GetKey("n"))
        {
            transform.rotation = Quaternion.AngleAxis(-0.1f, Vector3.up) * transform.rotation;
        }
    }
}
