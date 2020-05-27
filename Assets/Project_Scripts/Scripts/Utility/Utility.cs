using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Utility
{
    /// <summary>
    /// 判断点击的位置是否在UI上
    /// </summary>
    /// <returns>true:当前点击在UI上  false:当前不点击在UI上</returns>
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
        eventDataCurrentPosition.position = Input.GetTouch(0).position;
#else
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
#endif

        List<RaycastResult> results = new List<RaycastResult>();
        if (EventSystem.current)
        {
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        }

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.layer == LayerMask.NameToLayer("UI") && Input.GetMouseButton(0))// 
            {
                Debug.Log("检测到鼠标左键");
                return true;
            }
        }

        return false;
    }
    /// <summary>
    /// 右键在UI上
    /// </summary>
    /// <returns></returns>
    public static bool IsPointerOverUIObject2()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
        eventDataCurrentPosition.position = Input.GetTouch(0).position;
#else
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
#endif

        List<RaycastResult> results = new List<RaycastResult>();
        if (EventSystem.current)
        {
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        }

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.layer == LayerMask.NameToLayer("UI") && Input.GetMouseButton(1))// 
            {
                Debug.Log("监测到鼠标中键或者右键");
                return true;
            }
        }

        return false;
    }

    public static bool IsScrollOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
        eventDataCurrentPosition.position = Input.GetTouch(0).position;
#else
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
#endif

        List<RaycastResult> results = new List<RaycastResult>();
        if (EventSystem.current)
        {
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        }

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.layer == LayerMask.NameToLayer("UI") && Input.GetAxis("Mouse ScrollWheel") != 0)//
            {
                Debug.Log("监测到鼠标中键滚动");
                return true;
            }  
        }

        return false;
    }
    public static bool IsScrollOverUIObject2()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
        eventDataCurrentPosition.position = Input.GetTouch(0).position;
#else
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
#endif

        List<RaycastResult> results = new List<RaycastResult>();
        if (EventSystem.current)
        {
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        }

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.layer == LayerMask.NameToLayer("UI") && Input.GetMouseButton(2))//
            {
                Debug.Log("监测到鼠标中键");
                return true;
            }
        }

        return false;
    }
    public static bool IsOverBrowser() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
        eventDataCurrentPosition.position = Input.GetTouch(0).position;
#else
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
#endif

        List<RaycastResult> results = new List<RaycastResult>();
        if (EventSystem.current)
        {
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        }

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.tag == "Browser")//
            {
                Debug.Log("检测到鼠标在浏览器上");
                return true;
            }
        }
        return false;
    }
}
