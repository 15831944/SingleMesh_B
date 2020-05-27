using DXFConvert;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointDXF : MonoBehaviour
{
    public string id;
    public Point startPoint, endPoint;

    POINT pointStrcut;

    private float lineLenght = 0;

    public Material dataSetMat, selectMat, unselectMat;

    private bool isDateSet = false;
    private bool isSelect = false;
    // Use this for initialization
    void Start()
    {
        transform.GetComponent<MeshRenderer>().material = dataSetMat;

    }

    // Update is called once per frame
    void Update()
    {
        //CheckMat();
    }

    public void Set(string Id, POINT point, float autoScale = 1f)
    {
        this.id = Id;
        pointStrcut = point;

        var goLayer = TViewBase.Content.GetLayer(point.C8);
        if (goLayer != null)
        {
            //GenerateLine();
        }

        var p1 = new Vector3((float)point.C10 / autoScale, (float)point.C30 / autoScale, (float)point.C20 / autoScale);
        
        transform.localPosition = p1;
        if (RayTools.GetAixaHD(transform) != null)
        {
            transform.forward = RayTools.GetAixaHD(transform).parent.forward;
            //Debug.LogError(Id + "控制方向" + transform.forward + "父物体的前边" + RayTools.GetAixaHD(transform).parent.forward);
            //Vector3 targetPos = transform.position - transform.up - transform.forward * 50 + transform.right;
            //GameObject go = new GameObject();
            //go.transform.parent = transform;
            //go.transform.position = targetPos;

        }
        gameObject.name = "Point_" + Id;
    }

    public void Select(bool isSelect)
    {
        transform.GetComponent<MeshRenderer>().material = isSelect ? selectMat : unselectMat;
        this.isSelect = isSelect;
        if (isSelect)
        {
        }
    }

    public void SetData()
    {
        isDateSet = true;
    }

    public void CheckMat()
    {
        if (isSelect)
        {
            transform.GetComponent<MeshRenderer>().material = selectMat;
        }
        else
        {
            transform.GetComponent<MeshRenderer>().material = isDateSet ? dataSetMat : unselectMat;
        }
    }
}
