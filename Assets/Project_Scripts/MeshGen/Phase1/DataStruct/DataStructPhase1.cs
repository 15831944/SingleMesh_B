using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LineData {
    public string id;
    public string sPointId;
    public string ePointId;
 //   public float width = 1;
 //   public float height = 2;
	//public int type = 1;
 //   public string areaId = "1";
 //   public string bottomMatId = "001";
 //   public string sideMatId = "002";
	//public string topMatId="003";
    public LineData(Line line) {
        id = line.id;
        sPointId = line.startPoint.id;
        ePointId = line.endPoint.id;
    }
}

[Serializable]
public class NodeData {
    public string id;
    public string[] pointIds;
    //public string areaId = "1";
    //public string bottomMatId = "001";
    //public string sideMatId = "002";
    //public string topMatId = "003";

    public NodeData(Node node) {
        pointIds = new string[node.pointList.Count];
        for (var index = 0; index < pointIds.Length; index++) {
            pointIds[index] = node.pointList[index].id;
            id = node.id;
        }
    }
}

[Serializable]
public class PointData {
    public string id;
    public float posX;
    public float posY;
    public float posZ;

    public PointData(Point point) {
        id = point.id;
        posX = point.pos.x;
        posY = point.pos.y;
        posZ = point.pos.z;
    }
}

[Serializable]
public class DataMap {
    public NodeData[] nodes;
    public LineData[] lines;
    public PointData[] points;

    public DataMap(List<Node> nodeList,List<Line> lineList) {
        nodes = new NodeData[nodeList.Count];
        lines = new LineData[lineList.Count];
        points = new PointData[lineList.Count * 2];

        for (var index = 0; index < lineList.Count; index++) {
            points[index * 2] = new PointData(lineList[index].startPoint);
            points[index * 2 + 1] = new PointData(lineList[index].endPoint);
        }

        for (var index = 0; index < nodes.Length; index++) {
            nodes[index] = new NodeData(nodeList[index]);
        }

        for (var index = 0; index < lines.Length; index++)
        {
            lines[index] = new LineData(lineList[index]);
        }
    }
}
