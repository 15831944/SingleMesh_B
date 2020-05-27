using BS.BL.Manager;
using BS.BL.Two.Element;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BS.BL.Two.Loader {
    public class LoadConfig
    {
        public LoadConfig() {

        }
        /// <summary>
        /// 通过configType加载sprite
        /// </summary>
        /// <param name="diceTypeSprites"></param>
        /// <param name="configType"></param>
        /// <returns></returns>
        public IEnumerator LoadPhoto(Dictionary<string, Sprite> diceTypeSprites, ConfigType configType)
        {
            UnityWebRequest wr = new UnityWebRequest(configType.data.icon);
            DownloadHandlerTexture texD1 = new DownloadHandlerTexture(true);
            wr.downloadHandler = texD1;
            yield return wr.SendWebRequest();
            int width = 1920;
            int high = 1080;
            if (!wr.isNetworkError && wr.isDone && texD1.isDone)
            {
                Texture2D tex = new Texture2D(width, high);
                tex = texD1.texture;

                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                if (!diceTypeSprites.ContainsKey(configType.eCode))
                {
                    diceTypeSprites.Add(configType.eCode, sprite);
                }
            }
        }
        /// <summary>
        /// 通过路径加载sprite
        /// </summary>
        /// <param name="_iconUrl"></param>
        /// <param name="iconS"></param>
        /// <returns></returns>
        public IEnumerator LoadPhoto(string _iconUrl, Action<Sprite> actionS) {
            UnityWebRequest wr = new UnityWebRequest(_iconUrl);
            DownloadHandlerTexture texD1 = new DownloadHandlerTexture(true);
            wr.downloadHandler = texD1;
            yield return wr.SendWebRequest();
            int width = 1920;
            int high = 1080;
            if (!wr.isNetworkError && wr.isDone && texD1.isDone)
            {
                Texture2D tex = new Texture2D(width, high);
                tex = texD1.texture;
                if (actionS != null)
                {
                    actionS(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f)));
                }
            }
        }
    }
}

