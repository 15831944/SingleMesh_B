using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TFramework.ApplicationLevel
{
    public interface IEventCenter
    {
        void IInitManagers();
        void IRegisterAllListener();
        void IClearAllListener();
    }
}