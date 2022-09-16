using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHistoryManager : MonoBehaviour
{
    [SerializeField]
    GameObject head;
    [SerializeField]
    GameObject glass1;
    [SerializeField]
    GameObject glass2;

    protected List<Vector3> headPosData;
    protected List<Vector3> headRotData;

    protected List<Vector3> glass1PosData;
    protected List<Vector3> glass1RotData;

    protected List<Vector3> glass2PosData;
    protected List<Vector3> glass2RotData;
    // Start is called before the first frame update
    void Start()
    {
        headPosData = new List<Vector3>();
        headRotData = new List<Vector3>();
        glass1PosData = new List<Vector3>();
        glass1RotData = new List<Vector3>();
        glass2PosData = new List<Vector3>();
        glass2RotData = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        headPosData.Add(head.transform.position);
        headRotData.Add(head.transform.eulerAngles);
        if (glass1 != null)
        {
            glass1PosData.Add(glass1.transform.position);
            glass1RotData.Add(glass1.transform.eulerAngles);
        }
        if (glass2 != null) { 
            glass2PosData.Add(glass2.transform.position);
            glass2RotData.Add(glass2.transform.eulerAngles);
        }
    }

    public void restart()
    {
        headPosData = new List<Vector3>();
        headRotData = new List<Vector3>();
        glass1PosData = new List<Vector3>();
        glass1RotData = new List<Vector3>();
        glass2PosData = new List<Vector3>();
        glass2RotData = new List<Vector3>();
    }

    public List<Vector3> getHeadPosData()
    {
        return headPosData;
    }

    public List<Vector3> getHeadRotData()
    {
        return headRotData;
    }

    public List<Vector3> getGlass1PosData()
    {
        return glass1PosData;
    }

    public List<Vector3> getGlass1RotData()
    {
        return glass1RotData;
    }

    public List<Vector3> getGlass2PosData()
    {
        return glass2PosData;
    }

    public List<Vector3> getGlass2RotData()
    {
        return glass2RotData;
    }
}
