using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombine : MonoBehaviour {

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            CombineMesh();
        }
    }

    private void CombineMesh()
    {
        //获取自身和所有子物体中所有的 MeshRenderer 组件
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        //材质球数组
        Material[] materials = new Material[meshRenderers.Length];

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            materials[i] = meshRenderers[i].sharedMaterial;
        }

        // 合并 Mesh
        // 后去自身和子物体中所有 MsehFilter 组件
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

        CombineInstance[] combines = new CombineInstance[meshRenderers.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combines[i].mesh = meshFilters[i].sharedMesh;
            // 矩阵（Matrix）自身空间坐标的点转换成世界空间坐标的点
            combines[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        //重新生成mesh
        MeshFilter meshFilter = transform.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        // 给 MeshFilter 组件的 mesh 赋值
        meshFilter.mesh = new Mesh();
        //合并Mesh， 第二个参数 false，表示并不合并为一个网格，而是一个自网格列表
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combines, false);
        transform.gameObject.SetActive(true);

        //为合并后的新Mesh 指定材质
        //transform.GetComponent<MeshRenderer>().sharedMaterials = materials;

        MeshRenderer meshRender = transform.GetComponent<MeshRenderer>();
        if (meshRender == null)
        {
            meshRender = gameObject.AddComponent<MeshRenderer>();
        }
        meshRender.sharedMaterials = materials;
    }
}
