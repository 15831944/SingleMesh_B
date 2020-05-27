using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(transform.childCount - 1).Find("Line").gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
