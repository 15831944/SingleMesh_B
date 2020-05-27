using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Newtonsoft.Json.Linq;
using BS.BL.Interface;
using UnityEngine.Networking;
using BS.BL.Two.Loader;
using System.Text;

namespace BS.BL.Two.Element {
    public class ElementItem : MonoBehaviour
    {
        public SpriteRenderer iconSR, tuoSP, photoSP;//资源头像，资源头像底托，资源指标头像
        public GameObject first, second;
        public Transform line, indexPanel;
        public GameObject P_text;
        public Transform textParent;
        public Transform index;
        public string lineId;
        public string t_eType;
        public float indexH = 1f;
        public GameObject pian;
        public int nowState = 0;
        private Color elementColor;
        private float scale_x = 2f, scale_z = 2f;
        private float rate = 10f;


        public Transform arrayTrans = null;
        public GameObject tuo, circle;

        public Entity resEntityTemp;
        public int nowLevel = 0;

        public void Select() {
            Debug.Log("选择" + transform.name);
            if (!ElementContainer.GetInstance().listSelectItems.Contains(this))
            {
                ElementContainer.GetInstance().listSelectItems.Add(this);
            }
        }
        public void DisSelect() {
            Debug.Log("取消选择" + transform.name);
            if (ElementContainer.GetInstance().listSelectItems.Contains(this))
            {
                ElementContainer.GetInstance().listSelectItems.Remove(this);
            }
        }

