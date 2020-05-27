using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseMove : MonoBehaviour {

    private float sensitivityX = 1F;        //X转动增量速度  
    private float sensitivityY = 1F;        //y转动增量速度  
    private float minimumY = -90F;          //Y轴转动限制  
    private float maximumY = 90F;
    float rotationY = 0F;                   //y起始值  
    private float MovingSpeed = 1f;         //移动屏幕的速度  
    float delta_x, delta_y, delta_z;          //计算移动量  
    float distance = 5;
    float ZoomSpeed = 20f;                  //拉近拉远速度  
    Quaternion rotation;

    void Update()
    {
        if (Input.GetMouseButton(1) )
        {//左键旋转屏幕  
            {
                float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
            }
        }
        if (Input.GetMouseButton(2))
        {

        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0 && !EventSystem.current.IsPointerOverGameObject())
        {//滚轴拉近拉远  
            delta_z = -Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
            transform.Translate(0, 0, -delta_z);
            distance += delta_z;
        }
        if (Input.GetMouseButton(2))
        {//滚轴中间移动屏幕  
            delta_x = Input.GetAxis("Mouse X") * MovingSpeed;
            delta_y = Input.GetAxis("Mouse Y") * MovingSpeed;
            rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            transform.position = rotation * new Vector3(-delta_x, -delta_y, 0) + transform.position;
        }
    }
}
