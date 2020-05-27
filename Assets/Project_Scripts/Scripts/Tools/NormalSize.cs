using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NormalSize {

    public static Vector3 GetSize(Transform tt)
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

        center /= parent.GetComponentsInChildren<Renderer>().Length;

        Bounds bounds = new Bounds(center, Vector3.zero);

        foreach (Renderer child in renders)
        {

            bounds.Encapsulate(child.bounds);

        }
        parent.position = postion;

        parent.rotation = rotation;

        parent.localScale = scale;

        return bounds.size;
    }
    public static Vector3 GetListSize(List<Transform> tts)
    {
        Vector3 result = Vector3.zero;
        foreach (Transform tt in tts)
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

            center /= parent.GetComponentsInChildren<Renderer>().Length;

            Bounds bounds = new Bounds(center, Vector3.zero);

            foreach (Renderer child in renders)
            {

                bounds.Encapsulate(child.bounds);

            }
            parent.position = postion;

            parent.rotation = rotation;

            parent.localScale = scale;
            result += bounds.size;
        }
        return result / tts.Count;
    }
}
