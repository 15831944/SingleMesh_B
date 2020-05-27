using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BS.BL.Manager {
    public enum ManagerType {
        loadManager,
        uiManager,
    }
    public class MainManager : MonoBehaviour
    {
        private static MainManager instance;
        public static MainManager GetInstance() {
            if (instance == null)
            {
                instance = new MainManager();
            }
            return instance;
        }
        public Dictionary<ManagerType, ManagerBase> dicAllManager = new Dictionary<ManagerType, ManagerBase>();
        public GameObject overground;
        // Start is called before the first frame update
        private void Awake()
        {
            instance = this;
        }
        void Start()
        {
            Init();
            //InitOverGroundMat();
        }
        void Init() {
            dicAllManager.Add(ManagerType.loadManager, gameObject.AddComponent<LoadManager>());
            //dicAllManager.Add(ManagerType.uiManager, gameObject.AddComponent<UIManager>());
        }
        void InitOverGroundMat() {
            foreach (Material item in overground.GetComponent<MeshRenderer>().materials)
            {
                Color color = item.color;
                item.color = new Color(color.r, color.g, color.b, 0.5f);
            }
        }
        private void FixedUpdate()
        {
            waitTime += Time.deltaTime;
            if (waitTime > 30f)
            {
                UnLoadResources();
                waitTime = 0f;
            }
        }
        float waitTime = 0f;
        private void UnLoadResources() {
            Resources.UnloadUnusedAssets();
        }
    }
}

