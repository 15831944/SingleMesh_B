using DXFConvert;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TLine : MonoBehaviour
{
    public GameObject lineTypeGo;

    private float LineLenght = 0;

    private GameObject go;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void GenerateLine() {
        if (go == null) {
            go = Instantiate(lineTypeGo);
            go.transform.parent = transform;
        }
    }

    public void Set(DXFStructure dxf, LINE item)
    {
        var goLayer = TView.Content.GetLayer(item.C8);
        if (goLayer != null)
        {
            GenerateLine();
        }

        var p1 = new Vector3((float)item.C10, (float)item.C30, (float)item.C20);
        var p2 = new Vector3((float)item.C11, (float)item.C31, (float)item.C21);

        MeshGenerater.DataContainer.GetInstance().AddCommonNode(p1);
        MeshGenerater.DataContainer.GetInstance().AddCommonNode(p2);

        LineLenght = Vector3.Distance(p1, p2)/1.905f;

        go.transform.localScale = new Vector3(10,10,LineLenght);
        go.transform.localPosition = (p1 + p2) / 2;
        go.transform.LookAt(p2);

        gameObject.isStatic = true;
    }
}
