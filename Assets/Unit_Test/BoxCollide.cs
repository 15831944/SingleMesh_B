using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollide : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision ctl)
    {
        ContactPoint contact = ctl.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;
        Debug.Log(pos);
    }

}
