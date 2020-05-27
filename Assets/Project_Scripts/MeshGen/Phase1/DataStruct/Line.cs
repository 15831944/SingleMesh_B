using DXFConvert;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

    public string id;
    public Point startPoint, endPoint;

    LINE lineStruct;

    public float lineLenght = 0;

    public Material dataSetMat,selectMat, unselectMat;

    public Material tunnelMat;

    public GameObject tunnel;

    public GameObject normalGo, tunnelGo;

    private bool isDateSet = false;
    private bool isSelect = false;
    // Use this for initialization
    void Start () {
        //transform.GetChild(0).GetComponent<MeshRenderer>().material = dataSetMat;

    }
	
	// Update is called once per frame
	void Update () {
        //CheckMat();
	}

    public void Set(string Id, LINE line,float autoScale = 1f) {
        this.id = Id;
        lineStruct = line;

        var goLayer = TViewBase.Content.GetLayer(line.C8);
        if (goLayer != null)
        {
            //GenerateLine();
        }

        var p1 = new Vector3((float)line.C10/ autoScale, (float)line.C30/ autoScale, (float)line.C20/ autoScale);
        var p2 = new Vector3((float)line.C11/ autoScale, (float)line.C31/ autoScale, (float)line.C21/ autoScale);

        MeshGenerater.DataContainer.GetInstance().AddCommonNode(p1);
        MeshGenerater.DataContainer.GetInstance().AddCommonNode(p2);

        lineLenght = Vector3.Distance(p1, p2);

        transform.localScale = new Vector3(5, 5, lineLenght/2);
        transform.localPosition = (p1 + p2) / 2;
        transform.LookAt(p2);

        gameObject.name = "Line_" + Id;
        gameObject.isStatic = true;
        if (!string.IsNullOrEmpty(line.C62))
        {
            Material matTemp = new Material(dataSetMat);
            matTemp.color = InsertToRGB.IndexToRGB(int.Parse(line.C62));
            transform.GetChild(0).GetComponent<MeshRenderer>().material = matTemp;
        }
        else
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material = dataSetMat;
        }
        startPoint = new Point(id + "_S", p1);
        endPoint = new Point(id + "_E", p2);
        MeshGenerater.DataContainer.GetInstance().AddPoint(startPoint);
        MeshGenerater.DataContainer.GetInstance().AddPoint(endPoint);
        normalGo = transform.GetChild(0).gameObject;
    }


    public void SetStyleByMode() {
        normalGo.SetActive(false);

    }

    public void Select(bool isSelect) {
        transform.GetChild(0).GetComponent<MeshRenderer>().material = isSelect ? selectMat : unselectMat;
        this.isSelect = isSelect;
        if (isSelect)
        {
        }
    }

    public void SetData() {
        isDateSet = true;
    }

    public void CheckMat() {
        if (isSelect)
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material = selectMat;
        }
        else {
            transform.GetChild(0).GetComponent<MeshRenderer>().material = isDateSet ? dataSetMat : unselectMat;
        }
    }
}
