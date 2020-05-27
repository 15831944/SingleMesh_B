using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BS.BL.Two {
    public class RayTools : MonoBehaviour
    {

        public static Transform GetAxisHD(Vector3 _target) {
            Ray newRay = new Ray(_target, Vector3.up);
            RaycastHit hit;
            if (Physics.Raycast(newRay, out hit, 3))
            {
                if (hit.transform.tag == "HD")
                {
                    return hit.transform;
                }
            }
            return null;
        }
        public static Transform GetSperaHD(Vector3 _target) {
            int radius = 3;
            Collider[] cols = Physics.OverlapSphere(_target, radius, LayerMask.NameToLayer("Default"));
            if (cols.Length > 0)
            {
                for (int i = 0; i < cols.Length; i++)
                {
                    Debug.Log("检测到物体" + cols[i].name);
                    if (cols[i].transform.tag == "HD")
                    {
                        return cols[i].transform;
                    }
                    return null;
                }
            }
            return null;
        }
        public static Transform GetAxisHD2(Vector3 _target) {
            RaycastHit hit;
            bool isHit = Physics.Raycast(_target, -Vector3.up, out hit);
            if (isHit)
            {
                if (hit.transform.tag == "HD")
                {
                    return hit.transform;
                }
            }
            return null;
        }
        public static int GetArrayIndex() {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool isCollider = Physics.Raycast(ray, out hit);
                if (isCollider)
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Array"))
                    {
                        return 1;
                    }
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Element"))
                    {
                        return 2;
                    }
                    else
                    {
                        return 3;
                    }
                }
                else
                {
                    return 3;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}

