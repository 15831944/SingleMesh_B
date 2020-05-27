using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmTemp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        (AlarmManager.GetInstance() as IAlarm).IPrintAlarm();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