        //private void OnDisable()
        //{
        //    SetUIByLevel(0);
        //}
        /// <summary>
        /// 根据当前层级显示UI
        /// </summary>
        /// <param name="level">0-第一层级（默认层级），1-第二层级</param>
        public void SetUIByLevel(int level,bool zhankai = false)
        {
            switch (level)
            {
                case 0:
                    Debug.Log("显示第一层级");
                    pian.SetActive(false);
                    tuo.SetActive(true);
                    textParent.gameObject.SetActive(false);
                    indexPanel.DOScaleY(0, 0f).SetEase(Ease.Linear).OnComplete(delegate() {
                        first.transform.Find("Icon").DOLocalMoveY(0.45f, 0f);
                        line.DOScaleY(0, 0f).SetEase(Ease.Linear).OnComplete(delegate ()
                        {
                            second.SetActive(false);
                            first.SetActive(true);
                        });
                    });
                    ElementContainer.GetInstance().nowElement = null;
                    Camera.main.GetComponent<BLCameraControl>().SetZoomLimit(0, 2500);
                    circle.SetActive(false);
                    if (nowLevel == 0)
                    {
                        return;
                    }
                    if (!zhankai)
                    {
                        GameObject.Find("JSInterface").GetComponent<JSInterface>().unSelectRes(transform.name);
                    }
                    break;
                case 1:
                    Debug.Log("显示第二层级");
                    ElementContainer.GetInstance().isClick = false;
                    if (ElementContainer.GetInstance().nowElement != null)
                    {
                        if (ElementContainer.GetInstance().nowElement != this)
                        {
                            ElementContainer.GetInstance().nowElement.SetUIByLevel(0);
                        }
                    }
                    ElementContainer.GetInstance().nowElement = null;
                    first.SetActive(true);
                    second.SetActive(true);
                    if (nowState == 0)
                    {
                        circle.SetActive(true);
                        pian.SetActive(false);
                    }
                    else
                    {
                        circle.gameObject.SetActive(false);
                        pian.SetActive(true);
                        pian.GetComponent<MeshRenderer>().material.color = elementColor;
                    }
                    tuo.SetActive(false);
                    first.transform.Find("Icon").DOLocalMoveY(1.3f, 0.2f);
                    line.DOScaleY(0.9f, 0.2f).SetEase(Ease.Linear).OnComplete(delegate () {
                        indexPanel.DOScaleY(indexH, 0.2f).SetEase(Ease.Linear).OnComplete(delegate() {
                            textParent.gameObject.SetActive(true);
                            ElementContainer.GetInstance().SceneToLevel(2);
                            ElementContainer.GetInstance().nowElement = this;
                            ElementContainer.GetInstance().isClick = true;
                        });
                    });
                    Camera.main.GetComponent<BLCameraControl>().LookAtPosition2(transform.position);
                    if (nowLevel == 1)
                    {
                        return;
                    }
                    string calljs = "{\"resId\":\"" + transform.name + "\",\"t_eCode\":\"" + t_eType + "\"}";
                    GameObject.Find("JSInterface").GetComponent<JSInterface>().selectCurrentRes(calljs);
                    break;
                default:
                    break;
            }
            nowLevel = level;
        }
        private void Update()
        {
            if (first.active)
            {
                first.transform.LookAt(Camera.main.transform);
            }
            if (second.active)
            {
                indexPanel.LookAt(Camera.main.transform);
                second.transform.Find("Line").LookAt(Camera.main.transform);
            }
        }
        Tweener tw1 = null, tw2 = null, tw3 = null,tw4 = null;
        Tweener tws1 = null, tws2 = null, tws3 = null, tws4 = null, tws5 = null, tws6 = null, tws7 = null, tws8 = null;
        float numTemp = 1.8f;
        private int textSize = 0;
        public void Set(Entity resEntity,bool refresh = false,bool isPlay = false) {
            resEntityTemp = resEntity;
            t_eType = resEntity.t_eCode;
            lineId = resEntity.position.lineId;
            nowState = resEntity.state;
            pian.SetActive(false);
            Material matPian = new Material(pian.GetComponent<MeshRenderer>().material);
            pian.GetComponent<MeshRenderer>().material = matPian;
            if (resEntity.display == 1)
            {
                gameObject.SetActive(false);
            }
            if (resEntity.state == 0)
            {
                int dataCount = resEntity.data.Count;
                for (int i = resEntity.data.Count - 1; i >= 0; i--)
                {
                    GameObject goText = null;
                    if (textParent.Find(i.ToString()) != null)
                    {
                        if (isPlay)
                        {
                            goText = textParent.GetChild(dataCount - 1 - i).gameObject;
                        }
                        else
                        {
                            goText = textParent.GetChild(dataCount - 1 - i).gameObject;
                        }
                    }
                    else
                    {
                        numTemp += 0.25f;
                        goText = Instantiate(P_text) as GameObject;
                        goText.transform.SetParent(textParent);
                        goText.transform.localPosition = new Vector3(0, numTemp - 0.2f, 0);
                        goText.transform.localScale = new Vector3(1, 1, 1);
                        goText.transform.localEulerAngles = Vector3.zero;
                        indexH = numTemp - 1.8f;
                        goText.name = i.ToString();
                    }
                    
                }
               
                Invoke("SetTextMeshPro",0.1f);
            }
            if (resEntity.data.Count < textParent.childCount)
            {
                for (int i = resEntity.data.Count; i < textParent.childCount; i++)
                {
                    numTemp -= 0.25f;
                    DestroyImmediate(textParent.GetChild(i).gameObject);
                }
            }
            if (ElementContainer.GetInstance().diceTypeSprites.ContainsKey(resEntity.t_eCode))
            {
                iconSR.sprite = ElementContainer.GetInstance().diceTypeSprites[resEntity.t_eCode];
            }
            Color nowColor;
            ColorUtility.TryParseHtmlString(ElementContainer.GetInstance().diceType[resEntity.t_eCode].data.color, out nowColor);
            elementColor = new Color(nowColor.r, nowColor.g, nowColor.b, 0.4f);
            Color color1 = new Color(nowColor.r, nowColor.g, nowColor.b, 1);
            Color color2 = new Color(nowColor.r, nowColor.g, nowColor.b, 1);
            Color color3 = new Color(nowColor.r, nowColor.g, nowColor.b, 1);
            Color color4 = new Color(nowColor.r, nowColor.g, nowColor.b, 1);
            tuo.GetComponent<SpriteRenderer>().color = color4;
            transform.Find("Circle/1").GetComponent<MeshRenderer>().material.color = color1;
            transform.Find("Circle/2").GetComponent<MeshRenderer>().material.color = color2;
            transform.Find("Circle/3").GetComponent<MeshRenderer>().material.color = color3;
            second.transform.Find("Line/line").GetComponent<SpriteRenderer>().color = nowColor;
            if (refresh)
            {
                if (ElementContainer.GetInstance().nowElement == this && nowState != 0)
                {
                    pian.SetActive(true);
                }
                if (tw1 != null) tw1.Kill();
                if (tw2 != null) tw2.Kill();
                if (tw3 != null) tw3.Kill();
                if (tw4 != null) tw4.Kill();
                if (tws1 != null) tws1.Kill();
                if (tws2 != null) tws2.Kill();
                if (tws3 != null) tws3.Kill();
                if (tws4 != null) tws4.Kill(); 
                if (tws5 != null) tws5.Kill();
                if (tws6 != null) tws6.Kill();
                if (tws7 != null) tws7.Kill();
                if (tws8 != null) tws8.Kill();
                tws1 = transform.Find("Circle/1").DOScaleX(1, 1).SetLoops(-1).SetEase(Ease.Linear).OnKill(delegate() { transform.Find("Circle/1").localScale = new Vector3(0, 0.00001f, 0); });
                tws2 = transform.Find("Circle/1").DOScaleZ(1, 1).SetLoops(-1).SetEase(Ease.Linear); 
                tws3 = transform.Find("Circle/2").DOScaleX(2F / 3F, 1).SetLoops(-1).SetEase(Ease.Linear).OnKill(delegate () { transform.Find("Circle/2").localScale = new Vector3(0, 0.00001f, 0); });
                tws4 = transform.Find("Circle/2").DOScaleZ(2F / 3F, 1).SetLoops(-1).SetEase(Ease.Linear); 
                tws5 = transform.Find("Circle/3").DOScaleX(1F / 3F, 1).SetLoops(-1).SetEase(Ease.Linear).OnKill(delegate () { transform.Find("Circle/3").localScale = new Vector3(0, 0.00001f, 0); });
                tws6 = transform.Find("Circle/3").DOScaleZ(1F / 3F, 1).SetLoops(-1).SetEase(Ease.Linear);
                tw1 = DOTween.To(() => transform.Find("Circle/1").GetComponent<MeshRenderer>().material.color, x =>
                                 transform.Find("Circle/1").GetComponent<MeshRenderer>().material.color = x, 
                                 new Color(color1.r, color1.g, color1.b, 0), 1).SetLoops(-1).SetEase(Ease.Linear).OnKill(delegate () {
                                     transform.Find("Circle/1").GetComponent<MeshRenderer>().material.color = color1;
                                 }); 
                tw2 = DOTween.To(() => transform.Find("Circle/2").GetComponent<MeshRenderer>().material.color, x =>
                            transform.Find("Circle/2").GetComponent<MeshRenderer>().material.color = x, 
                            new Color(color2.r, color2.g, color2.b, 0), 1).SetLoops(-1).SetEase(Ease.Linear).OnKill(delegate () {
                                transform.Find("Circle/2").GetComponent<MeshRenderer>().material.color = color2;
                            });
                tw3 = DOTween.To(() => transform.Find("Circle/3").GetComponent<MeshRenderer>().material.color, x =>
                                transform.Find("Circle/3").GetComponent<MeshRenderer>().material.color = x,
                                new Color(color3.r, color3.g, color3.b, 0), 1).SetLoops(-1).SetEase(Ease.Linear).OnKill(delegate () {
                                    transform.Find("Circle/3").GetComponent<MeshRenderer>().material.color = color3;
                                });
                tw4 = DOTween.To(() => tuo.GetComponent<SpriteRenderer>().color, x =>
                                 tuo.GetComponent<SpriteRenderer>().color = x, 
                                 new Color(color4.r, color4.g, color4.b, 0.3f), 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).OnKill(delegate () {
                                     tuo.GetComponent<SpriteRenderer>().color = color4;
                                 });
            }
            if (resEntity.state != 0)
            {
                AlarmState();
                if (resEntity.s_data.rate != 0)
                {
                    rate = resEntity.s_data.rate;
                }
                if (resEntity.s_data.range != 0)
                {
                    if (tws1 != null) tws1.Kill();
                    if (tws2 != null) tws2.Kill();
                    if (tws3 != null) tws3.Kill();
                    if (tws4 != null) tws4.Kill();
                    if (tws5 != null) tws5.Kill();
                    if (tws6 != null) tws6.Kill();
                    if (tws7 != null) tws7.Kill();
                    if (tws8 != null) tws8.Kill();
                    scale_x = resEntity.s_data.range / 25f;
                    scale_z = resEntity.s_data.range / 25f;
                }
                tws7 = pian.transform.DOScaleX(scale_x, 0).SetEase(Ease.Linear);
                tws8 = pian.transform.DOScaleZ(scale_z, 0).SetEase(Ease.Linear);
                if (!string.IsNullOrEmpty(resEntity.s_data.icon) && !string.IsNullOrEmpty(resEntity.s_data.color))
                {
                    if (gameObject.activeInHierarchy)
                    {
                        StartCoroutine(ElementContainer.GetInstance().loadConfig.LoadPhoto(resEntity.s_data.icon, LoadSprite));
                    }
                    if (tw1 != null) tw1.Kill();
                    if (tw2 != null) tw2.Kill();
                    if (tw3 != null) tw3.Kill();
                    if (tw4 != null) tw4.Kill();
                    Color nowColor2;
                    ColorUtility.TryParseHtmlString(resEntity.s_data.color, out nowColor2);
                    Color color1_1 = new Color(nowColor2.r, nowColor2.g, nowColor2.b, 1);
                    Color color2_1 = new Color(nowColor2.r, nowColor2.g, nowColor2.b, 1);
                    Color color3_1 = new Color(nowColor2.r, nowColor2.g, nowColor2.b, 1);
                    Color color4_1 = new Color(nowColor2.r, nowColor2.g, nowColor2.b, 1);
                    tuo.GetComponent<SpriteRenderer>().color = color4_1;
                    transform.Find("Circle/1").GetComponent<MeshRenderer>().material.color = color1_1;
                    transform.Find("Circle/2").GetComponent<MeshRenderer>().material.color = color2_1;
                    transform.Find("Circle/3").GetComponent<MeshRenderer>().material.color = color3_1;

                    second.transform.Find("Line/line").GetComponent<SpriteRenderer>().color = nowColor2;
                    tw1 = DOTween.To(() => transform.Find("Circle/1").GetComponent<MeshRenderer>().material.color, x =>
                                transform.Find("Circle/1").GetComponent<MeshRenderer>().material.color = x, 
                                new Color(color1_1.r, color1_1.g, color1_1.b, 0), 1).SetLoops(-1).SetEase(Ease.Linear);
                    tw2 = DOTween.To(() => transform.Find("Circle/2").GetComponent<MeshRenderer>().material.color, x =>
                                transform.Find("Circle/2").GetComponent<MeshRenderer>().material.color = x, 
                                new Color(color2_1.r, color2_1.g, color2_1.b, 0), 1).SetLoops(-1).SetEase(Ease.Linear);
                    tw3 = DOTween.To(() => transform.Find("Circle/3").GetComponent<MeshRenderer>().material.color, x =>
                                    transform.Find("Circle/3").GetComponent<MeshRenderer>().material.color = x, 
                                    new Color(color3_1.r, color3_1.g, color3_1.b, 0), 1).SetLoops(-1).SetEase(Ease.Linear);
                    tw4 = DOTween.To(() => tuo.GetComponent<SpriteRenderer>().color, x =>
                                     tuo.GetComponent<SpriteRenderer>().color = x, 
                                     new Color(color4_1.r, color4_1.g, color4_1.b, 0.3f), 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                }
            }
        }
        private void SetTextMeshPro() {
            for (int j = 0; j < resEntityTemp.data.Count; j++)
            {
                if (textSize < Encoding.Default.GetByteCount(resEntityTemp.data[j].key + ":" + resEntityTemp.data[j].value))
                {
                    textSize = Encoding.Default.GetByteCount(resEntityTemp.data[j].key + ":" + resEntityTemp.data[j].value);
                }
                textParent.Find(j.ToString()).GetComponent<TextMeshPro>().SetText(resEntityTemp.data[j].key + ":" + resEntityTemp.data[j].value);
                textParent.Find(j.ToString()).SetSiblingIndex(j);
                textParent.Find(j.ToString()).localPosition = new Vector3(0, 1.6f + 0.25f * (resEntityTemp.data.Count - j), 0);
            }
            indexPanel.DOScaleX(1f / 32f * textSize, 0);
        }
        private void LoadSprite(Sprite s) {
            iconSR.sprite = s;
        }

        public void ChangePianRadius(float _index) {
            pian.transform.DOScaleX(scale_x + _index * rate / 25f, 0f);
            pian.transform.DOScaleZ(scale_z + _index * rate / 25f, 0f).OnComplete(delegate() {
                scale_x = pian.transform.localScale.x;
                scale_z = pian.transform.localScale.z;
                AlarmState();
            });
        }
        private Dictionary<string, List<ElementItem>> dicElements = new Dictionary<string, List<ElementItem>>();
        private void AlarmState() {
            dicElements.Clear();
            foreach (Transform item in textParent)
            {
                DestroyImmediate(item.gameObject);
            }
            numTemp = 1.8f;
            string result = "[{\"alarmResId\":\"" + transform.name + "\",\"alarmResTCode\":\"" + t_eType + "\",\"alarmRangeRes\":[";
            foreach (ElementItem item in transform.parent.parent.parent.GetComponentsInChildren<ElementItem>(true))
            {
                if (item.name != transform.name)
                {
                    Debug.Log(item.name + "和" + transform.name + "的距离" + Vector3.Distance(item.transform.position, transform.position));
                    Debug.Log("当前距离" + scale_x * 25);
                    if (Vector3.Distance(item.transform.position, transform.position) <= scale_x * 25)
                    {
                        result += "{\"resId\":\"" + item.name + "\",\"t_eCode\":\"" + item.t_eType + "\"},";
                        if (dicElements.ContainsKey(ElementContainer.GetInstance().diceType[item.t_eType].name))
                        {
                            dicElements[ElementContainer.GetInstance().diceType[item.t_eType].name].Add(item);
                        }
                        else
                        {
                            List<ElementItem> listTemp = new List<ElementItem>();
                            listTemp.Add(item);
                            dicElements.Add(ElementContainer.GetInstance().diceType[item.t_eType].name, listTemp);
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(result))
            {
                result = result.Remove(result.LastIndexOf(','), 1);
            }
            result += "]}]";
            GameObject.Find("JSInterface").GetComponent<JSInterface>().sendAlarmRes(result);
            //ElementContainer.GetInstance().SingleAlarm(transform, scale_x, dicElements);
            foreach (KeyValuePair<string,List<ElementItem>> item in dicElements)
            {
                GameObject goText = null;
                numTemp += 0.25f;
                if (textParent.Find(item.Key) != null)
                {
                    goText = textParent.Find(item.Key).gameObject;
                }
                else
                {
                    goText = Instantiate(P_text) as GameObject;
                    goText.transform.SetParent(textParent);
                    goText.transform.localPosition = new Vector3(0, numTemp - 0.2f, 0);
                    goText.transform.localScale = new Vector3(1, 1, 1);
                    goText.transform.localEulerAngles = Vector3.zero;
                    goText.name = item.Key;
                }
                if (textSize < Encoding.Default.GetByteCount(item.Key + "个数：" + item.Value.Count + "个"))
                {
                    textSize = Encoding.Default.GetByteCount(item.Key + "个数：" + item.Value.Count + "个");
                }
                goText.GetComponent<TextMeshPro>().text = item.Key + "个数：" + item.Value.Count + "个";
                indexPanel.DOScaleX(1f / 32f * textSize, 0);
            }
            indexH = numTemp - 1.8f;
            indexPanel.DOScaleY(indexH, 0.2f).SetEase(Ease.Linear).OnComplete(delegate() {
                foreach (Transform item in textParent)
                {
                    if (item.localPosition.y > indexH + 1.6f)
                    {
                        DestroyImmediate(item.gameObject);
                    }
                }
            });
        }
        public void SetTrackData(Track track, bool refresh = false,List<IndexData> _data = null,bool inPlay = true) {
            if (!refresh)
            {
                if (DynamicElement.GetInstance().exist)
                {
                    foreach (Transform item in textParent)
                    {
                        Destroy(item.gameObject);
                    }
                    numTemp = 1.8f;
                }
            }
            if (inPlay)
            {
                StartCoroutine(WaitTrack(track, refresh, _data));
            }
        }
        private IEnumerator WaitTrack(Track track, bool refresh = false, List<IndexData> _data = null) {
            yield return new WaitForSeconds(0);
            if (!refresh)
            {
                if (ElementContainer.GetInstance().diceTypeSprites.ContainsKey(track.t_eCode))
                {
                    iconSR.sprite = ElementContainer.GetInstance().diceTypeSprites[track.t_eCode];
                }
                if (_data != null)
                {
                    for (int i = _data.Count - 1; i >= 0; i--)
                    {
                        numTemp += 0.25f;
                        GameObject goTextTime = Instantiate(P_text) as GameObject;
                        goTextTime.transform.SetParent(textParent);
                        goTextTime.transform.localPosition = new Vector3(0, numTemp - 0.2f, 0);
                        goTextTime.transform.localScale = new Vector3(1, 1, 1);
                        goTextTime.transform.localEulerAngles = Vector3.zero;
                        indexH = numTemp - 1.8f;
                        goTextTime.name = (i + track.data.Count).ToString();
                        if (textSize < Encoding.Default.GetByteCount(track.track[0].data[i].key + ":" + track.track[0].data[i].value))
                        {
                            textSize = Encoding.Default.GetByteCount(track.track[0].data[i].key + ":" + track.track[0].data[i].value);
                        }
                        goTextTime.GetComponent<TextMeshPro>().text = track.track[0].data[i].key + ":" + track.track[0].data[i].value;
                    }
                }
                for (int i = track.data.Count - 1; i >= 0; i--)
                {
                    GameObject goText = null;
                    if (textParent.Find(i.ToString()) != null)
                    {
                        goText = textParent.Find(i.ToString()).gameObject;
                    }
                    else
                    {
                        numTemp += 0.25f;
                        goText = Instantiate(P_text) as GameObject;
                        goText.transform.SetParent(textParent);
                        goText.transform.localPosition = new Vector3(0, numTemp - 0.2f, 0);
                        goText.transform.localScale = new Vector3(1, 1, 1);
                        goText.transform.localEulerAngles = Vector3.zero;
                        indexH = numTemp - 1.8f;
                        goText.name = i.ToString();
                    }
                    if (textSize < Encoding.Default.GetByteCount(track.data[i].key + ":" + track.data[i].value))
                    {
                        textSize = Encoding.Default.GetByteCount(track.data[i].key + ":" + track.data[i].value);
                    }
                    goText.GetComponent<TextMeshPro>().text = track.data[i].key + ":" + track.data[i].value;
                }

                indexPanel.DOScaleX(1f / 32f * textSize, 0);
                indexPanel.DOScaleY(indexH, 0f);
            }
            else
            {
                if (_data != null)
                {
                    for (int i = 0; i < _data.Count; i++)
                    {
                        textParent.Find((i + (numTemp - 0.25f * _data.Count - 1.8f) / 0.25f).ToString()).GetComponent<TextMeshPro>().text = _data[i].key + ":" + _data[i].value;
                    }
                }                
            }
        }
    }
}