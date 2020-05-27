using BS.BL.Interface;
using BS.BL.Two.Element;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BS.BL.Two
{
    public class WayPoints : MonoBehaviour
    {
        public GameObject goPoint;
        public float lastKickTime = 0f;
        private Transform clickTrans;
        public Dictionary<string, GameObject> dicPoints = new Dictionary<string, GameObject>();
        public Material matNormal, matSelect;
        // Start is called before the first frame update
        void Start()
        {
            lastKickTime = Time.realtimeSinceStartup;
        }

        // Update is called once per frame
        void Update()
        {
            if (ElementContainer.GetInstance().domMode == DomMode.lx || ElementContainer.GetInstance().domMode == DomMode.xj)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    DoubleClick();
                }
            }
        }
        public void DoubleClick()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name.Split('_')[0] == "point")
                {
                    foreach (KeyValuePair<string, GameObject> item in dicPoints)
                    {
                        item.Value.GetComponent<MeshRenderer>().material = matNormal;
                    }
                    hit.transform.GetComponent<MeshRenderer>().material = matSelect;
                    string calljs = "{\"lineId\":\"" + hit.transform.name.Split('_')[1] + "\",\"distance\":" + hit.transform.name.Split('_')[2] + "}";
                    GameObject.Find("JSInterface").GetComponent<JSInterface>().selectCurrentRes(calljs);
                }
                if (Time.realtimeSinceStartup - lastKickTime < 0.2)//检测上次点击的时间和当前时间差 在一定范围内认为是双击
                {
                    if (hit.transform.parent.GetComponent<Line>() != null)
                    {
                        GameObject go = Instantiate(goPoint) as GameObject;
                        go.transform.parent = transform;
                        Line line = hit.transform.parent.GetComponent<Line>();
                        Vector3 startPos = ElementContainer.GetInstance().tviewBase.transform.TransformPoint(line.startPoint.pos);
                        Vector3 endPos = ElementContainer.GetInstance().tviewBase.transform.TransformPoint(line.endPoint.pos);
                        go.transform.position = GetAxisPos(startPos, endPos, hit.point);
                        float distance = Vector3.Distance(go.transform.position, startPos) / 2;
                        go.name = "point_" + line.id + "_" + distance.ToString();
                        if (!dicPoints.ContainsKey(go.name))
                        {
                            dicPoints.Add(go.name, go);
                        }
                        string wayTemp = "{\"lineId\":\"" + line.id + "\",\"distance\":" + distance + "}";
                        if (ElementContainer.GetInstance().domMode == DomMode.lx)
                        {
                            GameObject.Find("JSInterface").GetComponent<JSInterface>().sendWayPointPosition(wayTemp);
                        }
                        else
                        {
                            GameObject.Find("JSInterface").GetComponent<JSInterface>().sendInspectionPointPosition(wayTemp);
                        }
                    }

                }
                lastKickTime = Time.realtimeSinceStartup;//重新设置上次点击的时间
            }
        }
        private Vector3 GetAxisPos(Vector3 startPoint, Vector3 endPoint, Vector3 outPoint)
        {
            Vector3 axisPoint = new Vector3();
            float o_seDistance = MathTools.pointToLine(startPoint, endPoint, outPoint);//外边的点到线的垂直距离
            float o_sDistance = Vector3.Distance(startPoint, outPoint);//外边的点到起点的距离
            float s_aDistance = Mathf.Sqrt(o_sDistance * o_sDistance - o_seDistance * o_seDistance);//起点到垂直相交的点的距离
            Vector3 s_eFor = (endPoint - startPoint).normalized;//起点到终点的方向向量
            axisPoint = startPoint + s_eFor * s_aDistance;//交点坐标
            return axisPoint;
        }
    }
}
