using BS.BL.Element;
using BS.BL.Two;
using BS.BL.Two.Element;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BS.BL.Manager {
    public class LoadManager : MonoBehaviour,ManagerBase
    {
        private static LoadManager instance;
        public static LoadManager GetInstance() {
            if (instance == null)
            {
                instance = new LoadManager();
            }
            return instance;
        }
        public void IStart()
        {
            //StartCoroutine(LoadConfig());
        }
        public InitConfig initConfig;
        void Awake()
        {
            instance = this;
            StartCoroutine(InitConfig());
        }
        IEnumerator InitConfig() {
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(Application.streamingAssetsPath + "/config.json");
            yield return unityWebRequest.SendWebRequest();
            if (unityWebRequest.isDone && !unityWebRequest.isHttpError)
            {
                string config = unityWebRequest.downloadHandler.text;
                StartCoroutine(Load(config));
            }
        }
        IEnumerator Load(string _url) {
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(_url);
            yield return unityWebRequest.SendWebRequest();
            if (unityWebRequest.isDone && !unityWebRequest.isHttpError)
            {
                string config = unityWebRequest.downloadHandler.text;
                initConfig = JsonUtility.FromJson<InitConfig>(config);
                GetComponent<TManagerBase>().LoadCAD(initConfig.cadURL);
                Two.Element.ElementContainer.GetInstance().matLine.color = ColorToRGBA.GetColor(initConfig.cadColor);
                Two.Manager.GetInstance().limitConfig = initConfig.limit;
                Two.Manager.GetInstance().InitJH();
            }
        }
    }
}

