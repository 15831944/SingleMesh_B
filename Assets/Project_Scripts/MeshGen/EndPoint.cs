using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MeshGenerater.DataContainer.GetInstance().AddEndPoint(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector3 GetPosition() {
        return transform.position;
    }
}
