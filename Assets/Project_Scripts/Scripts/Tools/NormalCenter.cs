using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NormalCenter {

    public static Vector3 GetCenter(Transform tt)
    {
        Transform parent = tt;

        Vector3 postion = parent.position;

        Quaternion rotation = parent.rotation;

        Vector3 scale = parent.localScale;

        parent.position = Vector3.zero;

        parent.rotation = Quaternion.Euler(Vector3.zero);

        parent.localScale = Vector3.one;

        Vector3 center = Vector3.zero;

        Renderer[] renders = parent.GetComponentsInChildren<Renderer>(false);

        foreach (Renderer child in renders)
        {
            center += child.bounds.center;
        }
        if (renders.Length != 0)
        {
            center /= renders.Length;
        }

        Bounds bounds = new Bounds(center, Vector3.zero);

        foreach (Renderer child in renders)
        {

            bounds.Encapsulate(child.bounds);

        }

        parent.position = postion;

        parent.rotation = rotation;

        parent.localScale = scale;

        //foreach (Transform t in parent)
        //{

        //    t.position = t.position - bounds.center;

        //}

        //parent.transform.position = bounds.center + parent.position;
        return bounds.center + parent.position;
    }

    public static Vector3 GetCenter(Vector3[] points) {
        Vector3 targetPos = Vector3.zero;
        foreach (Vector3 pos in points) {
            targetPos += pos;
        }
        return targetPos / points.Length;
    }
    public static Vector3 GetCenter(List<Transform> points) {
        Vector3 targetPos = Vector3.zero;
        foreach (Transform pos in points)
        {
            targetPos += pos.position;
        }
        return targetPos / points.Count;
    }
}
