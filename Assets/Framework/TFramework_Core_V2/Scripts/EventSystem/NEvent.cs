using UnityEngine;
using System.Collections;

namespace TFramework.EventSystem
{
    public class TEvent
    {

        /// <summary>
        /// 事件类别
        /// </summary>
        public string eventType;

        /// <summary>
        /// 参数
        /// </summary>
        public object[] eventParams;

        /// <summary>
        /// 事件抛出者
        /// </summary>
        public object target;

        public TEvent(string eventType, params object[] eventParams)
        {
            this.eventType = eventType;
            this.eventParams = eventParams;
        }
    }
}