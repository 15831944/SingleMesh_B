using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeContainer : MonoBehaviour {

    private static NodeContainer instance;

    public static NodeContainer GetInstance() {
        return instance;
    }

    //   /// <summary>
    //   /// 各线公共点 用来判断路口岔口数
    //   /// </summary>
    //   private Dictionary<Vector3, int> commonNodes = new Dictionary<Vector3, int>();

    //   /// <summary>
    //   /// 各线两端端点
    //   /// </summary>
    //   private List<EndPoint> endPointList = new List<EndPoint>();

    //   private Dictionary<int, List<Vector3>> nodeXList = new Dictionary<int, List<Vector3>>();

    //   void Awake() {
    //       instance = this;
    //   }

    //   // Use this for initialization
    //   void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    //   public Dictionary<Vector3, int> GetCommonNodes()
    //   {
    //       return commonNodes;
    //   }

    //   public void AddCommonNode(Vector3 point)
    //   {
    //       if (commonNodes.ContainsKey(point))
    //       {
    //           commonNodes[point]++;
    //           return;
    //       }
    //       commonNodes.Add(point, 1);
    //   }

    //   private void CaculateNodes() {
    //       nodeXList.Clear();
    //       foreach (KeyValuePair<Vector3, int> node in commonNodes) {
    //           if (!nodeXList.ContainsKey(node.Value)) {
    //               nodeXList.Add(node.Value, new List<Vector3>());
    //           }
    //           nodeXList[node.Value].Add(node.Key);
    //       }
    //   }

    //   /// <summary>
    //   /// 获取每种岔口的坐标列表
    //   /// </summary>
    //   /// <param name="nodeType"></param>
    //   /// <returns></returns>
    //   public List<Vector3> GetNodesList(int nodeType) {
    //       CaculateNodes();
    //       if (nodeXList.ContainsKey(nodeType)) {
    //           return nodeXList[nodeType];
    //       }
    //       return null;
    //   }

    //public void AddEndPoint(EndPoint endPoint)
    //{
    //    endPointList.Add(endPoint);
    //}

    //public List<EndPoint> GetEndPoints()
    //{
    //    return endPointList;
    //}
}
