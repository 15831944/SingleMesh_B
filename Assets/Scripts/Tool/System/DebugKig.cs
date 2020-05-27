using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DebugKig
{
    /// <summary>
    /// unity log editor
    /// </summary>
    /// <param name="msg"></param>
    public static void Log(string msg) {
# if UNITY_EDITOR
        Debug.Log(msg);
#endif
    }
    /// <summary>
    /// unity logwaring editor
    /// </summary>
    /// <param name="msg"></param>
    public static void LogWarning(string msg) {
#if UNITY_EDITOR
        Debug.LogWarning(msg);
#endif
    }
    /// <summary>
    /// unity logerror editor
    /// </summary>
    /// <param name="msg"></param>
    public static void LogError(string msg) {
#if UNITY_EDITOR
        Debug.LogError(msg);
#endif
    }
}
