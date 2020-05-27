using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMesh : MonoBehaviour {

    Mesh mesh;
    public Transform meshTransform;
	// Use this for initialization
	void Start () {
        mesh = meshTransform.GetComponent<SkinnedMeshRenderer>().sharedMesh;
        //GetVertices();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GetVertices() {
        Vector3[] vertices =  mesh.vertices;
        Debug.Log(vertices);
    }
}
