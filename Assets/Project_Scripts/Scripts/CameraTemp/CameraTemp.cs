using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public class CameraTemp : MonoBehaviour {
    public Transform target;
    private float sensitivityX = 1F;        //X转动增量速度  
    private float sensitivityY = 1F;        //y转动增量速度  
    private float minimumY = -90F;          //Y轴转动限制  
    private float maximumY = 90F;
    float rotationY = 0F;                   //y起始值  
    private float MovingSpeed = 1f;         //移动屏幕的速度  
    float delta_x, delta_y, delta_z;          //计算移动量  
    public float distance = 5;
    public float maxDistance;
    public float minDistance;
    public Vector2 rotate = new Vector2();
    float ZoomSpeed = 20f;                  //拉近拉远速度  
    Quaternion rotation;
    public Transform norCenterGo, topDownCenterGo;
    private Quaternion rotateQ;
    private Vector3 direction;
    [Header("Zoom")]
    public bool canZoom = true;
    public float zoomSensitive = 500f;
    public float zoomLerpTime = 0.1f;
    private void Start()
    {
        Init();
    }

    void Update()
    {
        CheckScroll();
        if (Input.GetMouseButton(1))
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
        if (Input.GetMouseButton(1))
        {//右键移动屏幕  
            delta_x = Input.GetAxis("Mouse X") * MovingSpeed;
            delta_y = Input.GetAxis("Mouse Y") * MovingSpeed;
            rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            transform.position = rotation * new Vector3(-delta_x, -delta_y, 0) + transform.position;
        }
    }
    private void Init()
    {
        rotateQ = Quaternion.Euler(rotate.y, rotate.x, 0);
        direction = rotateQ * Vector3.forward;
        transform.position = target.position - direction * distance;
        transform.LookAt(target);
    }
    private float tempDistance;
    private void CheckScroll()
    {
        tempDistance = distance;
        if (canZoom)
        {
            tempDistance -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitive;
            distance = Mathf.SmoothStep(distance, tempDistance, zoomLerpTime);
            ZoomTo(distance, 0.0f);
        }
    }

    //for zoom lerp
    private Vector3 tempPos;
    public void ZoomTo(float distance, float duration = 0.5f, Action finishFunc = null)
    {
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        rotateQ = Quaternion.Euler(rotate.y, rotate.x, 0);
        direction = rotateQ * Vector3.forward;
        tempPos = transform.position;

        tempPos = Vector3.Lerp(tempPos, target.position - direction * distance, Time.deltaTime * 3);
        transform.DOMove(tempPos, duration).OnComplete(delegate ()
        {
            this.distance = distance;
            if (finishFunc != null)
            {
                finishFunc();
            }
        });
    }
    public void ToTopDown(int index)
    {
        target = topDownCenterGo;
        transform.rotation = Quaternion.Euler(89, 0, 0);
        if (index == 1)
        {
            maxDistance = 20f;
            minDistance = 2f;
            distance = 20f;
        }
        else
        {
            maxDistance = 300f;
            minDistance = 20f;
            distance = 250f;
        }
    }
    public void ToNormal()
    {

    }
}
