using BS.BL.Interface;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 绘制多边形
/// TODO: 存在两点重合时绘制有问题
/// </summary>
public class PolygonDrawer : MonoBehaviour
{
    public Material material;
    public Transform[] vertices;
    public MeshRenderer mRenderer;
    public MeshFilter mFilter;
    public string areaName;
    public List<Transform> listLines = new List<Transform>(); 

    public void Init(List<Transform> listVertices,Color color,string _areaName) {
        vertices = listVertices.ToArray();
        //material.color = color;
        material.SetColor("_FrontColor", color);
        material.SetColor("_BackColor", color);
        areaName = _areaName;
        Draw();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //点击鼠标右键
        {
            object ray = Camera.main.ScreenPointToRay(Input.mousePosition); //屏幕坐标转射线
            RaycastHit hit;                                                     //射线对象是：结构体类型（存储了相关信息）
            if (Physics.Raycast((Ray)ray, out hit))
            {
                if (hit.transform.name == "AreaSelect")
                {
                    Debug.Log("检测到的物体：" + hit.transform.name);
                    GameObject.Find("JSInterface").GetComponent<JSInterface>().selectCurrentArea(areaName);
                }
            }
        }
    }
    public void Draw()
    {
        Vector2[] vertices2D = new Vector2[vertices.Length];
        Vector3[] vertices3D = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertice = vertices[i].localPosition;
            vertices2D[i] = new Vector2(vertice.x, vertice.y);
            vertices3D[i] = vertice;
        }

        Triangulator tr = new Triangulator(vertices2D);
        int[] triangles = tr.Triangulate();

        Mesh mesh = new Mesh();
        mesh.vertices = vertices3D;
        mesh.triangles = triangles;

        if (mRenderer == null)
        {
            mRenderer = gameObject.GetOrAddComponent<MeshRenderer>();
        }
        mRenderer.material = material;
        if (mFilter == null)
        {
            mFilter = gameObject.GetOrAddComponent<MeshFilter>();
        }
        mFilter.mesh = mesh;
    }
}