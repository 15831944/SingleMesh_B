/********************************************************************************

** 作者： Nemo

** E-Mail： nemo.lbs@gamil.com

** 创始时间：2016-7-14

** 描述：

** 事件处理系统 统一外部调用接口（可另行包装或者直接使用Dispatcher与Lintsener）

*********************************************************************************/

namespace TFramework.EventSystem
{
    public class TEventSystem
    {
        private static TEventSystem instance;
        public static TEventSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TEventSystem();
                }
                return instance;
            }
        }

        public readonly NEventDispatcher EventManager = new NEventDispatcher();

    }
}