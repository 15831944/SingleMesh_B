using BS.BL.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BS.BL.Element {
    public class ElementPoint : MonoBehaviour
    {
        public ElementItem elementItem;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnBecameInvisible()
        {
            elementItem.gameObject.SetActive(false);
        }
        private void OnBecameVisible()
        {
            int _type = (MainManager.GetInstance().dicAllManager[ManagerType.uiManager] as UIManager).nowState;
            switch (_type)
            {
                case 0:
                    elementItem.gameObject.SetActive(true);
                    break;
                case 1:
                    if (elementItem.resEntitySelf.type == 1)
                    {
                        elementItem.gameObject.SetActive(true);
                    }
                    break;
                case 2:
                    if (elementItem.resEntitySelf.type == 4)
                    {
                        elementItem.gameObject.SetActive(true);
                    }
                    break;
                case 3:
                    if (elementItem.resEntitySelf.type == 3)
                    {
                        elementItem.gameObject.SetActive(true);
                    }
                    break;
                case 4:
                    if (elementItem.resEntitySelf.type == 5 || elementItem.resEntitySelf.type == 0)
                    {
                        elementItem.gameObject.SetActive(true);
                    }
                    break;
                case 5:
                    if (elementItem.resEntitySelf.type == 2)
                    {
                        elementItem.gameObject.SetActive(true);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}

