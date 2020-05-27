using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MeshGenerater
{
    public class DataContainer : MonoBehaviour
    {
        private Dictionary<string,Line> lineList = new Dictionary<string,Line>();
        private Dictionary<string, PointDXF> pointDXFList = new Dictionary<string, PointDXF>();
        private Dictionary<string,Node> nodeList = new Dictionary<string,Node>();
        private Dictionary<string, Point> pointList = new Dictionary<string, Point>();

        private List<Vector3> nodePosList = new List<Vector3>();

        private Vector3 centerPos = Vector3.zero;

        public Text errorText;
        public Transform modelRoot;
        private bool isError = false;
        private static DataContainer instance;

        public static DataContainer GetInstance() {
            return instance;
        }

        void Awake() {
            instance = this;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddLine(Line line) {
            try
            {
                if (lineList.ContainsKey(line.id))
                {
                    isError = true;
                    errorText.gameObject.SetActive(true);
                    errorText.text = "重复线段ID：" + line.id;
                }
                else
                {
                    lineList.Add(line.id, line);
                }
            }
            catch 
            {
                isError = true;
                errorText.gameObject.SetActive(true);
                errorText.text = "重复线段ID：" + line.id;
            }
           
        }

        public List<Line> GetLines() {
            return lineList.Values.ToList();
        }

        public void AddNode(Node node) {
            nodeList.Add(node.id,node);
        }

        public List<Node> GetNodes() {
            return nodeList.Values.ToList();
        }

        public List<Vector3> GetNodePosList() {
            return nodePosList;
        }

        public void AddPoint(Point point) {
            try
            {
                if (pointList.ContainsKey(point.id))
                {
                    isError = true;
                    errorText.gameObject.SetActive(true);
                    errorText.text += " " + point.id;
                }
                else
                {
                    pointList.Add(point.id, point);
                }
            }
            catch {
                isError = true;
                errorText.gameObject.SetActive(true);
                errorText.text = "重复点ID：" + point.id;
            }
        }
        public void AddPointDSF(PointDXF pointDXF) {
            try
            {
                pointDXFList.Add(pointDXF.id, pointDXF);
            }
            catch 
            {
                Debug.LogError("重复ID" + pointDXF.id);
                //throw;
            }
           
        }

        public void ResetError() {
            //errorText.text = "Same ID Existed: ";
            //errorText.gameObject.SetActive(false);
        }

        private void ClearLists() {
            pointList.Clear();
            lineList.Clear();
            nodeList.Clear();
        }

        private void ClearScene() {
            for (var index = 0; index < modelRoot.childCount; index++) {
                GameObject.Destroy(modelRoot.GetChild(index).gameObject,0f);
            }
        }

        public Point GetPointById(string id) {
            if (pointList.ContainsKey(id)) {
                return pointList[id];
            }
            return null;
        }

        
        public List<Point> GetPointListByPos(Vector3 pos) {
            List<Point> pointList = new List<Point>();
            foreach (Point point in this.pointList.Values) {
                if (point.pos == pos) {
                    pointList.Add(point);
                }
            }
            return pointList;
        }

        ///////////////////////NodeContainer////////////////////////////

        /// <summary>
        /// 各线公共点 用来判断路口岔口数
        /// </summary>
        private Dictionary<Vector3, int> commonNodes = new Dictionary<Vector3, int>();

        /// <summary>
        /// 各线两端端点
        /// </summary>
        private List<EndPoint> endPointList = new List<EndPoint>();

        /// <summary>
        /// 岔路口数目 节点结合
        /// </summary>
        private Dictionary<int, List<Vector3>> nodeXList = new Dictionary<int, List<Vector3>>();

        public Dictionary<Vector3, int> GetCommonNodes()
        {
            return commonNodes;
        }

        public void AddCommonNode(Vector3 point)
        {
            if (commonNodes.ContainsKey(point))
            {
                commonNodes[point]++;
                return;
            }
            commonNodes.Add(point, 1);
        }

        public void CaculateNodes()
        {
            nodeXList.Clear();
            foreach (KeyValuePair<Vector3, int> node in commonNodes)
            {
                if (!nodeXList.ContainsKey(node.Value))
                {
                    nodeXList.Add(node.Value, new List<Vector3>());
                }
                nodeXList[node.Value].Add(node.Key);
                nodePosList.Add(node.Key);
            }
        }

        /// <summary>
        /// 获取每种岔口的坐标列表
        /// </summary>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        public List<Vector3> GetNodesList(int nodeType)
        {
            if (nodeXList.ContainsKey(nodeType))
            {
                return nodeXList[nodeType];
            }
            return null;
        }

        public void AddEndPoint(EndPoint endPoint)
        {
            endPointList.Add(endPoint);
        }

        public List<EndPoint> GetEndPoints()
        {
            return endPointList;
        }

        public void SelectElement(Transform transform) {
            foreach (Line line in lineList.Values) {
                line.Select(line.transform == transform);
            }

            foreach (Node node in nodeList.Values)
            {
                node.Select(node.transform == transform);
            }
        }

        public void SetDataElement(string elementId) {
            if (lineList.ContainsKey(elementId)) {
                lineList[elementId].SetData();
            }
            if (nodeList.ContainsKey(elementId)) {
                nodeList[elementId].SetData();
            }
        }

        public Vector3 GetCenterPos() {
            foreach (Vector3 pos in nodePosList) {
                centerPos += pos;
            }
            if (nodePosList.Count == 0) {
                return Vector3.zero;
            }
            return centerPos / nodePosList.Count;
        }

        DataMap dataMap;
        public void SerializeData() {
            if (isError) {
                isError = false;
                ClearLists();
                ClearScene();
                return;
            }
            string mapDataStr = "";
            dataMap = new DataMap(nodeList.Values.ToList(), lineList.Values.ToList());
            mapDataStr = JsonUtility.ToJson(dataMap);
        }
        /// <summary>
        /// 清除所有List
        /// </summary>
        public void ClearAllList()
        {
            pointList.Clear();
            lineList.Clear();
            nodeList.Clear();
            nodePosList.Clear();
        }
    }
}