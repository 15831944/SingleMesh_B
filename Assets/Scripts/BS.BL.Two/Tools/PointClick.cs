using BS.BL.Two.Element;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BS.BL.Two
{
    public class PointClick : MonoBehaviour
    {
        private Transform array;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }
        public void OnMouseUp()
        {
            Debug.Log("鼠标按下");
            foreach (KeyValuePair<string, List<ElementItem>> item in ElementContainer.GetInstance().dicElements)
            {
                foreach (ElementItem itemValue in item.Value)
                {
                    if (itemValue.nowLevel == 1)
                    {
                        if (itemValue != ElementContainer.GetInstance().nowElement)
                        {
                            itemValue.SetUIByLevel(0);
                        }
                    }
                }
            }
            foreach (Transform item in Element.ElementContainer.GetInstance().arrayLabelParent)
            {
                if (!item.gameObject.active 
                && !DynamicElement.GetInstance().isPlay
                && !DynamicElement.GetInstance().inPlay)
                {
                    array = item;
                }
            }
            if (transform.parent.parent.GetComponent<ElementItem>() != null 
                && ElementContainer.GetInstance().isClick
                && !DynamicElement.GetInstance().isPlay
                && !DynamicElement.GetInstance().inPlay)
            {
                transform.parent.parent.GetComponent<ElementItem>().SetUIByLevel(1);
                if (array != null)
                {
                    if (transform.parent.parent.GetComponent<ElementItem>().lineId != array.name)
                    {
                        array.gameObject.SetActive(true);
                    }
                }
            }
            if (transform.parent.parent.GetComponent<ElementArray>() != null
                && ElementContainer.GetInstance().isClick 
                && !DynamicElement.GetInstance().isPlay 
                && !DynamicElement.GetInstance().inPlay)
            {
                if (array != null)
                {
                    array.gameObject.SetActive(true);
                }
                transform.parent.parent.gameObject.SetActive(false);
                if (Vector3.Distance(transform.position, Camera.main.transform.position) > 1000)
                {
                    Camera.main.GetComponent<BLCameraControl>().LookAtPosition3(transform.parent.parent.position);
                }
                transform.parent.parent.parent.GetComponent<ArrayParent>().WaitTime(transform.parent.parent.gameObject);
                ElementContainer.GetInstance().SceneToLevel(1);
            }
            if (transform.parent.parent.GetComponent<ElementArea>() != null
                && ElementContainer.GetInstance().isClick
                && !DynamicElement.GetInstance().isPlay
                && !DynamicElement.GetInstance().inPlay)
            {
                transform.parent.parent.GetComponent<ElementArea>().SetUIByLevel(1);
            }
        }
    }
}