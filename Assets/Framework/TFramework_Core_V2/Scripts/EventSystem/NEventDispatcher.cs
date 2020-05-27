/********************************************************************************

** 作者： Nemo

** E-Mail： nemo.lbs@gamil.com

** 创始时间：2016-7-14

** 描述：

** 事件处理系统 事件发送类

*********************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TFramework.EventSystem
{
    public class NEventDispatcher
    {

        protected Dictionary<string, NEventListener> eventListenerDict;

        public NEventDispatcher()
        {
            this.eventListenerDict = new Dictionary<string, NEventListener>();
        }

        /// <summary>
        /// 侦听事件
        /// </summary>
        /// <param name="eventType">事件类别</param>
        /// <param name="callback">回调函数</param>
        public void addEventListener(string eventType, NEventListener.EventListenerDelegate callback)
        {
            if (!this.eventListenerDict.ContainsKey(eventType))
            {
                this.eventListenerDict.Add(eventType, new NEventListener());
            }
            this.eventListenerDict[eventType].OnEvent += callback;
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="eventType">事件类别</param>
        /// <param name="callback">回调函数</param>
        public void removeEventListener(string eventType, NEventListener.EventListenerDelegate callback)
        {
            if (this.eventListenerDict.ContainsKey(eventType))
            {
                this.eventListenerDict[eventType].OnEvent -= callback;
            }
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="evt">Evt.</param>
        /// <param name="gameObject">Game object.</param>
        public void dispatchEvent(TEvent evt, object gameObject)
        {
            NEventListener eventListener = eventListenerDict[evt.eventType];
            if (eventListener == null) return;

            evt.target = gameObject;
            eventListener.Excute(evt);
        }

        /// <summary>
        /// 是否存在事件
        /// </summary>
        /// <returns><c>true</c>, if listener was hased, <c>false</c> otherwise.</returns>
        /// <param name="eventType">Event type.</param>
        public bool hasListener(string eventType)
        {
            return this.eventListenerDict.ContainsKey(eventType);
        }

        /// <summary>
        /// 清空所有事件监听
        /// </summary>
        public void ClearAllListener()
        {
            eventListenerDict.Clear();
        }
    }
}