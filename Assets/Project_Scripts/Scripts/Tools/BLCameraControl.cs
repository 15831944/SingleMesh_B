using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;
using TFramework.EventSystem;
using TFramework.ApplicationLevel;

public class BLCameraControl : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Transform norCenterGo, topDownCenterGo;
    public bool isTopDown = false;
    [Header("Rotate")]
    public Vector2 rotate;
    public float manualRotateSpeed = 100f;
    public bool ManualRotate = true;
    public float ManualRotateLerpTime = 0.1f;
    public bool AutoRotate = false;
    public float autoRotateSpeed = 10f;
    public float rotateLimiteMinY = -10f;
    public float rotateLimiteMaxY = 90f;
    [Header("Distance")]
    public float distance;
    public float minDistance = 20, maxDistance = 200;
    public float autoSizeScale = 0.1f;
    [Header("Zoom")]
    public bool canZoom = true;
    public float zoomSensitive = 100f;
    public float zoomLerpTime = 0.1f;

    private Quaternion rotateQ;
    private Vector3 direction;

    [Header("Move")]
    public float moveSpeed;
    public Vector2 moveSpeedGap = new Vector2(0.1f,10f);
    public bool isMove = true;
    /// <summary>
    /// 是否有变换操作正在执行
    /// </summary>
    private bool isTransaction = false;
    private bool isPause = false;

    public float lookDistance = 5f;

    private bool isLookRes = false;

    // Use this for initialization
    void Start()
    {
        Init();
        //TEventSystem.Instance.EventManager.addEventListener(TEventType.CanRotate, CanRotate);
        //RecordCenterInfo();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            canZoom = true;
        }
        if (!Utility.IsPointerOverUIObject())
        {
            target.transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);
            if (IsMoveCamera())
            {
                MoveCamera();
            }
            else
            {
                if (isPause)
                {
                    return;
                }
                if (isTransaction)
                {
                    return;
                }
                else
                {
                    if (!isTopDown && !Utility.IsPointerOverUIObject2() && !Utility.IsScrollOverUIObject() && !Utility.IsScrollOverUIObject2())
                    {
                        ManualRotation();
                        AutoRotation();
                    }
                    if (!Utility.IsScrollOverUIObject()) {
                        CheckScroll();
                        transform.LookAt(target);
                    }
                }
            }
        }
        //if (!IsMoveCamera() && !isPause && isTransaction && !isTopDown)
        //{
        //    ManualRotation();
        //                AutoRotation();
        //}
    }

    private void CanRotate(TEvent tEvent)
    {
        ManualRotate = bool.Parse(tEvent.eventParams[0].ToString());
        if (!ManualRotate) {
            AutoRotate = false;
        }
    }

    public void IsPause(bool isPause)
    {
        this.isPause = isPause;
    }

    private void Init()
    {
        rotateQ = Quaternion.Euler(rotate.y, rotate.x, 0);
        direction = rotateQ * Vector3.forward;
        transform.position = target.position - direction * distance;
        transform.LookAt(target);
    }

    /// <summary>
    /// 自转
    /// </summary>
    private void AutoRotation()
    {
        if (AutoRotate)
        {
            rotate.x -= autoRotateSpeed * Time.deltaTime;
            rotateQ = Quaternion.Euler(rotate.y, rotate.x, 0);
            direction = rotateQ * Vector3.forward;
            transform.position = Vector3.Lerp(transform.position, target.position - direction * distance, ManualRotateLerpTime);
        }
    }

    /// <summary>
    /// 手动旋转监测
    /// </summary>
    private void ManualRotation()
    {
        if (ManualRotate && Input.GetMouseButton(1) && rotate.y > rotateLimiteMinY && rotate.y < rotateLimiteMaxY)
        {
            rotate.x += Input.GetAxis("Mouse X") * manualRotateSpeed * Time.deltaTime;
            rotate.y -= Input.GetAxis("Mouse Y") * manualRotateSpeed * Time.deltaTime;
            rotateQ = Quaternion.Euler(rotate.y, rotate.x, 0);
            direction = rotateQ * Vector3.forward;
            transform.position = Vector3.Lerp(transform.position, target.position - direction * distance, ManualRotateLerpTime);
            //transform.position = target.position - direction * distance;
        }
        if (rotate.y <= rotateLimiteMinY)
        {
            rotate.y = rotateLimiteMinY + 1;
        }
        if (rotate.y >= rotateLimiteMaxY)
        {
            rotate.y = rotateLimiteMaxY - 1;
        }
    }

    private float tempDistance;

    private void CheckScroll()
    {
        tempDistance = distance;
        if (canZoom)
        {
            //zoomSensitive = 1000f / (maxDistance - minDistance) * (distance - minDistance);
            zoomSensitive = distance;
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

    public void MoveTo(float distance, float rotateY, float duration = 0.5f, Action finishFunc = null)
    {
        MoveTo(distance, transform.rotation.eulerAngles.x, rotateY, duration, finishFunc);
    }

    public void MoveTo(Vector3 targetPos, Vector3 targetRot, float duration = 0.5f, Action finishFunc = null)
    {
        MoveTo(Vector3.Distance(target.position, targetPos), targetRot.x, targetRot.y, duration, finishFunc);
    }

    public void MoveTo(float distance, float rotateX, float rotateY, float duration = 0.5f, Action finishFunc = null)
    {
        if (target != null)
        {
            isTransaction = true;
            rotateQ = Quaternion.Euler(rotateY, rotateX, 0);
            direction = rotateQ * Vector3.forward;
            transform.DOMove(target.position - direction * distance, duration).OnComplete(delegate ()
            {
                isTransaction = false;
                this.distance = distance;
                rotate.y = rotateY;
                rotate.x = rotateX;
                if (finishFunc != null)
                {
                    finishFunc();
                }
            }).OnUpdate(delegate () {
                transform.LookAt(target);
            });
        }
    }

    public void SetTarget(Transform target, float duration = 0.5f)
    {
        bool isAutoRotate = AutoRotate;
        this.target = target;
        isTransaction = true;
        AutoRotate = false;
        transform.DORotateQuaternion(Quaternion.LookRotation(target.position - transform.position), duration).SetEase(Ease.OutExpo).OnComplete(delegate () {
            isTransaction = false;
            AutoRotate = isAutoRotate;
            //transform.LookAt(target);
        });
    }

    public void SetTarget(Transform target, float distance, float rotateY, float duration = 0.5f)
    {
        bool isAutoRotate = AutoRotate;
        this.target = target;
        isTransaction = true;
        AutoRotate = false;
        transform.DORotateQuaternion(Quaternion.LookRotation(target.position - transform.position), duration).OnComplete(delegate () {
            isTransaction = false;
            AutoRotate = isAutoRotate;
            transform.LookAt(target);
            this.distance = distance;
            rotate.y = rotateY;
        });
    }

    public void SetZoomLimit(float minDistance, float maxDistance)
    {
        this.minDistance = minDistance;
        this.maxDistance = maxDistance;
    }

    public void AllView()
    {
        LookAtAreaAutoDis(GameObject.Find("ModelRoot").transform);
        ChangeMat(GameObject.Find("ModelRoot").transform, true);
    }

    public void SelectResNear(string resId) {
    }

    public void SelectRes(string resId)
    {
    }

    public void ResDetail()
    {
    }

    public void SelectArea(string areaId)
    {
        //LookAtAreaAutoDis(GameObject.Find("GenManager").GetComponent<ModelGenerater>().areaTransforms[areaId]);
        //if (areaId != "1") {
        //    ChangeMat(GameObject.Find("GenManager").GetComponent<ModelGenerater>().areaTransforms[areaId], true);
        //}
    }

    public Material selectMat;
    public MeshRenderer[] meshMats;
    private void ChangeMat(Transform parentTrans,bool isSelect) {
        if (parentTrans == null) {
            return;
        }
        meshMats = parentTrans.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in meshMats) {
            mr.material = selectMat;
        }
    }

    /// <summary>
    /// 显示地上区域
    /// </summary>
    /// <param name="isShow"></param>
    public void ShowUpground(bool isShow)
    {
        
        if (isShow)
        {
            LookAtUpground(GameObject.Find("OverGroundRoot").transform, 30f);
        }
        else
        {
            //if (levelChange.levelName != "ApplicationDC")
            //{
            //    LookAtAreaAutoDis(SceneController.GetInstance().perSelectAreaTransform);
            //}
            //else
            //{
            //    if (GameObject.Find("3DPoint"))
            //    {
            //        LookAtUpground2(GameObject.Find("3DPoint").transform, 90f, 89f,200);
            //    }
            //    else
            //    {
            //        LookAtUpground2(new GameObject().transform, 90f, 89f);
            //    }
            //}
        }
    }

    public void LookAtUpground(Transform target, float rotateY)
    {
        Vector3 targetSize = NormalSize.GetSize(target);
        Vector3 targetCenter = NormalCenter.GetCenter(target);
        norCenterGo.GetComponent<NormalTarget>().SetTarget(null);
        norCenterGo.position = targetCenter;
        float norDis = (targetSize.x + targetSize.z) / 2f * autoSizeScale + 100f;
        SetZoomLimit(0, 2500f);
        MoveTo(norDis, rotateY);
    }
    public void LookAtUpground2(Transform target, float rotateX, float rotateY, float minZoom = 0f)
    {
        Vector3 targetSize = NormalSize.GetSize(target);
        Vector3 targetCenter = NormalCenter.GetCenter(target);
        norCenterGo.GetComponent<NormalTarget>().SetTarget(null);
        norCenterGo.position = targetCenter;
        float norDis = (targetSize.x + targetSize.z) / 2f * autoSizeScale + 300f;
        SetZoomLimit(minZoom, 2000);
        MoveTo(norDis, rotateX, rotateY, 0.5f);
    }


    public void LookAtAreaAutoDis(Transform target)
    {
        if (target == null)
        {
            target = GameObject.Find("ModelRoot").transform;
        }
        Vector3 targetSize = NormalSize.GetSize(target);
        Vector3 targetCenter = NormalCenter.GetCenter(target);
        norCenterGo.GetComponent<NormalTarget>().SetTarget(null);
        norCenterGo.position = targetCenter;
        float norDis = (targetSize.x + targetSize.z) / 2f * autoSizeScale;
        SetZoomLimit(0, 2000);
        MoveTo(norDis, 30f);
    }

    public void LookAtResAutoDis(Transform target)
    {
        Vector3 targetSize = NormalSize.GetSize(target);
        Vector3 targetCenter = NormalCenter.GetCenter(target);
        norCenterGo.position = targetCenter;
        float norDis = (targetSize.x + targetSize.z) / 2f + 150f;
        SetZoomLimit(0, 2500);
        MoveTo(norDis, 30f);
        canZoom = true;
        isMove = true;
    }
    public void LookAtUpGround3(Transform target)
    {
        Vector3 targetSize = NormalSize.GetSize(target);
        Vector3 targetCenter = NormalCenter.GetCenter(target);
        norCenterGo.position = targetCenter;
        float norDis = (targetSize.x + targetSize.z) / 2f + 300f;
        SetZoomLimit(0, 2500);
        MoveTo(norDis, 30f);
        canZoom = true;
        isMove = true;
    }
    public void LookAtResAutoDisS(Transform target)
    {
        Vector3 targetSize = NormalSize.GetSize(target);
        Vector3 targetCenter = NormalCenter.GetCenter(target);
        norCenterGo.position = targetCenter;
        float norDis = (targetSize.x + targetSize.z) / 2f + 150f;
        SetZoomLimit(0, 2000);
        MoveTo(norDis, 90f);
        canZoom = false;
        isMove = false;
    }
    public void LookAtPosition(Vector3 targetPos) {
        norCenterGo.position = targetPos;
        float norDis = 320f;
        SetZoomLimit(0, 2000);
        MoveTo(norDis, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.x, 0.5f);
    }
    public void LookAtPosition2(Vector3 targetPos, float norDis = 350F)
    {
        isLookRes = true;
        norCenterGo.position = targetPos;
        SetZoomLimit(0, 1000);
        MoveTo(norDis, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.x, 0.5f);
    }
    public void LookAtPosition3(Vector3 targetPos)
    {
        isLookRes = true;
        norCenterGo.position = targetPos;
        float norDis = 1000f;
        SetZoomLimit(0, 2500);
        MoveTo(norDis, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.x, 0.5f);
    }
    public void LookAtResAutoDis2(Transform target)
    {
        Vector3 targetSize = NormalSize.GetSize(target);
        Vector3 targetCenter = NormalCenter.GetCenter(target);
        norCenterGo.position = targetCenter;
        float norDis = (targetSize.x + targetSize.z) + lookDistance;
        SetZoomLimit(0, 2000);
        MoveTo(norDis * 2, 30f);
    }
    public void LookAtResAutoDis3(List<Transform> targets)
    {
        Vector3 targetSize = Vector3.zero;
        for (int i = 0; i < targets.Count; i++)
        {
            targetSize += NormalSize.GetSize(targets[i]) * targets[i].localScale.z;
        }
        targetSize = targetSize / targets.Count;
        Vector3 targetCenter = NormalCenter.GetCenter(targets);
        norCenterGo.position = targetCenter;
        float norDis = (targetSize.x + targetSize.z)/2 + lookDistance;
        SetZoomLimit(0, 2000);
        MoveTo(norDis * 2, 30f);
    }

    public void LookAtArea(List<Transform> targets) {
        Vector3 targetSize =Vector3.zero;
        Vector3 targetCenter = Vector3.zero;
        for (int i = 0; i < targets.Count; i++)
        {
            targetSize += NormalSize.GetSize(targets[i]);
            targetCenter += NormalCenter.GetCenter(targets[i]);
        }
        targetSize = targetSize / targets.Count;
        targetCenter = targetCenter / targets.Count;
        norCenterGo.position = targetCenter;
        float norDis = (targetSize.x + targetSize.z) + 100;
        SetZoomLimit(0, 2000);
        MoveTo(norDis * 2, 30f);
    }

    void MoveCamera()
    {
        if (Input.GetMouseButton(2) && !Utility.IsScrollOverUIObject2())
        {//右键移动屏幕  
            moveSpeed = distance / 1000 * (moveSpeedGap.y - moveSpeedGap.x);
            float delta_x = Input.GetAxis("Mouse X") * moveSpeed;
            float delta_y = Input.GetAxis("Mouse Y") * moveSpeed;
            target.localPosition += target.right * -delta_x + target.forward * -delta_y;
        }
        transform.position = target.position - offer;
    }
    Vector3 offer = new Vector3();
    bool IsMoveCamera()
    {
        if (Input.GetMouseButton(2))
        {
            offer = target.position - transform.position;
            isMove = true;
        }
        else
        {
            isMove = false;
        }
        return isMove;
    }
}

