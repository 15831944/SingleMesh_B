using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTarget : MonoBehaviour {

    private Transform target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null) {
            transform.position = target.position;
        }
	}

    public void SetTarget(Transform target) {
        this.target = target;
    }
}
