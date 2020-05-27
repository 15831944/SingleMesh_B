using UnityEngine;
using TFramework.EventSystem;

namespace TFramework.ApplicationLevel
{
    public class TEventType {

        //北路
        public const string ChangeDim = "ChangeDim";
    }

    public class EventCenter : MonoBehaviour,IEventCenter
    {

        public void IClearAllListener()
        {
            throw new System.NotImplementedException();
        }

        private void InitView() {
            //applicationSM = GetComponent<ApplicationSM>();
            //showUpgroundButton = GameObject.Find("Canvas").transform.Find("GroundButtons").gameObject;
        }

        public void IInitManagers()
        {
        }

        public void IRegisterAllListener()
        {
            TEventSystem.Instance.EventManager.addEventListener(TEventType.ChangeDim, ChangeDim);
        }

        /***************************逻辑实现方法************************************/

        private void ChangeDim(TEvent nEvent) {
            //GameObject.Find("Root").GetComponent<ApplicationSM>().ChargeTransState(SOperate.ChangeDim, nEvent.eventParams);
        }
        // Use this for initialization
        void Start()
        {
            InitView();
            IInitManagers();
            IRegisterAllListener();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
