using BS.BL.Interface;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
namespace BS.BL.Two.Element {
    public class ElementArea : MonoBehaviour
    {
        public GameObject first, second;
        public Transform line, indexPanel;
        public Transform textParent;



        private void OnEnable()
        {
            SetUIByLevel(0);
        }
        /// <summary>
        /// 根据当前层级显示UI
        /// </summary>
        /// <param name="level">0-第一层级（默认层级），1-第二层级</param>
        public void SetUIByLevel(int level)
        {

            switch (level)
            {
                case 0:
                    Debug.Log("显示第一层级");
                    first.transform.Find("tuo").gameObject.SetActive(true);
                    textParent.gameObject.SetActive(false);
                    indexPanel.DOScaleY(0, 0.2f).SetEase(Ease.Linear).OnComplete(delegate () {
                        first.transform.Find("Icon").DOLocalMoveY(0.45f, 0.2f);
                        line.DOScaleY(0, 0.2f).SetEase(Ease.Linear).OnComplete(delegate ()
                        {
                            second.SetActive(false);
                            first.SetActive(true);
                        });
                    });
                    Camera.main.GetComponent<BLCameraControl>().SetZoomLimit(0, 2500);
                    transform.Find("Circle").gameObject.SetActive(false);
                    break;
                case 1:
                    Debug.Log("显示第二层级");
                    ElementContainer.GetInstance().isClick = false;
                    first.SetActive(true);
                    second.SetActive(true);
                    first.transform.Find("tuo").gameObject.SetActive(false);
                    first.transform.Find("Icon").DOLocalMoveY(1.3f, 0.2f);
                    transform.Find("Circle").gameObject.SetActive(true);
                    line.DOScaleY(0.9f, 0.2f).SetEase(Ease.Linear).OnComplete(delegate () {
                        indexPanel.DOScaleY(0.3f, 0.2f).SetEase(Ease.Linear).OnComplete(delegate () {
                            textParent.gameObject.SetActive(true);
                            ElementContainer.GetInstance().isClick = true;
                        });
                    });
                    Camera.main.GetComponent<BLCameraControl>().LookAtPosition2(transform.position,500F);
                    GameObject.Find("JSInterface").GetComponent<JSInterface>().selectCurrentArea(transform.name);
                    break;
                default:
                    break;
            }
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
        Tweener tw1 = null, tw2 = null, tw3 = null, tw4 = null;
        Tweener tws1 = null, tws2 = null, tws3 = null, tws4 = null, tws5 = null, tws6 = null, tws7 = null, tws8 = null;
        public void Set(string areaName)
        {
            int textSize = Encoding.Default.GetByteCount(areaName);
            indexPanel.DOScaleX(1f / 32f * textSize, 0);
            textParent.Find("TextP").GetComponent<TextMeshPro>().text = areaName;
            Color nowColor = transform.Find("Circle/1").GetComponent<MeshRenderer>().material.color;
            Color color1 = new Color(nowColor.r, nowColor.g, nowColor.b, 1);
            Color color2 = new Color(nowColor.r, nowColor.g, nowColor.b, 1);
            Color color3 = new Color(nowColor.r, nowColor.g, nowColor.b, 1);
            Color color4 = new Color(nowColor.r, nowColor.g, nowColor.b, 1);
            tws1 = transform.Find("Circle/1").DOScaleX(1, 1).SetLoops(-1).SetEase(Ease.Linear);
            tws2 = transform.Find("Circle/1").DOScaleZ(1, 1).SetLoops(-1).SetEase(Ease.Linear);
            tws3 = transform.Find("Circle/2").DOScaleX(2F / 3F, 1).SetLoops(-1).SetEase(Ease.Linear);
            tws4 = transform.Find("Circle/2").DOScaleZ(2F / 3F, 1).SetLoops(-1).SetEase(Ease.Linear);
            tws5 = transform.Find("Circle/3").DOScaleX(1F / 3F, 1).SetLoops(-1).SetEase(Ease.Linear);
            tws6 = transform.Find("Circle/3").DOScaleZ(1F / 3F, 1).SetLoops(-1).SetEase(Ease.Linear);
            tw1 = DOTween.To(() => transform.Find("Circle/1").GetComponent<MeshRenderer>().material.color, x =>
                            transform.Find("Circle/1").GetComponent<MeshRenderer>().material.color = x, new Color(color1.r, color1.g, color1.b, 0), 1).SetLoops(-1).SetEase(Ease.Linear);
            tw2 = DOTween.To(() => transform.Find("Circle/2").GetComponent<MeshRenderer>().material.color, x =>
                            transform.Find("Circle/2").GetComponent<MeshRenderer>().material.color = x, new Color(color2.r, color2.g, color2.b, 0), 1).SetLoops(-1).SetEase(Ease.Linear);
            tw3 = DOTween.To(() => transform.Find("Circle/3").GetComponent<MeshRenderer>().material.color, x =>
                            transform.Find("Circle/3").GetComponent<MeshRenderer>().material.color = x, new Color(color3.r, color3.g, color3.b, 0), 1).SetLoops(-1).SetEase(Ease.Linear);
            tw4 = DOTween.To(() => first.transform.Find("tuo").GetComponent<SpriteRenderer>().color, x =>
                                first.transform.Find("tuo").GetComponent<SpriteRenderer>().color = x, new Color(color4.r, color4.g, color4.b, 0.3f), 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
           
        }
    }
}

