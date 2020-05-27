using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

namespace BS.BL.Element {
    public class ElementItem : MonoBehaviour
    {
        public GameObject first, second;
        private Button firstBtn;
        public GameObject secondPanel, secondLine;
        public List<Sprite> tuo, icon;
        public ResEntity resEntitySelf;
        public Transform resPointTrans, resApTrans;
        public Text text1, text2, text3;
        public Image photo;

        private Vector3 targetPos;
        public ElementType elementTypeSelf;

        public Sprite personSpriteTemp, sbSpriteTemp;
        public GameObject dynamicResGo;
        public List<Sprite> personIcon = new List<Sprite>();
        // Start is called before the first frame update
        void Start()
        {
            InitView();
        }
        void InitView() {
            firstBtn = first.GetComponent<Button>();
            firstBtn.onClick.AddListener(delegate () { SetUIByLevel(1); });
        }
        private void OnEnable()
        {
            SetUIByLevel(0);
        }
        // Update is called once per frame
        void Update()
        {
            if (resEntitySelf != null)
            {
                if (!string.IsNullOrEmpty(resEntitySelf.lineId))
                {
                    Vector3 vector3Temp = Camera.main.WorldToScreenPoint(resPointTrans.position);
                    transform.position = new Vector3(vector3Temp.x, vector3Temp.y, 0);
                }
                else if (!string.IsNullOrEmpty(resEntitySelf.apId))
                {
                    if (_dynamicResPosition != null)
                    {
                        Vector3 vector3Temp = Camera.main.WorldToScreenPoint(targetPos);
                        transform.position = new Vector3(vector3Temp.x, vector3Temp.y, 0);
                    }
                }
            }
        }
        /// <summary>
        /// 根据数据设置资源实体 
        /// </summary>
        /// <param name="resEntity"></param>
        public void Set(ResEntity resEntity,GameObject followGo) {
            if (resEntity != null)
            {
                resEntitySelf = resEntity;
                elementTypeSelf = (ElementType)resEntitySelf.type;
                first.GetComponent<Image>().sprite = tuo[resEntity.type];
                first.transform.Find("Icon").GetComponent<Image>().sprite = icon[resEntity.type];
                text1.text = resEntitySelf.data.Count > 0 ? resEntitySelf.data[0].key + ":" + resEntitySelf.data[0].value : "";
                text2.text = resEntitySelf.data.Count > 1 ? resEntitySelf.data[1].key + ":" + resEntitySelf.data[1].value : "";
                text3.text = resEntitySelf.data.Count > 2 ? resEntitySelf.data[2].key + ":" + resEntitySelf.data[2].value : "";
                if (!string.IsNullOrEmpty(resEntitySelf.icon))
                {
                    StartCoroutine(LoadPhoto(resEntitySelf.icon));
                }
                if (resEntitySelf.type == 1)
                {
                    photo.sprite = personSpriteTemp;
                }
                else if (resEntitySelf.type == 5)
                {
                    photo.sprite = sbSpriteTemp;
                }
                if (!string.IsNullOrEmpty(resEntitySelf.lineId))
                {
                    try
                    {
                        resPointTrans = followGo.transform;
                        resPointTrans.gameObject.AddComponent<ElementPoint>().elementItem = this;
                    }
                    catch 
                    {

                        Debug.LogError(resEntitySelf.id);
                    }
                    
                }
                else if(!string.IsNullOrEmpty(resEntitySelf.apId))
                {
                    try
                    {
                        resApTrans = ElementContainer.GetInstance().transform.Find(resEntitySelf.apId).GetComponent<ElementItem>().resPointTrans;
                    }
                    catch 
                    {
                        Debug.LogError("请先添加基站的数据");
                    }
                }
            }
        }
        IEnumerator LoadPhoto(string photoURL) {
            UnityWebRequest wr = new UnityWebRequest(photoURL);
            DownloadHandlerTexture texD1 = new DownloadHandlerTexture(true);
            wr.downloadHandler = texD1;
            yield return wr.SendWebRequest();
            int width = 1920;
            int high = 1080;
            if (!wr.isNetworkError)
            {
                Texture2D tex = new Texture2D(width, high);
                tex = texD1.texture;

                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                photo.sprite = sprite;
            }
        }
        /// <summary>
        /// 通过位置信息创建动态资源
        /// </summary>
        /// <param name="dynamicResPosition"></param>
        private DynamicResPosition _dynamicResPosition;
        public void SetDynamicResPosition(DynamicResPosition dynamicResPosition) {
            _dynamicResPosition = dynamicResPosition;
            if (dynamicResPosition.dType == 1)
            {
                first.transform.Find("Icon").GetComponent<Image>().sprite = personIcon[dynamicResPosition.role];
            }
            targetPos = resApTrans.position - resApTrans.up + _dynamicResPosition.forb * resApTrans.forward * _dynamicResPosition.distance + resApTrans.right;
            GameObject go;
            if (resApTrans.parent.Find("Point_" + dynamicResPosition.id) == null)
            {
                go = Instantiate(dynamicResGo) as GameObject;
                go.transform.parent = resApTrans.parent;
                go.name = "Point_" + dynamicResPosition.id;
                go.AddComponent<ElementPoint>().elementItem = this;
            }
            else
            {
                go = resApTrans.parent.Find("Point_" + dynamicResPosition.id).gameObject;
            }
            go.transform.position = targetPos;
        }
        /// <summary>
        /// 根据当前层级显示UI
        /// </summary>
        /// <param name="level">0-第一层级（默认层级），1-第二层级</param>
        public void SetUIByLevel(int level) {
            
            switch (level)
            {
                case 0:
                    Debug.Log("显示第一层级");
                    secondPanel.SetActive(false);
                    secondLine.GetComponent<Image>().DOFillAmount(0, 0.3f).OnComplete(delegate () {
                        first.SetActive(level == 0 ? true : false);
                        second.SetActive(level == 1 ? true : false);
                    }).SetEase(Ease.Linear);
                    break;
                case 1:
                    Debug.Log("显示第二层级");
                    if (ElementContainer.GetInstance().nowElement != null)
                    {
                        if (ElementContainer.GetInstance().nowElement != this)
                        {
                            ElementContainer.GetInstance().nowElement.SetUIByLevel(0);
                        }
                    }
                    first.SetActive(level == 0 ? true : false);
                    second.SetActive(level == 1 ? true : false);
                    secondLine.GetComponent<Image>().DOFillAmount(1, 0.3f).OnComplete(delegate ()
                    {
                        secondPanel.SetActive(true);
                    }).SetEase(Ease.Linear);
                    ElementContainer.GetInstance().nowElement = this;
                    if (resEntitySelf.type != 1 && resEntitySelf.type != 2)
                    {
                        Camera.main.GetComponent<BLCameraControl>().LookAtResAutoDis(GameObject.Find("Follow_" + resEntitySelf.lineId + "_" + resEntitySelf.id).transform);
                    }
                    else
                    {
                        Camera.main.GetComponent<BLCameraControl>().LookAtPosition(targetPos);
                    }
                    break;
                default:
                    break;
            }
        }
    }
    [System.Serializable]
    public class DynamicRes {
        public List<DynamicResPosition> resPositions;
    }
    [System.Serializable]
    public class DynamicResPosition {
        public string id;//id
        public string name;//名字
        public int dType;//动态资源类型 1-人员，2-车辆
        public int role;//人员或者车辆的角色  人员：0-矿领导，1-工人，2-访客
        public string apId;//基站的id
        public int forb;//前后1-前，-1-后
        public float distance;//资源到基站的距离
    }
    #region 资源指标
    [System.Serializable]
    public class ResIndex {
        public string key;
        public string value;
    }
    #endregion
    #region 资源实体
    [System.Serializable]
    public class ResEntity {
        public string id;//id
        public int type;//资源的类型 0-基站，1-人员，2-车辆，3-传感器，4-摄像机，5-设备
        public string name;//资源的名称
        public string icon;//资源对应的图片
        public string lineId;//对应的巷道id
        public float distance;//距离起点的距离
        public string apId;//如果资源类型为1或者2，apId则不为空，且为基站的ID，否则为空
        public List<ResIndex> data;//资源的其他属性 标签上显示的属性，默认是三个，可以多几个或者少几个
    }
    [System.Serializable]
    public class ResEntityList {
        public List<ResEntity> entity;
    }
    #endregion
}

