using Loader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TManager : MonoBehaviour {

    public TView TView;

    // Use this for initialization
    void Start()
    {
        // this.GetComponent<Loader.ILoader>().Loaded = Loaded;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LoadDXF(string path)
    {
        try
        {
            DiskFile iLoader = new DiskFile(path);
            DXFConvert.DXFStructure dxfStructure = new DXFConvert.DXFStructure(iLoader);
            dxfStructure.Load();
            iLoader.Dispose();
            TView.Set(dxfStructure);
            Debug.Log("OK:" + path);
        }
        catch (System.Exception ex)
        {
            Debug.Log("Error:" + path);
            Debug.LogError(ex.ToString());
        }

    }

    public string fileName = "";


    public void LoadCAD() {
        LoadDXF(Application.streamingAssetsPath + "/CAD/"+fileName+".dxf");
    }
}
