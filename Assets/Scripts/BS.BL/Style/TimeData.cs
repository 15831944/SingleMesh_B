using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeData : MonoBehaviour
{
    private Text textDay, textTime;
    // Start is called before the first frame update
    void Start()
    {
        textTime = transform.Find("Time").GetComponent<Text>();
        textDay = transform.Find("Day").GetComponent<Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        textDay.text = System.DateTime.Now.ToString("MM/dd yyyy");
        textTime.text = System.DateTime.Now.ToString("HH:mm:ss");
    }
}
