using DXFConvert;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BS.BL.Two {
    public enum ManagerType { 
        initData,
        resEntity
    }
    public class Manager : MonoBehaviour,IManager
    {
        private static Manager instance;
        public static Manager GetInstance() {
            if (instance == null)
            {
                instance = new Manager();
            }
            return instance;
        }
        private void Awake()
        {
            instance = this;
            Init();
        }
        private Dictionary<ManagerType, IManager> dicManager = new Dictionary<ManagerType, IManager>();
        public Element.LimitConfig limitConfig;
        public Sprite iconJH;
        public Color iconColor;
        public void InitJH() {
            StartCoroutine(elementContainer.loadConfig.LoadPhoto(limitConfig.icon, SetSprite));
            iconColor = ColorToRGBA.GetColor(limitConfig.color);
        }
        private void SetSprite(Sprite _sprite)
        {
            iconJH = _sprite;
        }
        public void Init()
        {
            elementContainer = GameObject.Find("ResLabel").GetComponent<Element.ElementContainer>();
            if (!dicManager.ContainsKey(ManagerType.initData))
            {
                dicManager.Add(ManagerType.initData, gameObject.AddComponent<InitData>());
            }
            if (!dicManager.ContainsKey(ManagerType.resEntity))
            {
                dicManager.Add(ManagerType.resEntity, gameObject.AddComponent<ResEntity>());
            }
            
        }

        public Element.ElementContainer elementContainer;

        public IManager GetManager(ManagerType managerType) {
            if (dicManager.ContainsKey(managerType))
            {
                return dicManager[managerType];
            }
            return null;
        }
    }
}

