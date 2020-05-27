using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmManager : MonoBehaviour, IAlarm
{
    private static AlarmManager instance;
    public static AlarmManager GetInstance() {
        if (instance == null)
        {
            instance = new AlarmManager();
        }
        return instance;
    }
    void Awake() {
        instance = this;
    }
    public string IGetErrorMsg()
    {
        string msg = "获取错误信息";
        Debug.Log(msg);
        return msg;
    }

    public void IPrintAlarm()
    {
        string msg = "打印错误信息";
        Debug.Log(msg);
    }

    public void ISetErrorMsg(string msg)
    {
        Debug.Log(msg);
    }

    // Start is called before the first frame update
    void Start()
    {
        //IGetErrorMsg();
        //IPrintAlarm();
        //ISetErrorMsg("设置错误信息");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
