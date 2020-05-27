using BS.BL.Interface;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BS.BL.Two.Element
{
    public class DynamicElement : MonoBehaviour
    {
        private static DynamicElement instance;
        public static DynamicElement GetInstance() {
            if (instance == null)
            {
                instance = new DynamicElement();
            }
            return instance;
        }
        private void Awake()
        {
            instance = this;
        }
        public bool isPlay = false;
        public bool inPlay = false;
        private List<Vector3> listPos = new List<Vector3>();
        private Dictionary<Vector3, string> dicPosLine = new Dictionary<Vector3, string>();
        private Transform target;
        public float moveSpeed = 1f;
        public Track nowTrack;
        public bool exist = true;
        private Vector3 initPos;
        public ElementItem targetItem;
        private float d_Length = 0f;
        private float z_Degree = 0f;
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void FixedUpdate()
        {
            SetTargetAnimation();
            DisAbleEntity();
        }
        float waitTime = 0f;
        private void DisAbleEntity() {
            if (inPlay)
            {
                waitTime += Time.deltaTime;
                if (waitTime > 1f)
                {
                    foreach (KeyValuePair<string, List<ElementItem>> item in ElementContainer.GetInstance().dicElements)
                    {
                        foreach (ElementItem itemElement in item.Value)
                        {
                            if (itemElement != ElementContainer.GetInstance().nowElement)
                            {
                                itemElement.gameObject.SetActive(false);
                            }
                        }
                    }
                    foreach (Transform itemArray in ElementContainer.GetInstance().arrayLabelParent)
                    {
                        itemArray.gameObject.SetActive(false);
                    }
                    waitTime = 0f;
                }
                
            }
        }
        /// <summary>
        /// 添加路径数组
        /// </summary>
        /// <param name="pos"></param>
        public void SetPointPosition(Vector3 pos,string lineId) {
            if (listPos.Contains(pos))
            {
                return;
            }
            listPos.Add(pos);
            dicPosLine.Add(pos, lineId);
            inPlay = true;
        }
        /// <summary>
        /// 重置
        /// </summary>
        public void ResetStart()
        {
            listPos.Clear();
            dicPosLine.Clear();
            isPlay = false;
            inPlay = false;
            if (nowTrack!= null)
            {
                foreach (KeyValuePair<GameObject, bool> item in ElementContainer.GetInstance().dicTrack)
                {
                    if (item.Key != null)
                    {
                        item.Key.SetActive(item.Value);
                    }
                }
                if (exist)
                {
                    target.position = initPos;
                    foreach (KeyValuePair<string, List<Transform>> item in ElementContainer.GetInstance().dicLineLabels)
                    {
                        if (item.Value.Contains(target))
                        {
                            if (ElementContainer.GetInstance().arrayLabelParent.Find(item.Key) != null)
                            {
                                ElementContainer.GetInstance().arrayGo = ElementContainer.GetInstance().arrayLabelParent.Find(item.Key).gameObject;
                                ElementContainer.GetInstance().arrayLabelParent.Find(item.Key).gameObject.SetActive(false);
                            }
                            //ElementContainer.GetInstance().level = 1;
                        }
                    }
                    target.GetComponent<ElementItem>().Set(target.GetComponent<ElementItem>().resEntityTemp, false, true);
                }
                else
                {
                    Destroy(target.gameObject);
                    ElementContainer.GetInstance().SceneToLevel(1);
                    exist = true;
                }
                ElementContainer.GetInstance().nowElement = target.GetComponent<ElementItem>();
                if (ElementContainer.GetInstance().arrayGo != null)
                {
                    if (ElementContainer.GetInstance().dicTrack.ContainsKey(ElementContainer.GetInstance().arrayGo))
                    {
                        if (ElementContainer.GetInstance().dicTrack[ElementContainer.GetInstance().arrayGo] == true)
                        {
                            ElementContainer.GetInstance().ResetLabel(true);
                        }
                    }
                }
                nowTrack = null;
            }
            StartCoroutine(WaitTime());
        }
        IEnumerator WaitTime() {
            yield return new WaitForSeconds(0.1f);
            if (ElementContainer.GetInstance().nowElement != null)
            {
                ElementContainer.GetInstance().nowElement.SetUIByLevel(1);
            }
            target = null;
        }
        /// <summary>
        /// 设置目标物体
        /// </summary>
        /// <param name="_target"></param>
        public void SetTarget(Transform _target,Vector3 _initPos, bool _exist = true) {
            initPos = _initPos;
            posNum = 0;
            exist = _exist;
            target = _target;
            d_Length = 0f;
            z_Degree = 0f;
            target.localPosition = listPos[0];
            Vector3 temp = new Vector3();
            for (int i = 0; i < listPos.Count; i++)
            {
                if (i + 1 < listPos.Count)
                {
                    d_Length += Vector3.Distance(listPos[i], listPos[i + 1]);
                }
            }
            foreach (Vector3 item in listPos)
            {
                Vector3 temp1 = ElementContainer.GetInstance().transform.TransformPoint(item);
                temp += temp1;
            }
            Camera.main.GetComponent<BLCameraControl>().LookAtPosition3(temp / listPos.Count);
        }
        private int posNum = 0;
        private void SetTargetAnimation() {
            if (isPlay)
            {
                if (listPos.Count > 0)
                {
                    if (posNum < listPos.Count - 1)
                    {
                        if (posNum == 0)
                        {
                            z_Degree = Vector3.Distance(target.localPosition, listPos[0]) / d_Length;
                        }
                        else
                        {
                            float _distance = 0f;
                            for (int i = 0; i < posNum; i++)
                            {
                                _distance += Vector3.Distance(listPos[i], listPos[i + 1]);
                            }
                            _distance += Vector3.Distance(listPos[posNum], target.localPosition);
                            z_Degree = _distance / d_Length;
                        }
                        //ResTest.GetInstance().slider.DOValue(z_Degree, 0);
                        GameObject.Find("JSInterface").GetComponent<JSInterface>().sendTrackDegree(z_Degree.ToString());
                        if (dicPosLine[listPos[posNum]] == dicPosLine[listPos[posNum + 1]])
                        {
                            target.localPosition = Vector3.MoveTowards(target.localPosition, listPos[posNum + 1], moveSpeed * Time.deltaTime);
                        }
                        else
                        {
                            target.localPosition = listPos[posNum + 1];
                        }
                        target.GetComponent<ElementItem>().SetTrackData(null, true, nowTrack.track[posNum].data);
                        if (target.localPosition == listPos[posNum + 1])
                        {
                            posNum += 1;
                        }
                    }
                    else
                    {
                        posNum = 0;
                        target.localPosition = listPos[posNum];
                        GameObject.Find("JSInterface").GetComponent<JSInterface>().sendTrackDegree("0");
                        target.GetComponent<ElementItem>().SetTrackData(null, true, nowTrack.track[posNum].data);
                    }
                }
            }
        }
        public void SetTrackDegree(float degree) {
            isPlay = false;
            if (listPos.Count > 0)
            {
                float _degreeLength = degree * d_Length;
                float _distance2 = 0f;
                if (degree != 0)
                {
                    for (int i = 0; i < listPos.Count; i++)
                    {
                        if (i + 1 < listPos.Count)
                        {
                            if (_degreeLength > _distance2)
                            {
                                _distance2 += Vector3.Distance(listPos[i], listPos[i + 1]);
                            }
                            if (_degreeLength == _distance2)
                            {
                                target.localPosition = listPos[i + 1];
                                target.GetComponent<ElementItem>().SetTrackData(null, true, nowTrack.track[i + 1].data);
                                posNum = i + 1;
                                return;
                            }
                            if (_degreeLength < _distance2)
                            {
                                target.localPosition = listPos[i + 1] - (_distance2 - _degreeLength) * (listPos[i + 1] - listPos[i]).normalized;
                                target.GetComponent<ElementItem>().SetTrackData(null, true, nowTrack.track[i].data);
                                posNum = i;
                                return;
                            }
                        }
                    }
                }
                else
                {
                    target.localPosition = listPos[0];
                    posNum = 0;
                }
            }
        }
    }
}
