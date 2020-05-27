using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualMesh : MonoBehaviour {

    private Mesh ownMesh;
    public Vector3[] vertext;

	// Use this for initialization
	void Start () {
        ownMesh = GetComponent<MeshFilter>().mesh;
        ownMesh.vertices = vertext;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
