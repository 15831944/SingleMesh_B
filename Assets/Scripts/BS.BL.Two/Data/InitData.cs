using BS.BL.Element;
using BS.BL.Two.Element;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BS.BL.Two {
    public class InitData : MonoBehaviour,IManager
    {
        public void Init()
        {
            throw new System.NotImplementedException();
        }

        public void InitBasicData(Element.ElementContainer elementContainer, string result) {
            if (!string.IsNullOrEmpty(result))
            {
                JObject basicData = JObject.Parse(result);
                //巷道的线
                foreach (JObject itemLine in basicData["lines"])
                {
                    Element.LineData lineData = JsonUtility.FromJson<Element.LineData>(itemLine.ToString());
                    Transform tsTemp = elementContainer.tviewBase.transform.Find("Layer_HD/Lines/Line_" + lineData.id);
                    //添加区域的组
                    if (elementContainer.dicAreaLines.ContainsKey(!string.IsNullOrEmpty(lineData.a_eCode) ? lineData.a_eCode : "other"))
                    {
                        elementContainer.dicAreaLines[!string.IsNullOrEmpty(lineData.a_eCode) ? lineData.a_eCode : "other"].Add(tsTemp);
                        try
                        {
                            tsTemp.parent = elementContainer.tviewBase.transform.Find(!string.IsNullOrEmpty(lineData.a_eCode) ? lineData.a_eCode : "other");
                        }
                        catch (System.Exception E)
                        {

                            Debug.Log("区域id" + lineData.a_eCode + "线段的id" + lineData.id);
                        }
                    }
                    else
                    {
                        GameObject goArea = null;
                        if (elementContainer.tviewBase.transform.Find(!string.IsNullOrEmpty(lineData.a_eCode) ? lineData.a_eCode : "other") == null)
                        {
                            goArea = new GameObject();
                            goArea.name = !string.IsNullOrEmpty(lineData.a_eCode) ? lineData.a_eCode : "other";
                            goArea.transform.parent = elementContainer.tviewBase.transform;
                            goArea.transform.localPosition = Vector3.zero;
                        }
                        else
                        {
                            goArea = elementContainer.tviewBase.transform.Find(!string.IsNullOrEmpty(lineData.a_eCode) ? lineData.a_eCode : "other").gameObject;
                        }
                        List<Transform> listGoTemp = new List<Transform>();
                        listGoTemp.Add(tsTemp);
                        elementContainer.dicAreaLines.Add(!string.IsNullOrEmpty(lineData.a_eCode) ? lineData.a_eCode : "other", listGoTemp);
                        tsTemp.parent = goArea.transform;
                    }
                    //添加巷道名称的组
                    if (elementContainer.dicNameLines.ContainsKey(!string.IsNullOrEmpty(lineData.l_eCode) ? lineData.l_eCode : "noName"))
                    {
                        elementContainer.dicNameLines[!string.IsNullOrEmpty(lineData.l_eCode) ? lineData.l_eCode : "noName"].Add(tsTemp);
                    }
                    else
                    {
                        List<Transform> listGoTemp = new List<Transform>();
                        listGoTemp.Add(tsTemp);
                        elementContainer.dicNameLines.Add(!string.IsNullOrEmpty(lineData.l_eCode) ? lineData.l_eCode : "noName", listGoTemp);
                    }
                    if (elementContainer.dicLines.ContainsKey(lineData.id))
                    {
                        if (string.IsNullOrEmpty(lineData.a_eCode))
                        {
                            lineData.a_eCode = "other";
                        }
                        elementContainer.dicLines[lineData.id] = lineData;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(lineData.a_eCode))
                        {
                            lineData.a_eCode = "other";
                        }
                        elementContainer.dicLines.Add(lineData.id, lineData);
                    }
                }
                //路径
                foreach (JObject itemWay in basicData["ways"])
                {
                    WayData wayData = JsonUtility.FromJson<WayData>(itemWay.ToString());
                    if (elementContainer.dicWays.ContainsKey(wayData.type))
                    {
                        elementContainer.dicWays[wayData.type].Add(wayData);
                    }
                    else
                    {
                        List<WayData> wayDatas = new List<WayData>();
                        wayDatas.Add(wayData);
                        elementContainer.dicWays.Add(wayData.type, wayDatas);
                    }
                }
                //资源类型
                ConfigData configData = JsonUtility.FromJson<ConfigData>(basicData["config"].ToString());
                foreach (ConfigItem itemArea in configData.area)
                {
                    if (!elementContainer.dicAreaName.ContainsKey(itemArea.eCode))
                    {
                        elementContainer.dicAreaName.Add(itemArea.eCode, itemArea.name);
                    }
                }
                foreach (ConfigItem itemLine in configData.line)
                {
                    if (!elementContainer.dicLineName.ContainsKey(itemLine.eCode))
                    {
                        elementContainer.dicLineName.Add(itemLine.eCode, itemLine.name);
                    }
                }
                foreach (ConfigType itemType in configData.eType)
                {
                    StartCoroutine(elementContainer.loadConfig.LoadPhoto(elementContainer.diceTypeSprites, itemType));
                    if (!elementContainer.diceType.ContainsKey(itemType.eCode))
                    {
                        elementContainer.diceType.Add(itemType.eCode, itemType);
                    }
                }
            }
        }
    }
}

