using BS.BL.Manager;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Networking;
using BS.BL.Two.Loader;
using BS.BL.Interface;

namespace BS.BL.Two.Element
{
    public enum DomMode {
        zhjk,//综合监控
        xj,//巡检
        lx,//路线
        td,//2D
        jj,//掘进
        qx//区域选择
    }
    public class ElementContainer : MonoBehaviour
    {
        private static ElementContainer instance;
        public static ElementContainer GetInstance()
        {
            if (instance == null)
            {
                instance = new ElementContainer();
            }
            return instance;
        }
        /// <summary>
        /// 元素的字典
        /// </summary>
        public Dictionary<string, List<ElementItem>> dicElements = new Dictionary<string, List<ElementItem>>();
        public Dictionary<int, List<ElementItem>> dicPerson = new Dictionary<int, List<ElementItem>>();
        public Dictionary<string, GameObject> dicTunnel = new Dictionary<string, GameObject>();
        private List<string> listCodes = new List<string>();
        public GameObject P_ElementItem, P_ElementArray, P_ElementArea;
        public ElementItem nowElement;

        public TViewBase tviewBase;
        public Transform tunnelPa;
        public Transform wayPa;
        public List<ElementItem> listSelectItems = new List<ElementItem>();



        public DomMode domMode = DomMode.zhjk;
        //////////////////////////20200317新加字段--生成三维标签样式/////////////////////////////////
        public Transform lineLabelParent, upLabelParent, arrayLabelParent, areaLabelParent;
        public GameObject arrayGo;
        public Material matLine;
        public Transform cameraStartTrans;


        ///////////////////////////////////////////////20200429重构/////////////////////////////////////////////////////
        public LoadConfig loadConfig;
        public Material matArea;
        private JSInterface jsInterface;

        public bool isClick = true;

        // Start is called before the first frame update
        private void Awake()
        {
            instance = this;
            jsInterface = GameObject.Find("JSInterface").GetComponent<JSInterface>();
            loadConfig = new LoadConfig();
        }
        // Update is called once per frame
        void Update()
        {
            if (!Utility.IsPointerOverUIObject())
            {
                if (RayTools.GetArrayIndex() == 3)
                {

                    if (level == 2 && !DynamicElement.GetInstance().isPlay && !DynamicElement.GetInstance().inPlay)
                    {
                        ResetLabel();
                    }
                    if (level == 1 && !DynamicElement.GetInstance().isPlay && !DynamicElement.GetInstance().inPlay)
                    {
                        ResetArray();
                    }
                }
            }
            
        }
        public int level = 0;
        public void SceneToLevel(int _level) {
            StartCoroutine(WaitSomeTime(_level));
        }
        private IEnumerator WaitSomeTime(int _level) {
            yield return new WaitForSeconds(0.5f);
            level = _level;
            if (_level == 1)
            {
                nowElement = null;
            }
            if (_level == 0)
            {
                arrayGo = null;
            }
        }
        /// <summary>
        /// 初始化数据方法
        /// </summary>
        /// <param name="result"></param>
        public Dictionary<string, List<Transform>> dicNameLines = new Dictionary<string, List<Transform>>();
        public Dictionary<string, List<Transform>> dicAreaLines = new Dictionary<string, List<Transform>>();
        public Dictionary<string, LineData> dicLines = new Dictionary<string, LineData>();
        public Dictionary<string, List<ElementItem>> dicAreaElements = new Dictionary<string, List<ElementItem>>();
        public Dictionary<int, List<WayData>> dicWays = new Dictionary<int, List<WayData>>();//路径
        public Dictionary<string, string> dicAreaName = new Dictionary<string, string>();//区域配置
        public Dictionary<string, string> dicLineName = new Dictionary<string, string>();//巷道名称配置
        public Dictionary<string, ConfigType> diceType = new Dictionary<string, ConfigType>();//资源类型配置
        public Dictionary<string, Sprite> diceTypeSprites = new Dictionary<string, Sprite>();
        public Dictionary<string, Entity> dicAlarmEntity = new Dictionary<string, Entity>();
        public Dictionary<string, List<Transform>> dicLineLabels = new Dictionary<string, List<Transform>>();
        public Dictionary<string, string> dicEntityLine = new Dictionary<string, string>();


