using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
namespace BS.BL.Two.Element {
    public class ElementArray : MonoBehaviour
    {
        public Transform index;
        public GameObject textP;
        public Transform textParent;
        public SpriteRenderer iconJH, iconTuo;
        public Dictionary<string, List<Transform>> dicResType = new Dictionary<string, List<Transform>>();
        private int textSize = 0;
        public void Set(List<Transform> listEntitys)
        {
            foreach (KeyValuePair<string,List<Transform>> item in dicResType)
            {
                foreach (Transform itemValue in item.Value)
                {
                    if (!listEntitys.Contains(itemValue))
                    {
                        foreach (Transform itemChild in textParent)
                        {
                            Destroy(itemChild.gameObject);
                        }
                    }
                }
            }
            dicResType.Clear();
            foreach (Transform item in listEntitys)
            {
                item.GetComponent<ElementItem>().arrayTrans = transform;
                if ((DynamicElement.GetInstance().exist && DynamicElement.GetInstance().inPlay) || !DynamicElement.GetInstance().inPlay)
                {
                    if (!gameObject.activeInHierarchy)
                    {
                        item.gameObject.SetActive(true);
                    }
                }
                if (dicResType.ContainsKey(item.GetComponent<ElementItem>().t_eType))
                {
                    dicResType[item.GetComponent<ElementItem>().t_eType].Add(item);
                }
                else
                {
                    List<Transform> listTemp = new List<Transform>();
                    listTemp.Add(item);
                    dicResType.Add(item.GetComponent<ElementItem>().t_eType, listTemp);
                }
            }
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(wait());
            }
        }
        private IEnumerator wait() {
            yield return new WaitForSeconds(0f);

            float numTemp = 1.15f;
            foreach (KeyValuePair<string, List<Transform>> itemText in dicResType)
            {
                if (ElementContainer.GetInstance().listType.Contains(itemText.Key))
                {
                    GameObject goText = null;
                    numTemp += 0.25f;
                    if (textParent.Find(itemText.Key) != null)
                    {
                        goText = textParent.Find(itemText.Key).gameObject;
                    }
                    else if (ElementContainer.GetInstance().diceType.ContainsKey(itemText.Key))
                    {
                        goText = Instantiate(textP) as GameObject;
                        goText.transform.SetParent(textParent);
                        goText.transform.localPosition = new Vector3(0, numTemp - 0.2f, 0);
                        goText.transform.localScale = new Vector3(1, 1, 1);
                        goText.transform.localEulerAngles = Vector3.zero;
                        goText.name = itemText.Key;
                    }
                    if (textSize < Encoding.Default.GetByteCount(ElementContainer.GetInstance().diceType[itemText.Key].name + "个数：" + itemText.Value.Count))
                    {
                        textSize = Encoding.Default.GetByteCount(ElementContainer.GetInstance().diceType[itemText.Key].name + "个数：" + itemText.Value.Count);
                    }
                    goText.GetComponent<TextMeshPro>().text = ElementContainer.GetInstance().diceType[itemText.Key].name + "个数：" + itemText.Value.Count;
                }
            }
            index.transform.localScale = new Vector3(1f / 32f * textSize, numTemp - 1.1f, 1);
            iconJH.sprite = Manager.GetInstance().iconJH;
            iconTuo.color = Manager.GetInstance().iconColor;
        }
        private void OnDisable()
        {
            if (!DynamicElement.GetInstance().inPlay)
            {
                foreach (KeyValuePair<string, List<Transform>> item in dicResType)
                {
                    foreach (Transform itemChild in item.Value)
                    {
                        itemChild.gameObject.SetActive(true);
                    }
                }
            }
        }
        private void OnEnable()
        {
            foreach (KeyValuePair<string, List<Transform>> item in dicResType)
            {
                foreach (Transform itemChild in item.Value)
                {
                    itemChild.gameObject.SetActive(false);
                    itemChild.GetComponent<ElementItem>().SetUIByLevel(0);
                }
            }
            StartCoroutine(wait());
        }
        private void Update()
        {
            transform.LookAt(Camera.main.transform);
        }
        private void FixedUpdate()
        {
            //if (Vector3.Distance(transform.position,Camera.main.transform.position) < 350f)
            //{
            //    if (!GetActiveArray())
            //    {
            //        gameObject.SetActive(false);
            //        transform.parent.GetComponent<ArrayParent>().WaitTime();
            //    }
            //}
        }
        private bool GetActiveArray() {
            foreach (Transform item in transform.parent)
            {
                if (!item.gameObject.activeInHierarchy)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

