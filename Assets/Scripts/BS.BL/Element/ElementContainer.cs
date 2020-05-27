using BS.BL.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BS.BL.Element {
    public enum ElementType {
        JZ,//基站
        PERSON,//人员
        CAR,//车辆
        CGQ,//传感器
        JK,//监控
        SB,//其它设备
    }
    public class ElementContainer : MonoBehaviour
    {
        private static ElementContainer instance;
        public static ElementContainer GetInstance() {
            if (instance == null)
            {
                instance = new ElementContainer();
            }
            return instance;
        }
        /// <summary>
        /// 元素的字典
        /// </summary>
        public Dictionary<ElementType, List<ElementItem>> dicElements = new Dictionary<ElementType, List<ElementItem>>();
        public Dictionary<int, List<ElementItem>> dicPerson = new Dictionary<int, List<ElementItem>>();
        private List<string> entityIds = new List<string>();
        public GameObject P_ElementItem;
        public ElementItem nowElement;

        public TViewBase tviewBase;

        // Start is called before the first frame update
        private void Awake()
        {
            instance = this;
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (nowElement != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (Utility.IsPointerOverUIObject())
                    {
                        Debug.Log("点击到UI上");
                    }
                    else
                    {
                        nowElement.SetUIByLevel(0);
                    }
                }
            }
        }
        /// <summary>
        /// 通过资源类型和资源添加资源
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="elementItem"></param>
        public void AddElements(ElementType elementType, ElementItem elementItem) {
            if (dicElements.ContainsKey(elementType))
            {
                if (dicElements[elementType].Contains(elementItem))
                {
                    dicElements[elementType].Add(elementItem);
                }
            }
            else
            {
                List<ElementItem> elementItems = new List<ElementItem>();
                elementItems.Add(elementItem);
                dicElements.Add(elementType, elementItems);
            }
        }
        public List<ElementItem> GetElementItems(ElementType elementType) {
            if (dicElements.ContainsKey(elementType))
            {
                return dicElements[elementType];
            }
            return null;
        }

        private int nowStateTemp;
        public void SetEntity(string result)
        {
            //nowStateTemp = (MainManager.GetInstance().dicAllManager[ManagerType.uiManager] as UIManager).nowState;
            ResEntityList resEntityList = JsonUtility.FromJson<ResEntityList>(result);
            foreach (ResEntity item in resEntityList.entity)
            {
                GameObject go;
                List<ElementItem> elementItems = new List<ElementItem>();
                if (!dicElements.ContainsKey((ElementType)item.type))
                {
                    Line line = GameObject.Find("Line_" + item.lineId).GetComponent<Line>();
                    Vector3 vFor = (line.endPoint.pos - line.startPoint.pos).normalized;
                    GameObject followGo = new GameObject();
                    followGo.transform.parent = tviewBase.transform.Find("Layer_HD/Points");
                    followGo.name = "Follow_" + item.lineId + "_" + item.id;
                    followGo.transform.localPosition = line.startPoint.pos + vFor * item.distance;
                    go = Instantiate(P_ElementItem) as GameObject;
                    go.transform.parent = transform;
                    go.name = item.id;
                    go.GetComponent<ElementItem>().Set(item, followGo);
                    entityIds.Add(item.id);
                    elementItems.Add(go.GetComponent<ElementItem>());
                    dicElements.Add((ElementType)item.type, elementItems);
                }
                else
                {
                    elementItems = dicElements[(ElementType)item.type];
                    if (entityIds.Contains(item.id))
                    {
                        go = transform.Find(item.id).gameObject;
                    }
                    else
                    {
                        Line line = GameObject.Find("Line_" + item.lineId).GetComponent<Line>();
                        Vector3 vFor = (line.endPoint.pos - line.startPoint.pos).normalized;
                        GameObject followGo = new GameObject();
                        followGo.transform.parent = tviewBase.transform.Find("Layer_HD/Points");
                        followGo.name = "Follow_" + item.lineId + "_" + item.id;
                        followGo.transform.localPosition = line.startPoint.pos + vFor * item.distance;
                        go = Instantiate(P_ElementItem) as GameObject;
                        go.transform.parent = transform;
                        go.name = item.id;
                        go.GetComponent<ElementItem>().Set(item, GameObject.Find("Follow_" + item.lineId + "_" + item.id));
                        entityIds.Add(item.id);
                    }
                    try
                    {
                        if (!dicElements[(ElementType)item.type].Contains(go.GetComponent<ElementItem>()))
                        {
                            dicElements[(ElementType)item.type].Add(go.GetComponent<ElementItem>());
                        }
                    }
                    catch
                    {
                        Debug.Log(go.name);
                        throw;
                    }
                    
                }
            }
        }
        public void SetDynamicResPosition(string result)
        {
            nowStateTemp = (MainManager.GetInstance().dicAllManager[ManagerType.uiManager] as UIManager).nowState;
            DynamicRes dynamicRes = JsonUtility.FromJson<DynamicRes>(result);
            foreach (DynamicResPosition item in dynamicRes.resPositions)
            {
                foreach (ElementItem elementItem in dicElements[(ElementType)item.dType])
                {
                    if (elementItem.name == item.id)
                    {
                        if (nowStateTemp == 0 || (nowStateTemp == 1 && item.dType == 1) || (nowStateTemp == 5 && item.dType == 2))
                        {
                            elementItem.gameObject.SetActive(true);
                        }
                        else
                        {
                            elementItem.gameObject.SetActive(false);
                        }
                        elementItem.SetDynamicResPosition(item);
                        if (!dicPerson.ContainsKey(item.role))
                        {
                            List<ElementItem> elementList = new List<ElementItem>();
                            elementList.Add(elementItem);
                            dicPerson.Add(item.role, elementList);
                        }
                        else
                        {
                            dicPerson[item.role].Add(elementItem);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 通过资源类型设置资源标签的显示隐藏
        /// </summary>
        /// <param name="_elementType"></param>
        public void SetElementItemVisible(ElementType _elementType) {
            foreach (KeyValuePair<ElementType,List<ElementItem>> item in dicElements)
            {
                foreach (ElementItem itemElement in item.Value)
                {
                    itemElement.gameObject.SetActive(false);
                }
                if (item.Key == _elementType)
                {
                    foreach (ElementItem itemActive in item.Value)
                    {
                        itemActive.gameObject.SetActive(true);
                    }
                }
            }
        }
        /// <summary>
        /// 通过角色设置标签的显示隐藏
        /// </summary>
        /// <param name="_role"></param>
        public void SetPersonItemVisible(int _role) {
            foreach (KeyValuePair<int, List<ElementItem>> item in dicPerson)
            {
                if (_role != -1)
                {
                    foreach (ElementItem itemElement in item.Value)
                    {
                        itemElement.gameObject.SetActive(false);
                    }
                    if (item.Key == _role)
                    {
                        foreach (ElementItem itemActive in item.Value)
                        {
                            if (itemActive.resEntitySelf.type == 1)
                            {
                                itemActive.gameObject.SetActive(true);
                            }
                        }
                    }
                }
                else
                {
                    foreach (ElementItem itemElement in item.Value)
                    {
                        if (itemElement.resEntitySelf.type == 1)
                        {
                            itemElement.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }
}

