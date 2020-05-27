using BS.BL.Interface;
using BS.BL.Two.Element;
using DXFConvert;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TLayerBase : MonoBehaviour
{
    public static TLayerBase DefaultLayer;

    //图层材质
    public Material LayerMaterial;

    LAYER Layer;
    public List<LINE> LINEList { get; set; }
    public List<TEXT> TEXTList { get; set; }
    public List<POINT> POINTList { get; set; }
    public Dictionary<string,cadMessage> cadMessages =new Dictionary<string, cadMessage>();
    //public List<TEXT> AutoScaleText { get; set; }
    public GameObject Line, Node, Point;

    public float idNearGap = 0.000001f;

    public GameObject goPoints;

    public void Set(LAYER layer)
    {
        Layer = layer;
        gameObject.name = "Layer_" + layer.C2;
    }

    public void Load(DXFStructure dxf)
    {
        //找到当前层的物体
        LINEList = dxf.ENTITIES.LINEList.Where(x => x.C8 == Layer.C2).ToList();
        TEXTList = dxf.ENTITIES.TEXTList.Where(x => x.C8 == Layer.C2).ToList();
        POINTList = dxf.ENTITIES.POINTList.Where(x => x.C8 == Layer.C2).ToList();
        //AutoScaleText = dxf.ENTITIES.TEXTList.Where(x=>x.C8 == "BL").ToList();

        //if (AutoScaleText.Count == 0) {
        //    Debug.Log("CAD比例尺未标定!");
        //}

        //绘制层下属物体
        DrawLINEList(LINEList);
        GenNodeList();
        ResetCenterPos();
        Invoke("DrawPointLater", 0.1f);
        Invoke("TelPageLater", 1);
    }
    private void DrawPointLater() {
        DrawPointList(POINTList);
    }
    private void TelPageLater() {
        ElementContainer.GetInstance().cameraStartTrans = Camera.main.transform;
        GameObject.Find("JSInterface").GetComponent<JSInterface>().initFinished();
        SendMessageToWeb();
    }
    private void SendMessageToWeb(){
        if (cadMessages.Count > 0)
        {
            string ballmessage = "[";
            foreach (KeyValuePair<string,cadMessage> item in cadMessages)
            {
                ballmessage += "{\"lineId\":\""+item.Key + "\",\"length\":" + item.Value.length.ToString() + "},";
            }
            ballmessage = ballmessage.Substring(0,ballmessage.Length - 1) + "]";
            GameObject.Find("JSInterface").GetComponent<JSInterface>().sendAllLine(ballmessage);
        }
    }
    //绘制直线集合
    public void DrawLINEList(List<LINE> LINEList)
    {
        GameObject goLines = new GameObject();
        goLines.transform.parent = gameObject.transform;
        goLines.transform.localPosition = Vector3.zero;
        goLines.name = "Lines";
        //绘制直线
        foreach (LINE item in LINEList)
        {
            GameObject go = Instantiate(Line) as GameObject;
            go.tag = "HD";
            go.transform.GetChild(0).tag = "HD";
            go.transform.parent = goLines.transform;
            var line = go.GetComponent<Line>();
            string lineId = GetLineId(item);
            line.Set(lineId, item);// float.Parse(AutoScaleText[0].C1.Split(':')[1])
            float length = line.lineLenght;
            if (!cadMessages.ContainsKey(lineId))
            {
                cadMessage cad = new cadMessage();
                cad.lineId = lineId;
                cad.length = length;
                cadMessages.Add(lineId,cad);
            }
            MeshGenerater.DataContainer.GetInstance().AddLine(line);
        }
    }
    /// <summary>
    /// 绘制点
    /// </summary>
    /// <param name="pOINTs"></param>
    private List<string> pointIds = new List<string>();
    public void DrawPointList(List<POINT> pOINTs) {
        goPoints = new GameObject();
        goPoints.transform.parent = gameObject.transform;
        goPoints.transform.localPosition = Vector3.zero;
        goPoints.name = "Points";
        foreach (POINT item in pOINTs)
        {
            if (!pointIds.Contains(GetPointDXFId(item)))
            {
                GameObject go = Instantiate(Point) as GameObject;
                go.tag = "HD";
                go.transform.parent = goPoints.transform;
                var point = go.GetComponent<PointDXF>();
                point.Set(GetPointDXFId(item), item);
                MeshGenerater.DataContainer.GetInstance().AddPointDSF(point);
                pointIds.Add(GetPointDXFId(item));
            }
        }
    }

    private int nodeId = 0;
    public void GenNodeList() {
        MeshGenerater.DataContainer.GetInstance().CaculateNodes();
        List<Vector3> nodePosList = MeshGenerater.DataContainer.GetInstance().GetNodePosList();
        nodeId = 0;
        foreach (Vector3 pos in nodePosList) {
            GameObject go = Instantiate(Node) as GameObject;
            go.transform.position = pos;
            go.transform.parent = transform.Find("Nodes");
            go.name = "node" + nodeId++;
            go.GetComponent<Node>().id = go.name;
            go.GetComponent<Node>().SetPointList(MeshGenerater.DataContainer.GetInstance().GetPointListByPos(pos));
            MeshGenerater.DataContainer.GetInstance().AddNode(go.GetComponent<Node>());
        }
    }


    private string GetLineId(LINE item) {
        
        Vector3 linePos = (new Vector3((float)item.C10, (float)item.C20, (float)item.C30)+ new Vector3((float)item.C11, (float)item.C21, (float)item.C31))/2f;
        string id = "default " + linePos;
        for (var index = 0; index < TEXTList.Count; index++) {
            if (Vector2.Distance(linePos, new Vector3((float)TEXTList[index].C10, (float)TEXTList[index].C20, (float)TEXTList[index].C30)) <= idNearGap)
            {
                //Debug.Log(Vector3.Distance(linePos, new Vector3((float)TEXTList[index].C10, (float)TEXTList[index].C20, (float)TEXTList[index].C30)));
                return TEXTList[index].C1;
            }
        }
        return id;
    }
    private string GetPointDXFId(POINT item)
    {
        Vector3 pointPos = new Vector3((float)item.C10, (float)item.C20, (float)item.C30);
        string id = "default " + pointPos;
        for (var index = 0; index < TEXTList.Count; index++)
        {
            if (Vector3.Distance(pointPos, new Vector3((float)TEXTList[index].C10, (float)TEXTList[index].C20, (float)TEXTList[index].C30)) <= idNearGap)
            {
                Debug.Log(Vector3.Distance(pointPos, new Vector3((float)TEXTList[index].C10, (float)TEXTList[index].C20, (float)TEXTList[index].C30)));
                return TEXTList[index].C1;
            }
        }
        return id;
    }

    private void ResetCenterPos() {
        GameObject.Find("TViewBase").transform.position -= MeshGenerater.DataContainer.GetInstance().GetCenterPos();
    }
}