        public List<string> listType = new List<string>();
        public void ResetLabel(bool zhankai = false) {
            if (nowElement != null)
            {
                foreach (KeyValuePair<string, List<ElementItem>> item in dicElements)
                {
                    foreach (ElementItem itemValue in item.Value)
                    {
                        if (itemValue.nowLevel == 1)
                        {
                            if (itemValue != nowElement)
                            {
                                itemValue.SetUIByLevel(0);
                            }
                        }
                    }
                }
                if (DynamicElement.GetInstance().exist)
                {
                    nowElement.SetUIByLevel(0, zhankai);
                }
                else
                {
                    Destroy(nowElement.gameObject);
                }
                SceneToLevel(1);
            }
        }
        private void ResetArray() {
            if (arrayGo != null && nowElement == null)
            {
                arrayGo.SetActive(true);
                SceneToLevel(0);
            }
        }
        /// <summary>
        /// 初始化资源实体
        /// </summary>
        /// <param name="result"></param>
        public void RefreshRes(string result)
        {
            //ResetLabel();
            if (!DynamicElement.GetInstance().inPlay)
            {
                if (domMode == DomMode.zhjk || domMode == DomMode.jj || domMode == DomMode.qx)
                {
                    transform.position = tviewBase.transform.position;
                    JObject resEntityList = JObject.Parse(result);
                    //listType.Clear();
                    foreach (JObject item in resEntityList["entity"])
                    {
                        Entity entity = JsonUtility.FromJson<Entity>(item.ToString());
                        if (entity.a_type == 0)//地下资源
                        {
                            if (entity.state == 0)
                            {
                                if (dicAlarmEntity.ContainsKey(entity.id))
                                {
                                    dicAlarmEntity.Remove(entity.id);
                                }
                            }
                            else
                            {
                                if (dicAlarmEntity.ContainsKey(entity.id))
                                {
                                    dicAlarmEntity[entity.id] = entity;
                                }
                                else
                                {
                                    dicAlarmEntity.Add(entity.id, entity);
                                }
                            }
                            if (!listType.Contains(entity.t_eCode))
                            {
                                listType.Add(entity.t_eCode);
                            }
                            if (entity.exist == 1)
                            {
                                if (dicElements.ContainsKey(entity.t_eCode))
                                {
                                    if (lineLabelParent.Find(entity.t_eCode + "/" + entity.id) != null)
                                    {
                                        Transform transElementTemp = lineLabelParent.Find(entity.t_eCode + "/" + entity.id);
                                        if (dicElements[entity.t_eCode].Contains(transElementTemp.GetComponent<ElementItem>()))
                                        {
                                            dicElements[entity.t_eCode].Remove(transElementTemp.GetComponent<ElementItem>());
                                        }
                                        if (dicLineLabels[entity.position.lineId].Contains(transElementTemp))
                                        {
                                            dicLineLabels[entity.position.lineId].Remove(transElementTemp);
                                        }
                                        DestroyImmediate(transElementTemp.gameObject);
                                    }
                                }
                            }
                            else if (entity.exist == 0)//添加
                            {
                                if (!string.IsNullOrEmpty(entity.position.lineId))
                                {
                                    if (dicLines.ContainsKey(entity.position.lineId))
                                    {
                                        Line line = tviewBase.transform.Find(dicLines[entity.position.lineId].a_eCode + "/Line_"
                                            + entity.position.lineId).GetComponent<Line>();
                                        Vector3 vFor = (line.endPoint.pos - line.startPoint.pos).normalized;
                                        if (entity.position.distance < line.lineLenght)
                                        {
                                            GameObject goItem;
                                            if (listCodes.Contains(entity.id))
                                            {
                                                goItem = lineLabelParent.Find(entity.t_eCode + "/" + entity.id).gameObject;
                                            }
                                            else
                                            {
                                                goItem = Instantiate(P_ElementItem) as GameObject;
                                            }
                                            Transform goItemPa;
                                            if (dicElements.ContainsKey(entity.t_eCode))
                                            {
                                                goItemPa = lineLabelParent.Find(entity.t_eCode);
                                                goItem.transform.parent = goItemPa;
                                                goItem.name = entity.id;
                                                if (!DynamicElement.GetInstance().inPlay)
                                                {
                                                    goItem.transform.localPosition = line.startPoint.pos + vFor * entity.position.distance;

                                                }
                                                //goItem.GetComponent<ElementItem>().Set(entity);
                                                dicElements[entity.t_eCode].Add(goItem.GetComponent<ElementItem>());
                                            }
                                            else
                                            {
                                                GameObject goPa = new GameObject();
                                                goPa.transform.parent = lineLabelParent;
                                                goPa.name = entity.t_eCode;
                                                goPa.transform.localPosition = Vector3.zero;
                                                goItemPa = goPa.transform;
                                                goItem.transform.parent = goItemPa;
                                                goItem.name = entity.id;
                                                if (!DynamicElement.GetInstance().inPlay)
                                                {
                                                    goItem.transform.localPosition = line.startPoint.pos + vFor * entity.position.distance;

                                                }
                                                //goItem.GetComponent<ElementItem>().Set(entity);
                                                List<ElementItem> elementItems = new List<ElementItem>();
                                                elementItems.Add(goItem.GetComponent<ElementItem>());
                                                dicElements.Add(entity.t_eCode, elementItems);

                                            }
                                            if (dicLines.ContainsKey(entity.position.lineId))
                                            {
                                                if (dicAreaElements.ContainsKey(dicLines[entity.position.lineId].a_eCode))
                                                {
                                                    dicAreaElements[dicLines[entity.position.lineId].a_eCode].Add(goItem.GetComponent<ElementItem>());
                                                }
                                                else
                                                {
                                                    List<ElementItem> elementItemsTemp = new List<ElementItem>();
                                                    elementItemsTemp.Add(goItem.GetComponent<ElementItem>());
                                                    dicAreaElements.Add(dicLines[entity.position.lineId].a_eCode, elementItemsTemp);
                                                }
                                            }
                                            if (!MainManager.GetInstance().GetComponent<SelectObjects>().characters.Contains(goItem))
                                            {
                                                MainManager.GetInstance().GetComponent<SelectObjects>().characters.Add(goItem);
                                            }
                                            if (!dicLineLabels.ContainsKey(entity.position.lineId))
                                            {
                                                List<Transform> listTemp = new List<Transform>();
                                                listTemp.Add(goItem.transform);
                                                dicLineLabels.Add(entity.position.lineId, listTemp);
                                            }
                                            else
                                            {
                                                if (!dicLineLabels[entity.position.lineId].Contains(goItem.transform))
                                                {
                                                    dicLineLabels[entity.position.lineId].Add(goItem.transform);
                                                }
                                            }
                                            if (!dicEntityLine.ContainsKey(entity.id))
                                            {
                                                dicEntityLine.Add(entity.id, entity.position.lineId);
                                            }

                                            //刷新
                                            if (!DynamicElement.GetInstance().inPlay)
                                            {
                                                Transform itemPos = lineLabelParent.Find(entity.t_eCode + "/" + entity.id);
                                                if (entity.position.content.Count == 0)
                                                {
                                                    if (!string.IsNullOrEmpty(itemPos.GetComponent<ElementItem>().lineId))
                                                    {
                                                        if (entity.position.lineId == itemPos.GetComponent<ElementItem>().lineId)
                                                        {
                                                            itemPos.DOLocalMove(line.startPoint.pos + vFor * entity.position.distance, 1).SetEase(Ease.Linear);
                                                        }
                                                        else
                                                        {
                                                            itemPos.localPosition = line.startPoint.pos + vFor * entity.position.distance;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    List<Vector3> listVectorTmep = new List<Vector3>();
                                                    foreach (PositionContent pos in entity.position.content)
                                                    {
                                                        Line lineTemp = GameObject.Find("Line_" + pos.lineId).GetComponent<Line>();
                                                        Vector3 vForTemp = (lineTemp.endPoint.pos - lineTemp.startPoint.pos).normalized;
                                                        if (pos.distance < lineTemp.lineLenght)
                                                        {
                                                            listVectorTmep.Add(lineTemp.startPoint.pos + vForTemp * pos.distance);
                                                        }
                                                        else
                                                        {
                                                            Debug.Log("刷新资源中间点，" + entity.id + "资源超出" + lineTemp.id + "巷道的长度范围");
                                                        }
                                                    }
                                                    Line lineOver = GameObject.Find("Line_" + entity.position.lineId).GetComponent<Line>();
                                                    Vector3 vForOver = (lineOver.endPoint.pos - lineOver.startPoint.pos).normalized;
                                                    Vector3 posOver = lineOver.startPoint.pos + vForOver * entity.position.distance;
                                                    DynamicResMove(itemPos, listVectorTmep, posOver, 1f / entity.position.content.Count);
                                                }
                                                itemPos.GetComponent<ElementItem>().Set(entity, true);
                                                if (dicEntityLine.ContainsKey(entity.id))
                                                {
                                                    if (dicEntityLine[entity.id] != entity.position.lineId)
                                                    {
                                                        if (dicLineLabels.ContainsKey(dicEntityLine[entity.id]))
                                                        {
                                                            for (int i = 0; i < dicLineLabels[dicEntityLine[entity.id]].Count; i++)
                                                            {
                                                                if (dicLineLabels[dicEntityLine[entity.id]][i] != null)
                                                                {
                                                                    if (dicLineLabels[dicEntityLine[entity.id]][i].name == entity.id)
                                                                    {
                                                                        dicLineLabels[dicEntityLine[entity.id]].Remove(dicLineLabels[dicEntityLine[entity.id]][i]);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (dicLineLabels.ContainsKey(entity.position.lineId))
                                                        {
                                                            if (!dicLineLabels[entity.position.lineId].Contains(itemPos.transform))
                                                            {
                                                                dicLineLabels[entity.position.lineId].Add(itemPos.transform);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            List<Transform> listTemp = new List<Transform>();
                                                            listTemp.Add(itemPos.transform);
                                                            dicLineLabels.Add(entity.position.lineId, listTemp);
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                        else
                                        {
                                            Debug.Log("添加/刷新资源时，" + entity.id + "资源超出" + line.id + "巷道的长度范围");
                                        }
                                    }
                                }
                                else
                                {
                                    Debug.LogError("请配置巷道");
                                }
                            }
                        }
                        else if (int.Parse(item["a_type"].ToString()) == 1)//地上资源
                        {
                            GameObject goItem = Instantiate(P_ElementItem) as GameObject;
                        }
                        else
                        {
                            Debug.Log(item["id"] + "资源所属地上地下划分错误");
                        }
                    }
                    ShowOrHideLabel();
                    string callback = "[";
                    foreach (KeyValuePair<string, Entity> item in dicAlarmEntity)
                    {
                        string range = ShowAlarmRange(item.Value);
                        callback += "{\"alarmResId\":\"" + item.Key + "\",\"alarmResTCode\":\"" + item.Value.t_eCode + "\",\"alarmRangeRes\":[" + range + "]},";
                    }
                    if (!string.IsNullOrEmpty(callback) && callback.Contains(","))
                    {
                        callback = callback.Remove(callback.LastIndexOf(','), 1);
                    }
                    callback += "]";
                    //Debug.Log("报警范围" + callback);
                    jsInterface.GetComponent<JSInterface>().sendAlarmRes(callback);
                    listCodes.Clear();
                    foreach (Transform item in lineLabelParent.GetComponentsInChildren<Transform>(true))
                    {
                        if (!dicElements.ContainsKey(item.name) && item.name != "LineLabelParent")
                        {
                            listCodes.Add(item.name);
                        }
                    }
                }
                else
                {
                    Debug.Log("请进入综合监控");
                }
            }
            
            
        }
        private void ShowOrHideLabel() {
            foreach (KeyValuePair<string,List<Transform>> item in dicLineLabels)
            {
                Line line = tviewBase.transform.Find(dicLines[item.Key].a_eCode + "/Line_" + item.Key).GetComponent<Line>();
                double num = Math.Floor(line.lineLenght / Manager.GetInstance().limitConfig.perDis);

                if (item.Value.Count > num && item.Value.Count > 1)
                {
                    foreach (Transform itemChild in item.Value)
                    {
                        if (itemChild != null)
                        {
                            itemChild.gameObject.SetActive(false);
                        }
                    }
                    GameObject goItem = null;
                    if (arrayLabelParent.Find(item.Key) == null)
                    {
                        goItem = Instantiate(P_ElementArray) as GameObject;
                        goItem.name = item.Key;
                        goItem.transform.SetParent(arrayLabelParent);
                        goItem.transform.localPosition = line.transform.localPosition;
                    }
                    else
                    {
                        goItem = arrayLabelParent.Find(item.Key).gameObject;
                    }
                    goItem.GetComponent<ElementArray>().Set(item.Value);
                }
            }
        }
        /// <summary>
        /// 刷新资源
        /// </summary>
        /// <param name="result"></param>
        //public void RefreshResEntity(string result)
        //{
        //    if (domMode == DomMode.zhjk || domMode == DomMode.jj || domMode == DomMode.qx)
        //    {
        //        JObject resPosition = JObject.Parse(result);
        //        foreach (JObject item in resPosition["entity"])
        //        {
        //            Entity entity = JsonUtility.FromJson<Entity>(item.ToString());
        //            try
        //            {
        //                Transform itemPos = lineLabelParent.Find(entity.t_eCode + "/" + entity.id);
        //                if (entity.state == 0)
        //                {
        //                    if (dicAlarmEntity.ContainsKey(entity.id))
        //                    {
        //                        dicAlarmEntity.Remove(entity.id);
        //                    }
        //                }
        //                else
        //                {
        //                    if (dicAlarmEntity.ContainsKey(entity.id))
        //                    {
        //                        dicAlarmEntity[entity.id] = entity;
        //                    }
        //                    else
        //                    {
        //                        dicAlarmEntity.Add(entity.id, entity);
        //                    }
        //                }
        //                if (entity.exist == 1)
        //                {
        //                    if (lineLabelParent.Find(entity.t_eCode + "/" + entity.id) != null)
        //                    {
        //                        DestroyImmediate(lineLabelParent.Find(entity.t_eCode + "/" + entity.id).gameObject);
        //                    }
        //                }
        //                else if (entity.exist == 0)
        //                {
        //                    if (entity.a_type == 0)//地下
        //                    {
        //                        if (dicElements.ContainsKey(entity.t_eCode))
        //                        {
        //                            Line line = tviewBase.transform.Find(dicLines[entity.position.lineId].a_eCode + "/Line_"
        //                            + entity.position.lineId).GetComponent<Line>();
        //                            Vector3 vFor = (line.endPoint.pos - line.startPoint.pos).normalized;
        //                            if (entity.position.distance < line.lineLenght)
        //                            {
        //                                //Debug.Log("content" + position.content.Count);
        //                            }
        //                            else
        //                            {
        //                                Debug.Log("刷新资源时，" + entity.id + "资源超出" + line.id + "巷道的长度范围");
        //                            }
        //                        }
        //                    }
        //                }
        //                ShowOrHideLabel();
        //            }
        //            catch
        //            {
        //                Debug.Log("更新位置错误");
        //            }
        //        }
        //        string callback = "[";
        //        foreach (KeyValuePair<string,Entity> item in dicAlarmEntity)
        //        {
        //            string range = ShowAlarmRange(item.Value);
        //            callback += "{\"alarmResId\":\"" + item.Key + "\",\"alarmResTCode\":\"" + item.Value.t_eCode + "\",\"alarmRangeRes\":[" + range + "]},";
        //        }
        //        if (!string.IsNullOrEmpty(callback) && callback.Contains(","))
        //        {
        //            callback = callback.Remove(callback.LastIndexOf(','), 1);
        //        }
        //        callback += "]";
        //        Debug.Log("报警范围" + callback);
        //        jsInterface.GetComponent<JSInterface>().sendAlarmRes(callback);
        //    }
        //    else
        //    {
        //        Debug.Log("非综合监控模式下刷新无效");
        //    }
            
        //}
        public string ShowAlarmRange(Entity entity) {
            string result = "";
            Transform itemPos = lineLabelParent.Find(entity.t_eCode + "/" + entity.id);
            foreach (ElementItem item in transform.GetComponentsInChildren<ElementItem>())
            {
                if (item.name != entity.id && itemPos != null)
                {
                    if (Vector3.Distance(item.transform.position, itemPos.position) <= entity.s_data.range)
                    {
                        result += "{\"resId\":\"" + item.name + "\",\"t_eCode\":\"" + item.t_eType + "\"},";
                    }
                }
            }
            if (!string.IsNullOrEmpty(result))
            {
                result = result.Remove(result.LastIndexOf(','), 1);
            }
            return result;
        }
        public void SingleAlarm(Transform alarmTs, float _range, Dictionary<string,List<ElementItem>> _dicElements) {
            string result = "[{\"alarmResId\":\"" + alarmTs.name + "\",\"alarmResTCode\":\"" + alarmTs.GetComponent<ElementItem>().t_eType + "\",\"alarmRangeRes\":[";
            foreach (ElementItem item in transform.GetComponentsInChildren<ElementItem>())
            {
                if (item.name != alarmTs.name)
                {
                    if (Vector3.Distance(item.transform.position, alarmTs.position) <= _range)
                    {
                        result += "{\"resId\":\"" + item.name + "\",\"t_eCode\":\"" + item.t_eType + "\"},";
                        if (_dicElements.ContainsKey(item.t_eType))
                        {
                            _dicElements[item.t_eType].Add(item);
                        }
                        else
                        {
                            List<ElementItem> listTemp = new List<ElementItem>();
                            listTemp.Add(item);
                            _dicElements.Add(item.t_eType, listTemp);
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(result))
            {
                result = result.Remove(result.LastIndexOf(','), 1);
            }
            result += "]}]";
            jsInterface.GetComponent<JSInterface>().sendAlarmRes(result);
            //return result;
        }
        /// <summary>
        /// 动态资源移动动画
        /// </summary>
        /// <param name="entityTrans"></param>
        /// <param name="vector3s"></param>
        /// <param name="overPos"></param>
        /// <param name="timeTemp"></param>
        /// <param name="indexTemp"></param>
        private void DynamicResMove(Transform entityTrans, List<Vector3> vector3s,Vector3 overPos, float timeTemp, int indexTemp = 0) {
            if (vector3s.Count > 0)
            {
                if (entityTrans != null)
                {
                    entityTrans.DOLocalMove(vector3s[indexTemp], timeTemp).SetEase(Ease.Linear).OnComplete(delegate () {
                        if (indexTemp < vector3s.Count - 1)
                        {
                            indexTemp++;
                            DynamicResMove(entityTrans, vector3s, overPos, timeTemp, indexTemp);
                        }
                        else
                        {
                            entityTrans.DOLocalMove(overPos, timeTemp);
                        }
                    });
                }
            }
        }
        /// <summary>
        /// 通过资源ID设置当前资源
        /// </summary>
        /// <param name="resId"></param>
        public void SetDominantRes(string result) {
            if (domMode == DomMode.zhjk || domMode == DomMode.jj || domMode == DomMode.qx)
            {
                JObject res = JObject.Parse(result);
                if (lineLabelParent.Find(res["t_eCode"].ToString() + "/" + res["resId"].ToString()) != null)
                {
                    if (arrayLabelParent.Find(lineLabelParent.Find(res["t_eCode"].ToString() + "/" + res["resId"].ToString()).
                        GetComponent<ElementItem>().lineId) != null)
                    {
                        ElementArray _elementArray = arrayLabelParent.Find(lineLabelParent.Find(res["t_eCode"].ToString() + "/" + res["resId"].ToString()).
                        GetComponent<ElementItem>().lineId).GetComponent<ElementArray>();
                        if (_elementArray != null)
                        {
                            _elementArray.gameObject.SetActive(false);
                        }
                    }
                    Transform tsTemp = lineLabelParent.Find(res["t_eCode"].ToString() + "/" + res["resId"].ToString());
                    Camera.main.GetComponent<BLCameraControl>().LookAtPosition(tsTemp.position);
                    tsTemp.GetComponent<ElementItem>().SetUIByLevel(1);
                }
            }
            else
            {
                Debug.Log("非综合监控模式下选择资源无效");
            }
            
        }
        /// <summary>
        /// 通过资源类型显示资源，支持多选
        /// </summary>
        /// <param name="resTypes"></param>
        public void SetDminantResByTypes(string resTypes) {
            if (!DynamicElement.GetInstance().inPlay)
            {
                if (domMode == DomMode.zhjk || domMode == DomMode.jj || domMode == DomMode.qx)
                {
                    resTypes = "{ \"list\": " + resTypes + "}";
                    ResTypes resTypesTemp = JsonUtility.FromJson<ResTypes>(resTypes);
                    listType.Clear();
                    try
                    {
                        foreach (Transform item in lineLabelParent)
                        {
                            item.gameObject.SetActive(false);
                        }
                        foreach (string itemName in resTypesTemp.list)
                        {
                            if (lineLabelParent.Find(itemName) != null)
                            {
                                if (!listType.Contains(itemName))
                                {
                                    listType.Add(itemName);
                                }
                                lineLabelParent.Find(itemName).gameObject.SetActive(true);
                            }
                        }
                        ShowOrHideLabel();
                    }
                    catch (Exception E)
                    {
                        Debug.Log("按类型显示错误" + E);
                    }
                }
                else
                {
                    Debug.Log("非综合监控模式下选择资源类型无效");
                }
            }
        }
        /// <summary>
        /// 通过巷道ID选择巷道
        /// </summary>
        /// <param name="lineId"></param>
        public void SetDominatTunnel(string lineId) {
            if (domMode == DomMode.zhjk)
            {
                try
                {
                    foreach (Transform item in tviewBase.GetComponentsInChildren<Transform>())
                    {
                        if (item.name == "Line_" + lineId)
                        {
                            Camera.main.GetComponent<BLCameraControl>().LookAtPosition(item.position);
                        }
                    }
                }
                catch
                {
                    Debug.Log("不存在此ID" + lineId);
                }
            }
            else
            {
                Debug.Log("非综合监控模式下选择巷道无效");
            }
        }
        /// <summary>
        /// 通过区域ID选择区域
        /// </summary>
        /// <param name="areaResult"></param>
        public void SetDominatArea(string areaResult) {
            if (domMode == DomMode.qx)
            {
                JObject area = JObject.Parse(areaResult);
                if (dicAreaName.ContainsKey(area["eCode"].ToString()))
                {
                    foreach (Transform item in areaLabelParent)
                    {
                        DestroyImmediate(item.gameObject);
                    }
                    List<Transform> listTrans = new List<Transform>();
                    GameObject goAreaSelect = GameObject.Find("AreaSelect");
                    PolygonDrawer polygonDrawer = goAreaSelect.GetComponent<PolygonDrawer>();
                    goAreaSelect.transform.position = tviewBase.transform.position;
                    if (polygonDrawer.listLines.Count > 0)
                    {
                        foreach (Transform item in polygonDrawer.listLines)
                        {
                            if (item.GetComponent<Line>() != null)
                            {
                                item.GetChild(0).GetComponent<MeshRenderer>().material = item.GetComponent<Line>().dataSetMat;
                            }
                        }
                    }
                    polygonDrawer.listLines.Clear();
                    foreach (Transform item in goAreaSelect.transform)
                    {
                        DestroyImmediate(item.gameObject);
                    }
                    if (dicAreaLines.ContainsKey(area["eCode"].ToString()))
                    {
                        foreach (Line item in tviewBase.transform.Find(area["eCode"].ToString()).GetComponentsInChildren<Line>())
                        {
                            GameObject goStart = new GameObject();
                            GameObject goEnd = new GameObject();
                            goStart.transform.parent = goAreaSelect.transform;
                            goEnd.transform.parent = goAreaSelect.transform;
                            goStart.transform.localPosition = item.startPoint.pos;
                            goEnd.transform.localPosition = item.endPoint.pos;
                            listTrans.Add(goStart.transform);
                            listTrans.Add(goEnd.transform);
                            polygonDrawer.listLines.Add(item.transform);
                        }
                    }
                    if (listTrans.Count > 2)
                    {
                        Transform ts1 = listTrans[0];
                        Transform ts2 = listTrans[1];
                        List<Transform> listTransTemp = new List<Transform>();
                        listTrans.RemoveAt(0);
                        listTrans.RemoveAt(0);
                        if (listTrans.Count > 1)
                        {
                            listTrans.Sort(delegate (Transform x, Transform y) {
                                Debug.Log("x" + MathTools.GetAxis(ts2.position, ts1.position, x.position) + "y" + MathTools.GetAxis(ts2.position, ts1.position, y.position));
                                return -MathTools.GetAxis(ts2.position, ts1.position, x.position).
                                CompareTo(MathTools.GetAxis(ts2.position, ts1.position, y.position));
                            });
                        }
                        listTransTemp.Add(ts1);
                        listTransTemp.Add(ts2);
                        foreach (Transform item in listTrans)
                        {
                            listTransTemp.Add(item);
                        }
                        Color nowColor = ColorToRGBA.GetColor(area["color"].ToString());
                        Color newColor = new Color(nowColor.r, nowColor.g, nowColor.b, 0.3f);
                        if (area["areaType"].ToString() == "area")
                        {
                            polygonDrawer.Init(listTransTemp, nowColor, area["eCode"].ToString());
                            if (goAreaSelect.GetComponent<MeshCollider>() == null)
                            {
                                goAreaSelect.AddComponent<MeshCollider>();
                            }
                            if (goAreaSelect.transform.childCount == 0)
                            {
                                return;
                            }
                            Camera.main.GetComponent<BLCameraControl>().LookAtResAutoDis2(goAreaSelect.transform);
                            GameObject go = Instantiate(P_ElementArea) as GameObject;
                            go.transform.parent = areaLabelParent;
                            go.transform.position = NormalCenter.GetCenter(goAreaSelect.transform);
                            go.GetComponent<ElementArea>().Set(dicAreaName[area["eCode"].ToString()]);
                            go.name = area["eCode"].ToString(); 
                        }
                        else
                        {
                            GameObject go = Instantiate(P_ElementArea) as GameObject;
                            go.transform.parent = areaLabelParent;
                            go.transform.position = NormalCenter.GetCenter(polygonDrawer.listLines);
                            go.GetComponent<ElementArea>().Set(dicAreaName[area["eCode"].ToString()]);
                            go.name = area["eCode"].ToString();
                            Camera.main.GetComponent<BLCameraControl>().LookAtResAutoDis3(polygonDrawer.listLines);
                        }
                        foreach (Transform item in polygonDrawer.listLines)
                        {
                            Material matTemp = new Material(matArea);
                            if (area["areaType"].ToString() == "area")
                            {
                                matTemp.color = newColor;
                            }
                            else
                            {
                                matTemp.color = nowColor;
                            }
                            item.GetChild(0).GetComponent<MeshRenderer>().material = matTemp;
                        }


                        foreach (KeyValuePair<string, List<ElementItem>> itemElement in dicAreaElements)
                        {
                            if (itemElement.Key != areaResult)
                            {
                                foreach (ElementItem itemChild in itemElement.Value)
                                {
                                    itemChild.gameObject.SetActive(false);
                                }
                            }
                            else
                            {
                                foreach (ElementItem itemChild in itemElement.Value)
                                {
                                    itemChild.gameObject.SetActive(true);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("当前区域不存在");
                }
               
            }
            else
            {
                Debug.Log("请先设置综合监控模式下的区域选择状态");
            }
            
        }
        /// <summary>
        /// 设置掘进进度
        /// </summary>
        /// <param name="tunnelResult"></param>
        public GameObject tunnelGo;
        public void SetTunnel(string tunnelResult) {
            if (domMode == DomMode.jj)
            {
                tunnelResult = "{ \"list\": " + tunnelResult + "}";
                TunnelList tunnelList = JsonUtility.FromJson<TunnelList>(tunnelResult);
                foreach (Tunnel item in tunnelList.list)
                {
                    if (dicLines.ContainsKey(item.lineId))
                    {
                        Line line = tviewBase.transform.Find(dicLines[item.lineId].a_eCode).Find("Line_" + item.lineId).GetComponent<Line>();
                        Vector3 vFor = (line.endPoint.pos - line.startPoint.pos).normalized;
                        Vector3 tunnelStart = line.startPoint.pos + vFor * item.tunnelDegree * (Vector3.Distance(line.startPoint.pos, line.endPoint.pos));
                        GameObject tunnelGa = Instantiate(tunnelGo) as GameObject;
                        tunnelGa.transform.parent = tunnelPa;
                        tunnelGa.transform.GetChild(0).GetComponent<MeshRenderer>().material = tunnelGa.GetComponent<TunnelLine>().tunnelMat;
                        tunnelGa.name = "Tunnel_" + item.lineId;
                        tunnelGa.transform.eulerAngles = line.transform.eulerAngles;
                        tunnelGa.transform.localPosition = (line.endPoint.pos + tunnelStart) / 2;
                        tunnelGa.transform.localScale = new Vector3(5.1f, 5.1f, Vector3.Distance(line.endPoint.pos, tunnelStart) / 2);

                    }
                }
            }
            else
            {
                Debug.Log("请先设置综合监控模式下的掘进状态");
            }
            
        }
        /// <summary>
        /// 通过数据设置路径
        /// </summary>
        /// <param name="ways"></param>
        public GameObject wayGo;
        public bool editWay = false;
        public GameObject editTip;
        public void SetWays(string ways) {
            if (domMode == DomMode.lx)
            {
                if (!string.IsNullOrEmpty(ways))
                {
                    JObject waysTemp = JObject.Parse(ways);
                    WayData wayData = JsonUtility.FromJson<WayData>(waysTemp["ways"].ToString());
                    if (!string.IsNullOrEmpty(wayData.id))
                    {
                        if (wayPa.childCount > 0)
                        {
                            foreach (Transform item in wayPa)
                            {
                                DestroyImmediate(item.gameObject);
                            }
                        }
                        editWay = false;
                        editTip.SetActive(false);
                        Transform wayTypePa = null;
                        if (wayPa.Find(wayData.type.ToString()) == null)
                        {
                            wayTypePa = (new GameObject()).transform;
                            wayTypePa.parent = wayPa;
                            wayTypePa.name = wayData.type.ToString();
                            wayTypePa.localPosition = Vector3.zero;
                        }
                        else
                        {
                            wayTypePa = wayPa.Find(wayData.type.ToString());
                        }
                        Color newColor = new Color(0, 1, 0, 0.3f);
                        if (!string.IsNullOrEmpty(wayData.color))
                        {
                            Color nowColor = ColorToRGBA.GetColor(wayData.color);
                            newColor = new Color(nowColor.r, nowColor.g, nowColor.b, 0.3f);
                        }
                        for (int i = 0; i < wayData.points.Count - 1; i++)
                        {
                            Line line1 = tviewBase.transform.Find(dicLines[wayData.points[i].lineId].a_eCode).Find("Line_" + wayData.points[i].lineId).GetComponent<Line>();
                            Vector3 vFor1 = (line1.endPoint.pos - line1.startPoint.pos).normalized;
                            Vector3 vec1 = line1.startPoint.pos + vFor1 * wayData.points[i].distance;
                            Line line2 = tviewBase.transform.Find(dicLines[wayData.points[i + 1].lineId].a_eCode).Find("Line_" + wayData.points[i + 1].lineId).GetComponent<Line>();
                            Vector3 vFor2 = (line2.endPoint.pos - line2.startPoint.pos).normalized;
                            Vector3 vec2 = line2.startPoint.pos + vFor2 * wayData.points[i + 1].distance;
                            Vector3 eulerAngles = Quaternion.FromToRotation(Vector3.forward, vec2 - vec1).eulerAngles;
                            GameObject goWay = Instantiate(wayGo) as GameObject;
                            goWay.transform.parent = wayTypePa;
                            goWay.transform.localPosition = (vec1 + vec2) / 2;
                            goWay.transform.localEulerAngles = eulerAngles + new Vector3(0, 90, 0);
                            goWay.transform.localScale = new Vector3(Vector3.Distance(vec2, vec1) / 10, 1, 1);
                            goWay.GetComponent<MeshRenderer>().material = new Material(goWay.GetComponent<MeshRenderer>().material);
                            goWay.GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(Vector3.Distance(vec2, vec1) / 10, 1));
                            DOTween.To(() => goWay.GetComponent<MeshRenderer>().material.mainTextureOffset, x =>
                            goWay.GetComponent<MeshRenderer>().material.mainTextureOffset = x, new Vector2(-1, 0), 1).SetLoops(-1).SetEase(Ease.Linear);
                            goWay.GetComponent<MeshRenderer>().material.color = newColor;
                        }
                        if (wayTypePa.childCount > 0)
                        {
                            Camera.main.GetComponent<BLCameraControl>().LookAtResAutoDis(wayTypePa);
                        }
                        else
                        {
                            Debug.Log("直接进入编辑模式，此编辑模式不分路线类型");
                            Camera.main.GetComponent<BLCameraControl>().LookAtResAutoDis(tviewBase.transform);
                            editTip.SetActive(true);
                            editWay = true;
                        }
                    }
                    else
                    {
                        Debug.Log("直接进入编辑模式，此编辑模式不分路线类型");
                        Camera.main.GetComponent<BLCameraControl>().LookAtResAutoDis(tviewBase.transform);
                        editTip.SetActive(true);
                        editWay = true;
                    }
                }
                else
                {
                    Debug.Log("直接进入编辑模式，此编辑模式不分路线类型");
                    Camera.main.GetComponent<BLCameraControl>().LookAtResAutoDis(tviewBase.transform);
                    editTip.SetActive(true);
                    editWay = true;
                }
            }
            else
            {
                Debug.Log("请先进入路线模式");
            }
            
        }
        /// <summary>
        /// 设置巡检
        /// </summary>
        /// <param name="inspacetion"></param>
        public Transform inspectionPa;
        public bool startInspection = false;
        public void SetInspection(string inspacetion) {
            if (domMode == DomMode.xj && !string.IsNullOrEmpty(inspacetion))
            {
                inspacetion = "{\"insp\":" + inspacetion + "}";
                Inspection _inspaction = JsonUtility.FromJson<Inspection>(inspacetion);
                if (_inspaction.insp.Count > 0)
                {
                    foreach (Transform item in inspectionPa)
                    {
                        DestroyImmediate(item.gameObject);
                    }
                    for (int i = 0; i < _inspaction.insp.Count; i++)
                    {
                        GameObject goInspaction = new GameObject();
                        goInspaction.transform.parent = inspectionPa;
                        Line line = tviewBase.transform.Find(dicLines[_inspaction.insp[i].lineId].a_eCode).Find("Line_" + _inspaction.insp[i].lineId).GetComponent<Line>();
                        Vector3 vFor = (line.endPoint.pos - line.startPoint.pos).normalized;
                        Vector3 vec = line.startPoint.pos + vFor * _inspaction.insp[i].distance;
                        goInspaction.transform.localPosition = vec;
                        goInspaction.name = "Inspection_" + i.ToString();
                    }
                    startInspection = true;
                    InvokeRepeating("InspectionUpdate", 1, 2);
                }
                else
                {
                    InspactionEdit();
                }
            }
            else
            {
                Debug.Log("请设置巡检模式再进行巡检");
            }
        }

        public void AdjustRange(int rate) {
            if (nowElement != null)
            {
                if (nowElement.pian.activeInHierarchy)
                {
                    nowElement.ChangePianRadius(rate);
                }
            }
        }
        int num = 0;
        private void InspectionUpdate() {
            if (domMode == DomMode.xj && startInspection)
            {
                if (inspectionPa.childCount > 0)
                {
                    if (num < inspectionPa.childCount)
                    {
                        Camera.main.GetComponent<BLCameraControl>().LookAtPosition(inspectionPa.GetChild(num).position);
                        StartCoroutine(WaitInspection());
                        startInspection = false;
                    }
                    else
                    {
                        num = 0;
                    }
                }
            }
        }
        IEnumerator WaitInspection()
        {
            yield return new WaitForSeconds(1f);
            num++;
            startInspection = true;
        }
        private void InspactionEdit() {
            Debug.Log("直接进入巡览编辑模式");
            Camera.main.GetComponent<BLCameraControl>().LookAtResAutoDis(tviewBase.transform);
            editTip.SetActive(true);
            editWay = true;
        }
        /// <summary>
        /// 通过数据删除点位
        /// </summary>
        /// <param name="pointMsg"></param>
        public void DeletePoint(string pointMsg) {
            if (editWay)
            {
                if (!string.IsNullOrEmpty(pointMsg))
                {
                    Points points = JsonUtility.FromJson<Points>(pointMsg);
                    if (wayPa.Find("point_"+points.lineId + "_" + points.distance) != null)
                    {
                        wayPa.GetComponent<WayPoints>().dicPoints.Remove("point_" + points.lineId + "_" + points.distance);
                        DestroyImmediate(wayPa.Find("point_" + points.lineId + "_" + points.distance).gameObject);
                    }
                    else
                    {
                        Debug.Log("当前点位不存在");
                    }
                }
            }
            else
            {
                Debug.Log("请进入编辑模式");
            }
        }
        /// <summary>
        /// 设置当前模式
        /// </summary>
        /// <param name="mode">0-综合监控，1-巡检，2-路线，3-2D，4-综合监控模式下的掘进进度，5-综合监控模式下的区域选择 </param>
        public void SetDominantMode(int mode) {
            editTip.SetActive(false);
            domMode = (DomMode)mode;
            switch (mode)
            {
                case 0:
                    listType.Clear();
                    foreach (Transform item in lineLabelParent)
                    {
                        if (!listType.Contains(item.name))
                        {
                            listType.Add(item.name);
                        }
                    }
                    foreach (KeyValuePair<string, List<ElementItem>> item in dicElements)
                    {
                        foreach (ElementItem itemChild in item.Value)
                        {
                            itemChild.gameObject.SetActive(true);
                        }
                    }
                    foreach (KeyValuePair<string, List<Transform>> item in dicAreaLines)
                    {
                        if (item.Value != null)
                        {
                            foreach (Transform itemGo in item.Value)
                            {
                                itemGo.GetChild(0).GetComponent<MeshRenderer>().material = itemGo.GetComponent<Line>().dataSetMat;
                            }
                        }
                    }
                    Camera.main.GetComponent<BLCameraControl>().LookAtResAutoDis(tviewBase.transform);
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    foreach (KeyValuePair<string, List<ElementItem>> item in dicElements)
                    {
                        foreach (ElementItem itemChild in item.Value)
                        {
                            itemChild.gameObject.SetActive(true);
                        }
                    }
                    foreach (KeyValuePair<string, List<Transform>> item in dicAreaLines)
                    {
                        foreach (Transform itemGo in item.Value)
                        {
                            itemGo.GetChild(0).GetComponent<MeshRenderer>().material = itemGo.GetComponent<Line>().dataSetMat;
                        }
                    }
                    Camera.main.GetComponent<BLCameraControl>().LookAtResAutoDisS(tviewBase.transform);
                    break;
                case 4:
                    break;
                case 5:
                   
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 显示隐藏地上
        /// </summary>
        /// <param name="result"></param>
        public void ShowGround(string result) {
            MainManager.GetInstance().overground.transform.position = tviewBase.transform.position;
            string[] ground = result.Split('+');
            if (ground[0] == "1")
            {
                MainManager.GetInstance().overground.SetActive(true);
                tviewBase.gameObject.SetActive(false);
                Camera.main.GetComponent<BLCameraControl>().LookAtUpGround3(MainManager.GetInstance().overground.transform);
            }
            if (ground[0] == "0")
            {
                foreach (KeyValuePair<string, List<ElementItem>> item in dicElements)
                {
                    foreach (ElementItem itemChild in item.Value)
                    {
                        itemChild.gameObject.SetActive(true);
                    }
                }
                foreach (KeyValuePair<string, List<Transform>> item in dicAreaLines)
                {
                    foreach (Transform itemGo in item.Value)
                    {
                        itemGo.GetChild(0).GetComponent<MeshRenderer>().material = itemGo.GetComponent<Line>().dataSetMat;
                    }
                }
                tviewBase.gameObject.SetActive(true);
                MainManager.GetInstance().overground.SetActive(false);
                Camera.main.GetComponent<BLCameraControl>().LookAtResAutoDis(tviewBase.transform);
            }
            if (ground.Length > 1)
            {
                if (ground[1] == "1")
                {
                    MainManager.GetInstance().overground.SetActive(true);
                }
                if (ground[1] == "0")
                {
                    tviewBase.gameObject.SetActive(true);
                }
            }
        }
        public bool isCurrent = false;
        /// <summary>
        /// 框选
        /// </summary>
        /// <param name="current"></param>
        public void CurrentRes(int current) {
            if (current == 0)
            {
                isCurrent = true;
            }
            else
            {
                isCurrent = false;
            }
        }
        private Track trackMsg;
        public Dictionary<GameObject, bool> dicTrack = new Dictionary<GameObject, bool>();
        /// <summary>
        /// 添加回访点位
        /// </summary>
        /// <param name="track"></param>
        public void AddTrack(string track) {
            if (!DynamicElement.GetInstance().inPlay)
            {
                if (!string.IsNullOrEmpty(track))
                {
                    trackMsg = JsonUtility.FromJson<Track>(track);
                    DynamicElement.GetInstance().nowTrack = trackMsg;
                    foreach (Transform itemAll in lineLabelParent)
                    {
                        if (!dicTrack.ContainsKey(itemAll.gameObject))
                        {
                            dicTrack.Add(itemAll.gameObject, itemAll.gameObject.activeInHierarchy);
                        }
                        else
                        {
                            dicTrack[itemAll.gameObject] = itemAll.gameObject.activeInHierarchy;
                        }
                        foreach (Transform itemChildAll in itemAll)
                        {
                            if (!dicTrack.ContainsKey(itemChildAll.gameObject))
                            {
                                dicTrack.Add(itemChildAll.gameObject, itemChildAll.gameObject.activeInHierarchy);
                            }
                            else
                            {
                                dicTrack[itemChildAll.gameObject] = itemChildAll.gameObject.activeInHierarchy;
                            }
                        }
                    }
                    foreach (Transform itemTrans in lineLabelParent)
                    {
                        if (itemTrans.name != trackMsg.t_eCode)
                        {
                            itemTrans.gameObject.SetActive(false);
                        }
                        else
                        {
                            foreach (Transform itemChild in itemTrans)
                            {
                                if (itemChild.name != trackMsg.id)
                                {
                                    itemChild.gameObject.SetActive(false);
                                }
                            }
                        }
                    }
                    foreach (TrackPoints item in trackMsg.track)
                    {
                        Line lineTemp = GameObject.Find("Line_" + item.lineId).GetComponent<Line>();
                        Vector3 vForTemp = (lineTemp.endPoint.pos - lineTemp.startPoint.pos).normalized;
                        if (item.distance < lineTemp.lineLenght)
                        {
                            DynamicElement.GetInstance().SetPointPosition(lineTemp.startPoint.pos + vForTemp * item.distance, item.lineId);
                        }
                    }
                    foreach (Transform itemArray in arrayLabelParent)
                    {
                        if (!dicTrack.ContainsKey(itemArray.gameObject))
                        {
                            dicTrack.Add(itemArray.gameObject, itemArray.gameObject.activeInHierarchy);
                        }
                        else
                        {
                            dicTrack[itemArray.gameObject] = itemArray.gameObject.activeInHierarchy;
                        }
                    }
                    if (lineLabelParent.Find(trackMsg.t_eCode) == null)
                    {
                        GameObject goPaTemp = new GameObject();
                        goPaTemp.transform.SetParent(lineLabelParent);
                        goPaTemp.transform.localPosition = Vector3.zero;
                        goPaTemp.name = trackMsg.t_eCode;
                    }
                    if (lineLabelParent.Find(trackMsg.t_eCode + "/" + trackMsg.id) != null)
                    {
                        Transform trackTemp = lineLabelParent.Find(trackMsg.t_eCode + "/" + trackMsg.id);
                        foreach (KeyValuePair<string, List<Transform>> item in dicLineLabels)
                        {
                            if (item.Value.Contains(trackTemp))
                            {
                                if (arrayLabelParent.Find(item.Key) != null)
                                {
                                    arrayLabelParent.Find(item.Key).gameObject.SetActive(false);
                                }
                                foreach (Transform itemChild in item.Value)
                                {
                                    if (itemChild.name != trackMsg.id)
                                    {
                                        itemChild.gameObject.SetActive(false);
                                    }
                                }
                            }
                        }
                        trackTemp.GetComponent<ElementItem>().SetUIByLevel(1);
                        DynamicElement.GetInstance().SetTarget(trackTemp, trackTemp.position);
                        trackTemp.gameObject.SetActive(true);
                        trackTemp.GetComponent<ElementItem>().SetTrackData(trackMsg, false, trackMsg.track[0].data);
                    }
                    else
                    {
                        if (nowElement != null)
                        {
                            nowElement.SetUIByLevel(0);
                            SceneToLevel(1);
                        }
                        Transform goItemPa = lineLabelParent.Find(trackMsg.t_eCode);
                        GameObject goItem = Instantiate(P_ElementItem) as GameObject;
                        Line line = tviewBase.transform.Find(dicLines[trackMsg.track[0].lineId].a_eCode + "/Line_"
                                        + trackMsg.track[0].lineId).GetComponent<Line>();
                        Vector3 vFor = (line.endPoint.pos - line.startPoint.pos).normalized;
                        goItem.transform.parent = goItemPa;
                        goItem.name = trackMsg.id;
                        goItem.transform.localPosition = line.startPoint.pos + vFor * trackMsg.track[0].distance;
                        goItem.GetComponent<ElementItem>().nowState = 0;
                        DynamicElement.GetInstance().SetTarget(goItem.transform, Vector3.zero, false);
                        goItem.GetComponent<ElementItem>().SetUIByLevel(1);
                        goItem.gameObject.SetActive(true);
                        goItem.GetComponent<ElementItem>().SetTrackData(trackMsg, false, trackMsg.track[0].data);
                        goItem.GetComponent<ElementItem>().circle.SetActive(true);
                        foreach (Transform goTemp in goItemPa)
                        {
                            if (goTemp != goItem.transform)
                            {
                                goTemp.gameObject.SetActive(false);
                            }
                        }
                    }

                    foreach (Transform item in arrayLabelParent)
                    {
                        item.gameObject.SetActive(false);
                    }
                }
                Invoke("play", 0.5f);
            }
        }
        /// <summary>
        /// 设置轨迹回放速度
        /// </summary>
        /// <param name="speed"></param>
        public void SetTrackSpeed(float speed) {
            if (speed != 0)
            {
                DynamicElement.GetInstance().moveSpeed = speed;
            }
        }
        /// <summary>
        /// 设置轨迹回放进度
        /// </summary>
        /// <param name="degree"></param>
        public void SetTrackDegree(float degree) {
            DynamicElement.GetInstance().SetTrackDegree(degree);
        }
        /// <summary>
        /// 设置动画开始和暂停
        /// </summary>
        /// <param name="isPlay"></param>
        public void SetTrackPlay(int isPlay) {
            if (DynamicElement.GetInstance().inPlay)
            {
                DynamicElement.GetInstance().isPlay = isPlay == 0 ? true : false;
            }
        }
        private void play() {
            SetTrackPlay(0);
        }
        /// <summary>
        /// 将object对象转换为实体对象
        /// </summary>
        /// <typeparam name="T">实体对象类名</typeparam>
        /// <param name="asObject">object对象</param>
        /// <returns></returns>
        private T ConvertObject<T>(object asObject) where T : new()
        {
            //创建实体对象实例
            var t = Activator.CreateInstance<T>();
            if (asObject != null)
            {
                Type type = asObject.GetType();
                //遍历实体对象属性
                foreach (var info in typeof(T).GetProperties())
                {
                    object obj = null;
                    //取得object对象中此属性的值
                    var val = type.GetProperty(info.Name)?.GetValue(asObject);
                    if (val != null)
                    {
                        //非泛型
                        if (!info.PropertyType.IsGenericType)
                            obj = Convert.ChangeType(val, info.PropertyType);
                        else//泛型Nullable<>
                        {
                            Type genericTypeDefinition = info.PropertyType.GetGenericTypeDefinition();
                            if (genericTypeDefinition == typeof(Nullable<>))
                            {
                                obj = Convert.ChangeType(val, Nullable.GetUnderlyingType(info.PropertyType));
                            }
                            else
                            {
                                obj = Convert.ChangeType(val, info.PropertyType);
                            }
                        }
                        info.SetValue(t, obj, null);
                    }
                }
            }
            return t;
        }
    }
    
    [System.Serializable]
    public class LineData {
        public string id;
        public string l_eCode;
        public float length;
        public string a_eCode;
    }
    #region 轨迹回放
    [Serializable]
    public class Track {
        public string id;
        public List<IndexData> data;//指标
        public string t_eCode;
        public List<TrackPoints> track;
    }
    [Serializable]
    public class TrackPoints {
        public string lineId;
        public float distance;
        public List<IndexData> data;
    }
    #endregion
    #region  路径解析
    [System.Serializable]
    public class WayData {
        public string id;
        public string name;
        public int type;
        public string color;
        public List<Points> points;
    }
    [System.Serializable]
    public class Points {
        public string lineId;
        public float distance;
    }
    #endregion
    #region 巡检
    [Serializable]
    public class Inspection {
        public List<Points> insp;
    }
    #endregion
    #region config 配置
    [System.Serializable]
    public class ConfigData {
        public List<ConfigItem> area;
        public List<ConfigItem> line;
        public List<ConfigType> eType;
    }
    [System.Serializable]
    public class ConfigItem {
        public string eCode;
        public string name;
    }
    [System.Serializable]
    public class ConfigType {
        public string eCode;
        public string name;
        public TypeData data;
    }
    [System.Serializable]
    public class TypeData {
        public string icon;
        public string color;
    }
    #endregion
    #region 资源实体解析
    [Serializable]
    public class Entity {
        public string id;//资源的id
        public string t_eCode;//资源类型编号
        public string name;//资源名称
        public int a_type;//资源的位置0-地下，1-地上
        public int state;//资源状态 0-正常，1,2,3,4...不正常
        public StateData s_data;
        public int exist;//资源操作 0-刷新，1-删除
        public int display;//资源是否显示 0-显示，1-不显示
        public Position position;//资源位置
        public List<IndexData> data;
    }
    [System.Serializable]
    public class StateData {
        public string icon;
        public string color;
        public float range;
        public float rate;
    }
    [System.Serializable]
    public class PositionContent
    {
        public string lineId;
        public float distance;
    }
    [System.Serializable]
    public class Position
    {
        public string lineId;
        public List<PositionContent> content;
        public float distance;
    }
    [Serializable]
    public class IndexData {
        public string key;
        public float keyfontsize;
        public string keyfontcolor;
        public string value;
        public float valuefontsize;
        public string valuefontcolor;
    }
    #endregion
    #region 资源类型
    [Serializable]
    public class ResTypes {
        public List<string> list;
    }
    #endregion
    #region 掘进
    [Serializable]
    public class TunnelList {
        public List<Tunnel> list;
    }
    [Serializable]
    public class Tunnel {
        public string lineId;
        public float tunnelDegree;
    }
    #endregion
    #region 发送cad信息
    [Serializable]
    public class cadMessage{
        public string lineId;
        public float length;
    }
    #endregion
    #region 巷道图标限制
    [Serializable]
    public class LimitConfig {
        public string icon;
        public string color;
        public int perDis;
    }
    #endregion
    #region cad等的初始化配置
    [Serializable]
    public class InitConfig {
        public string cadURL;
        public string cadColor;
        public LimitConfig limit;
        public string background;
    }
    #endregion
}

