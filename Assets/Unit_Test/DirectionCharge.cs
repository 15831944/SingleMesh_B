using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionCharge : MonoBehaviour {

    public Transform line0, line1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.A)) {
            CaculateAngle();
        }
	}

    private void CaculateAngle() {
        Debug.Log(angle_360(line0.forward, line1.forward));
    }

    float angle_360(Vector3 from_, Vector3 to_)
    {
        Vector3 v3 = Vector3.Cross(from_, to_);
        if (v3.z > 0)
            return Vector3.Angle(from_, to_);
        else
            return 360 - Vector3.Angle(from_, to_);
    }

}
