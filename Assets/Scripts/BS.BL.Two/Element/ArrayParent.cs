using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BS.BL.Two.Element {
    public class ArrayParent : MonoBehaviour
    {

        // Start is called before the first frame update
        void Start()
        {

        }
        private Transform noActiveArray = null;
        private bool isNear = false;
        public void WaitTime(GameObject go) {
            StartCoroutine(waitSomeTime(go));
        }
        IEnumerator waitSomeTime(GameObject go) {
            yield return new WaitForSeconds(0.6f);
            isNear = true;
            Element.ElementContainer.GetInstance().arrayGo = go;
        }
        // Update is called once per frame
        private void FixedUpdate()
        {
            //if (GetChildActive() != null)
            //{
            //    noActiveArray = GetChildActive();
            //    if (Vector3.Distance(noActiveArray.position, Camera.main.transform.position) > 1100 && isNear)
            //    {
            //        noActiveArray.gameObject.SetActive(true);
            //        noActiveArray = null;
            //        isNear = false;
            //    }
            //}
            //if (!RayTools.GetArrayIndex() && !isStartTime)
            //{
            //    if (noActiveArray != null)
            //    {
            //        noActiveArray.gameObject.SetActive(true);
            //        noActiveArray = null;
            //        isStartTime = true;
            //    }
            //}
        }
        private Transform GetChildActive() {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (!transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    return transform.GetChild(i);
                }
            }
            return null;
        }
    }
}

