using BS.BL.Element;
using BS.BL.Style;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BS.BL.Manager {
    public class UIManager : MonoBehaviour, ManagerBase
    {
        public void IStart()
        {
            
        }
        public Transform panelPerson;
        public ButtonGroup twoBtnGroup, sixBtnGroup, changeMode, modes, move, upDown, personGroup;

        public int nowState = 0;//当前状态 0-全部，1-人员，2-监控，3-传感器，4-设备，5-车辆
        // Start is called before the first frame update
        void Start()
        {
            InitView();
            InitListener();
        }
        /// <summary>
        /// 初始化加载ui元素
        /// </summary>
        void InitView() {
            panelPerson = GameObject.Find("Canvas").transform.Find("PersonPanel");//人员选择按钮页面
            //personGroup = panelPerson.GetComponent<ButtonGroup>();
            twoBtnGroup = GameObject.Find("Canvas").transform.Find("TwoBtnGroup").GetComponent<ButtonGroup>();//综合总览和询揽模式切换
            sixBtnGroup = GameObject.Find("Canvas").transform.Find("SixBtnGroup").GetComponent<ButtonGroup>();//资源筛选按钮
            changeMode = GameObject.Find("Canvas").transform.Find("Action/ChangeMode").GetComponent<ButtonGroup>();//2D和3D切换按钮
            modes = GameObject.Find("Canvas").transform.Find("Action/Modes").GetComponent<ButtonGroup>();//当前功能模式选择按钮
            move = GameObject.Find("Canvas").transform.Find("Action/Move").GetComponent<ButtonGroup>();//移动和放大缩小按钮选择
            upDown = GameObject.Find("Canvas").transform.Find("UpDown").GetComponent<ButtonGroup>();//地上地下切换按钮
        }
        /// <summary>
        /// 初始化按钮事件
        /// </summary>
        void InitListener() {
            //综合监控和巡览模式
            twoBtnGroup.listBtn[0].GetComponent<Button>().onClick.AddListener(delegate () { SelectZHZLOrXLMS(0); });
            twoBtnGroup.listBtn[1].GetComponent<Button>().onClick.AddListener(delegate () { SelectZHZLOrXLMS(1); });
            //资源筛选按钮
            sixBtnGroup.listBtn[0].GetComponent<Button>().onClick.AddListener(delegate () { SelectResType(0); });
            sixBtnGroup.listBtn[1].GetComponent<Button>().onClick.AddListener(delegate () { SelectResType(1); });
            sixBtnGroup.listBtn[2].GetComponent<Button>().onClick.AddListener(delegate () { SelectResType(2); });
            sixBtnGroup.listBtn[3].GetComponent<Button>().onClick.AddListener(delegate () { SelectResType(3); });
            sixBtnGroup.listBtn[4].GetComponent<Button>().onClick.AddListener(delegate () { SelectResType(4); });
            sixBtnGroup.listBtn[5].GetComponent<Button>().onClick.AddListener(delegate () { SelectResType(5); });
            //地面，地下切换按钮
            upDown.listBtn[0].GetComponent<Button>().onClick.AddListener(delegate () { SelectUpGroundOrDownGround(0); });
            upDown.listBtn[1].GetComponent<Button>().onClick.AddListener(delegate () { SelectUpGroundOrDownGround(1); });
            //2D和3D模式切换
            changeMode.listBtn[0].GetComponent<Button>().onClick.AddListener(delegate () { ChangeModesByType(0); });
            changeMode.listBtn[1].GetComponent<Button>().onClick.AddListener(delegate () { ChangeModesByType(1); });
            //当前功能模式选择按钮
            modes.listBtn[0].GetComponent<Button>().onClick.AddListener(delegate () { ChangeModeByClick(0); });
            modes.listBtn[1].GetComponent<Button>().onClick.AddListener(delegate () { ChangeModeByClick(1); });
            modes.listBtn[2].GetComponent<Button>().onClick.AddListener(delegate () { ChangeModeByClick(2); });
            //移动和放大缩小按钮选择
            move.listBtn[0].GetComponent<Button>().onClick.AddListener(delegate () { ChangeMoveType(0); });
            move.listBtn[1].GetComponent<Button>().onClick.AddListener(delegate () { ChangeMoveType(1); });
            move.listBtn[2].GetComponent<Button>().onClick.AddListener(delegate () { ChangeMoveType(2); });
            //人员分角色
            personGroup.listBtn[0].GetComponent<Button>().onClick.AddListener(delegate () { ChangePersonByHole(0); });
            personGroup.listBtn[1].GetComponent<Button>().onClick.AddListener(delegate () { ChangePersonByHole(1); });
            personGroup.listBtn[2].GetComponent<Button>().onClick.AddListener(delegate () { ChangePersonByHole(2); });
            personGroup.listBtn[3].GetComponent<Button>().onClick.AddListener(delegate () { ChangePersonByHole(3); });
        }
        #region 综合总览和巡览模式事件
        /// <summary>
        /// 综合总览和巡览模式切换事件
        /// </summary>
        /// <param name="num">0-综合总览，1-询揽模式</param>
        public void SelectZHZLOrXLMS(int num) {
            switch (num)
            {
                case 0://综合总览
                    Debug.Log("进入综合总览模式");
                    break;
                case 1://巡览模式
                    Debug.Log("进入巡览模式");
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 资源切换按钮事件
        /// <summary>
        /// 资源切换按钮事件
        /// </summary>
        /// <param name="type">0-全部，1-人员，2-监控，3-传感器，4-设备，5-车辆</param>
        public void SelectResType(int type)
        {
            nowState = type;
            move.ResetButtonStyle();
            panelPerson.gameObject.SetActive(false);
            Camera.main.GetComponent<BLCameraControl>().LookAtResAutoDis(GameObject.Find("TViewBase").transform);
            switch (type)
            {
                case 0:
                    Debug.Log("选择全部资源按钮");
                    foreach (KeyValuePair<ElementType,List<ElementItem>> item in ElementContainer.GetInstance().dicElements)
                    {
                        foreach (ElementItem itemElement in item.Value)
                        {
                            itemElement.gameObject.SetActive(true);
                        }
                    }
                    break;
                case 1:
                    Debug.Log("选择人员按钮");
                    panelPerson.gameObject.SetActive(true);
                    ElementContainer.GetInstance().SetElementItemVisible(ElementType.PERSON);
                    break;
                case 2:
                    Debug.Log("选择监控按钮");
                    ElementContainer.GetInstance().SetElementItemVisible(ElementType.JK);
                    break;
                case 3:
                    Debug.Log("选择传感器按钮");
                    ElementContainer.GetInstance().SetElementItemVisible(ElementType.CGQ);
                    break;
                case 4:
                    Debug.Log("选择设备按钮");
                    ElementContainer.GetInstance().SetElementItemVisible(ElementType.SB);
                    break;
                case 5:
                    Debug.Log("选择车辆按钮");
                    ElementContainer.GetInstance().SetElementItemVisible(ElementType.CAR);
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 地面和地下按钮
        public void SelectUpGroundOrDownGround(int num) {
            switch (num)
            {
                case 0:
                    Debug.Log("显示地面");
                    MainManager.GetInstance().overground.SetActive(true);
                    Camera.main.GetComponent<BLCameraControl>().LookAtResAutoDis(MainManager.GetInstance().overground.transform);
                    break;
                case 1:
                    Debug.Log("显示地下");
                    MainManager.GetInstance().overground.SetActive(false);
                    Camera.main.GetComponent<BLCameraControl>().LookAtResAutoDis(GameObject.Find("TViewBase").transform);
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 2D和3D功能之间的切换
        public void ChangeModesByType(int num) {
            switch (num)
            {
                case 0:
                    Debug.Log("2D");
                    break;
                case 1:
                    Debug.Log("3D");
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 当前功能模式选择按钮
        public void ChangeModeByClick(int num)
        {
            switch (num)
            {
                case 0:
                    Debug.Log("掘进");
                    break;
                case 1:
                    Debug.Log("区域");
                    break;
                case 2:
                    Debug.Log("路线");
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 移动和放大缩小按钮选择
        public void ChangeMoveType(int num) {
            switch (num)
            {
                case 0:
                    Debug.Log("缩小");
                    break;
                case 1:
                    Debug.Log("放大");
                    break;
                case 2:
                    Debug.Log("移动");
                    break;
                default:
                    break;
            }
        }
        private float waitTime = 0f;
        private void UpdateMoveCore() {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                waitTime = 0;
                move.listBtn[0].GetComponent<ButtonStyle>().OnClickEvent();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                waitTime = 0;
                move.listBtn[1].GetComponent<ButtonStyle>().OnClickEvent();
            }
            else
            {
                waitTime += Time.deltaTime;
                if (waitTime > 0.2f)
                {
                    move.ResetButtonStyle();
                    waitTime = 0;
                }
            }
            if (Input.GetMouseButton(2))
            {
                move.listBtn[2].GetComponent<ButtonStyle>().OnClickEvent();
            }
        }
        #endregion
        #region 人员根据不同角色划分
        public void ChangePersonByHole(int num) {
            ElementContainer.GetInstance().SetPersonItemVisible(num - 1);
            switch (num)
            {
                case 0:
                    Debug.Log("全部");
                    personGroup.listBtn[0].transform.DOLocalMoveY(4, 0.5f);
                    personGroup.listBtn[1].transform.DOLocalMoveY(-6, 0.5f);
                    personGroup.listBtn[2].transform.DOLocalMoveY(-6, 0.5f);
                    personGroup.listBtn[3].transform.DOLocalMoveY(-6, 0.5f);
                    break;
                case 1:
                    Debug.Log("矿领导");
                    personGroup.listBtn[1].transform.DOLocalMoveY(4, 0.5f);
                    personGroup.listBtn[0].transform.DOLocalMoveY(-6, 0.5f);
                    personGroup.listBtn[2].transform.DOLocalMoveY(-6, 0.5f);
                    personGroup.listBtn[3].transform.DOLocalMoveY(-6, 0.5f);
                    break;
                case 2:
                    Debug.Log("工人");
                    personGroup.listBtn[2].transform.DOLocalMoveY(4, 0.5f);
                    personGroup.listBtn[1].transform.DOLocalMoveY(-6, 0.5f);
                    personGroup.listBtn[0].transform.DOLocalMoveY(-6, 0.5f);
                    personGroup.listBtn[3].transform.DOLocalMoveY(-6, 0.5f);
                    break;
                case 3:
                    Debug.Log("访客");
                    personGroup.listBtn[3].transform.DOLocalMoveY(4, 0.5f);
                    personGroup.listBtn[1].transform.DOLocalMoveY(-6, 0.5f);
                    personGroup.listBtn[2].transform.DOLocalMoveY(-6, 0.5f);
                    personGroup.listBtn[0].transform.DOLocalMoveY(-6, 0.5f);
                    break;
                default:
                    break;
            }
        }
        #endregion

        // Update is called once per frame
        void Update()
        {
            UpdateMoveCore();
        }
    }
}

