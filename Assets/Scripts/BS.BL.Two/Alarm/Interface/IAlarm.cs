using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAlarm 
{
    void IPrintAlarm();
    string IGetErrorMsg();
    void ISetErrorMsg(string msg);
}
