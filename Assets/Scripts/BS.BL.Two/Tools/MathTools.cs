using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathTools
{
    public static float pointToLine(Vector3 v1, Vector3 v2, Vector3 point) {
        float xDis = v2.x - v1.x;
        float yDis = v2.y - v1.y;
        float zDis = v2.z - v1.z;
        float dx = point.x - v1.x;
        float dy = point.y - v1.y;
        float dz = point.z - v1.z;
        float d = xDis * xDis + yDis * yDis + zDis * zDis;
        float t = xDis * dx + yDis * dy + zDis * dz;//向量点积

        if (d > 0)
        {
            t /= d;
        }

        if (t < 0)
        {
            t = 0;
        }
        else if (t > 1)
        {
            t = 1;
        }
        dx = v1.x + t * xDis - point.x;
        dy = v1.y + t * yDis - point.y;
        dz = v1.z + t * zDis - point.z;
        return Mathf.Sqrt(dx * dx + dy * dy + dz * dz);
    }
    /// <summary>
    /// 获取某向量的垂直向量
    /// </summary>
    /// <param name="_dir"></param>
    /// <returns></returns>
    public static Vector3 GetVerticalDir(Vector3 _dir)
    {
        //（_dir.x,_dir.z）与（？，1）垂直，则_dir.x * ？ + _dir.z * 1 = 0
        if (_dir.z == 0)
        {
            return new Vector3(0, 0, -1);
        }
        else
        {
            return new Vector3(-_dir.z / _dir.x, 0, 1).normalized;
        }
    }
    /// <summary>
    /// 通过向量获取两个向量的夹角
    /// </summary>
    /// <param name="startPos">原点位置</param>
    /// <param name="vec1">第一个点的位置</param>
    /// <param name="vec2">第二个点的位置</param>
    /// <returns></returns>
    public static float GetAxis(Vector3 startPos, Vector3 vec1,Vector3 vec2) {
        Vector3 axis1 = vec1 - startPos;
        Vector3 axis2 = vec2 - startPos;
        float _axis = Vector3.Angle(axis1, axis2);
        return _axis;
    }
}
