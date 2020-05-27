using Loader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TManagerBase : MonoBehaviour {

    public TViewBase TView;

    // Use this for initialization
    void Start()
    {
        // this.GetComponent<Loader.ILoader>().Loaded = Loaded;
        LoadCADTemp();
    }

    // Update is called once per frame
    void Update()
    {
        //if (TView.transform.childCount > 0)
        //{
        //    NormalCenter.GetCenter(TView.transform);
        //}
    }

    DiskFile iLoader;

    private void LoadDXF(string path = null)
    {
        //try
        //{
            GetComponent<MeshGenerater.DataContainer>().ResetError();
            if (path == null) {
                iLoader = new DiskFile(content);
            }
            else{
                iLoader = new DiskFile(path);
            }
            
            DXFConvert.DXFStructure dxfStructure = new DXFConvert.DXFStructure(iLoader);
            dxfStructure.Load();
            iLoader.Dispose();
            TView.Set(dxfStructure);
            NormalCenter.GetCenter(TView.transform);
            GetComponent<MeshGenerater.DataContainer>().SerializeData();
        Camera.main.GetComponent<BLCameraControl>().LookAtResAutoDis(GameObject.Find("TViewBase").transform);
        //Debug.Log("OK:" + path);
        //}
        //catch (System.Exception ex)
        //{
        //    Debug.Log("Error:" +ex.Message);
        //}

    }

    public string fileName = "";


    public void LoadCAD(string _url) {
        //plication.streamingAssetsPath, "CAD/" + fileName + ".dxf"));
        //LoadDXF(Application.streamingAssetsPath + "/CAD/"+fileName+".dxf");
        MeshGenerater.DataContainer.GetInstance().ClearAllList();
        //StartCoroutine(loadStreamingAsset(_url));
        StartCoroutine(loadStreamingAsset(_url));
        //StartCoroutine(LoadURL(Application.streamingAssetsPath + _url));

    }
    public void LoadCADTemp()
    {
        //StartCoroutine(loadStreamingAsset(Application.streamingAssetsPath + "/BL-sl2-2.dxf"));

    }

    private string[] content;
    IEnumerator loadStreamingAsset(string _url)
    {
        //string filePath = "http://localhost/webgl/StreamingAssets/CAD/" + fileName + ".dxf";
        //string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, _url);
        string result;
        if (_url.Contains("://") || _url.Contains(":///"))
        {
            WWW www = new WWW(_url);
            yield return www;
            result = www.text;
            content = result.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            LoadDXF();
        }
        else {
            content = File.ReadAllLines(_url);
            LoadDXF();
        }
    }

    public void TestLoadCAD(string name) {
        MeshGenerater.DataContainer.GetInstance().ClearAllList();
        string url = System.IO.Path.Combine(Application.streamingAssetsPath, "CAD/"+name);
        StartCoroutine(loadStreamingAsset(url));
    }
}
