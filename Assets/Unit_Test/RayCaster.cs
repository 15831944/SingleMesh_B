using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCaster : MonoBehaviour {

    private Vector3 connerPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyUp(KeyCode.R)) {
        //    RayCast();
        //}
        //Debug.DrawLine(transform.position, transform.position + transform.forward * 60, Color.red);
    }

    public void RayCast(int layerMask) {
        Ray ray = new Ray(transform.position, transform.forward * 60);

        RaycastHit[] hits = Physics.RaycastAll(ray, 60);
        if (hits != null) {
            foreach (RaycastHit hit in hits) {
                if (hit.transform.gameObject.layer == layerMask) {
                    connerPos = hit.point;
                }
            }
        }
    }

    public Vector3 GetConnerPosition(int layerMask)
    {
        RayCast(layerMask);
        return connerPos;
    }

    public void RayCast(Transform nearTransform) {
        connerPos = LianZX_JD(transform.Find("RC").position, transform.position, nearTransform.Find("LC").position, nearTransform.position);
    }

    public Vector3 GetConnerPosition(Transform nearTransform)
    {
        RayCast(nearTransform);
        return connerPos;
    }


    public Vector3 LianZX_JD(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        Vector3 Jiaod = Vector3.zero;
        float P1x = 0.0f;
        float P1y = 0.0f;
        float P1z = 0.0f;
        double plr1_x = p2.x - p1.x;
        double plr1_y = p2.y - p1.y;
        double plr1_z = p2.z - p1.z;
        double plr2_x = p4.x - p3.x;
        double plr2_y = p4.y - p3.y;
        double plr2_z = p4.z - p3.z;
        double t = 1.0f;
        if (((plr1_x != 0) && (plr2_x == 0)) || ((plr1_x == 0) && (plr2_x != 0)))
        {
            if (plr2_x == 0)
            {
                t = (p3.x - p1.x) / plr1_x;
                P1x = (float)(p1.x + t * plr1_x);
                P1y = (float)(p1.y + t * plr1_y);
                P1z = (float)(p1.z + t * plr1_z);
                Jiaod = new Vector3(P1x, P1y, P1z);
                return Jiaod;
            }
            else
            {
                t = (p1.x - p3.x) / plr2_x;
                P1x = (float)(p3.x + t * plr2_x);
                P1y = (float)(p3.y + t * plr2_y);
                P1z = (float)(p3.z + t * plr2_z);
                Jiaod = new Vector3(P1x, P1y, P1z);
                return Jiaod;
            }
        }
        else if (((plr1_y != 0) && (plr2_y == 0)) || ((plr1_y == 0) && (plr2_y != 0)))
        {
            if (plr2_y == 0)
            {
                t = (p3.y - p1.y) / plr1_y;
                P1x = (float)(p1.x + t * plr1_x);
                P1y = (float)(p1.y + t * plr1_y);
                P1z = (float)(p1.z + t * plr1_z);
                Jiaod = new Vector3(P1x, P1y, P1z);
                return Jiaod;
            }
            else
            {
                t = (p1.y - p3.y) / plr2_y;
                P1x = (float)(p3.x + t * plr2_x);
                P1y = (float)(p3.y + t * plr2_y);
                P1z = (float)(p3.z + t * plr2_z);
                Jiaod = new Vector3(P1x, P1y, P1z);
                return Jiaod;
            }
        }
        else if (((plr1_z != 0) && (plr2_z == 0)) || ((plr1_z == 0) && (plr2_z != 0)))
        {
            if (plr2_z == 0)
            {
                t = (p3.z - p1.z) / plr1_z;
                P1x = (float)(p1.x + t * plr1_x);
                P1y = (float)(p1.y + t * plr1_y);
                P1z = (float)(p1.z + t * plr1_z);
                Jiaod = new Vector3(P1x, P1y, P1z);
                return Jiaod;
            }
            else
            {
                t = (p1.z - p3.z) / plr2_z;
                P1x = (float)(p3.x + t * plr2_x);
                P1y = (float)(p3.y + t * plr2_y);
                P1z = (float)(p3.z + t * plr2_z);
                Jiaod = new Vector3(P1x, P1y, P1z);
                return Jiaod;
            }
        }
        else
        {
            if (((plr1_x != 0) && (plr2_x != 0)) && ((plr1_y != 0) && (plr2_y != 0)))
            {
                double fz = (p3.x * plr2_y - p3.y * plr2_x - plr2_y * p1.x + plr2_x * p1.y);
                double fm = (plr1_x * plr2_y - plr1_y * plr2_x);
                t = fz / fm;
                P1x = (float)(p1.x + t * plr1_x);
                P1y = (float)(p1.y + t * plr1_y);
                P1z = (float)(p1.z + t * plr1_z);
                Jiaod = new Vector3(P1x, P1y, P1z);
                return Jiaod;
            }
            else if (((plr1_x != 0) && (plr2_x != 0)) && ((plr1_z != 0) && (plr2_z != 0)))
            {
                double fz = (p3.x * plr2_z - p3.z * plr2_x - plr2_z * p1.x + plr2_x * p1.z);
                double fm = (plr1_x * plr2_z - plr1_z * plr2_x);
                t = fz / fm;
                P1x = (float)(p1.x + t * plr1_x);
                P1y = (float)(p1.y + t * plr1_y);
                P1z = (float)(p1.z + t * plr1_z);
                Jiaod = new Vector3(P1x, P1y, P1z);
                return Jiaod;
            }
            else if (((plr1_y != 0) && (plr2_y != 0)) && ((plr1_z != 0) && (plr2_z != 0)))
            {
                double fz = (p3.y * plr2_z - p3.z * plr2_y - plr2_z * p1.y + plr2_y * p1.z);
                double fm = (plr1_y * plr2_z - plr1_z * plr2_y);
                t = fz / fm;
                P1x = (float)(p1.x + t * plr1_x);
                P1y = (float)(p1.y + t * plr1_y);
                P1z = (float)(p1.z + t * plr1_z);
                Jiaod = new Vector3(P1x, P1y, P1z);
                return Jiaod;
            }
            else
            {
                return Vector3.zero;
            }

        }
        return Vector3.zero;
    }
}
