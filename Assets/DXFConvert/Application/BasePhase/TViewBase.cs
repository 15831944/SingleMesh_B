using DXFConvert;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TViewBase : MonoBehaviour {

    public static TViewBase Content { get; set; }

    //图层对象
    public GameObject LayerGO;

    //视图缩放级别，用来控制线条显示粗细
    //public float Zoom;

    //用于显示的相机
    //public Camera camera;

    //图层列表，用于其他元检索
    public Dictionary<string, TLayerBase> Layers;

    void Awake()
    {
        Content = this;
        Layers = new Dictionary<string, TLayerBase>();

        //if (camera == null)
        //    camera = Camera.main;
    }

    //private bool isUdelta = false;

    // Update is called once per frame
    void Update()
    {

        ////鼠标右键移动
        //if (Input.GetMouseButton(2) && isUdelta == false)
        //{
        //    camera.transform.Translate(camera.transform.right * -Input.GetAxis("Mouse X") * (camera.orthographicSize / 2) * 0.2f, Space.World);
        //    camera.transform.Translate(camera.transform.up * -Input.GetAxis("Mouse Y") * (camera.orthographicSize / 2) * 0.2f, Space.World);
        //}
    }


    /// <summary>
    /// 可以调整的对象集合
    /// </summary>
    public List<IResizeObject> ResizeObjects = new List<IResizeObject>();

    public void Set(DXFStructure dxf)
    {
        //先初始化图层
        foreach (TABLE table in dxf.TABLES.TABLEList)
        {
            if (table.LAYERList.Count == 0) continue;

            foreach (LAYER item in table.LAYERList)
            {
                if (item.C2 != null && item.C2 == "HD")
                {
                    GameObject go = Instantiate(LayerGO) as GameObject;
                    go.transform.parent = gameObject.transform;
                    var l = go.GetComponent<TLayerBase>();
                    l.Set(item);
                    Layers.Add(item.C2, l);
                }
            }
        }

        //构建一个默认图层，没有图层属性的对象放在此图层
        //GameObject goDefaultLayer = Instantiate(LayerGO) as GameObject;
        //TLayerBase.DefaultLayer = goDefaultLayer.GetComponent<TLayerBase>();

        //绘制各图层下的元素
        foreach (var item in Layers)
        {
            item.Value.Load(dxf);
        }
        Layers.Clear();
        //初始化优化代码
        ResizeObjects.ForEach((l) =>
        {
            l.ToMin();
            if (l.gameObject.activeSelf)
                l.SetSetWidth();
        });
    }

    //获得图层材质
    public Material GetLayerMaterial(string name)
    {
        if (Layers.ContainsKey(name))
            return Layers[name].LayerMaterial;
        else
        {
            Debug.LogError("GetLayerMaterial_No Layer_" + name);
            return null;
        }
    }


    //获得图层材质
    public TLayerBase GetLayer(string name)
    {
        if (Layers.ContainsKey(name))
            return Layers[name];
        else
            return TLayerBase.DefaultLayer;
    }
}
