/********************************************************************************

** 作者： Nemo

** E-Mail： nemo.lbs@gamil.com

** 创始时间：2016-7-14

** 描述：

** 事件处理系统 事件监听器

*********************************************************************************/

using UnityEngine;
using System.Collections;

namespace TFramework.EventSystem
{
    public class NEventListener
    {

        public NEventListener() { }

        public delegate void EventListenerDelegate(TEvent evt);
        public event EventListenerDelegate OnEvent;

        public void Excute(TEvent evt)
        {
            if (OnEvent != null)
            {
                this.OnEvent(evt);
            }
        }
    }
}