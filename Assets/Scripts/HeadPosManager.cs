using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadPosManager : MonoBehaviour
{
    [SerializeField]
    GameObject head;
    protected List<Vector3> headPosData;
    protected List<Vector3> headRotData;
    // Start is called before the first frame update
    void Start()
    {
        headPosData = new List<Vector3>();
        headRotData = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        headPosData.Add(head.transform.position);
        headRotData.Add(head.transform.eulerAngles);
    }

    public void restart()
    {
        headPosData = new List<Vector3>();
        headRotData = new List<Vector3>();
    }

    public List<Vector3> getPosData()
    {
        return headPosData;
    }

    public List<Vector3> getRotData()
    {
        return headRotData;
    }
}
