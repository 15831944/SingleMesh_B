using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BackGround : MonoBehaviour
{
    public Image imgBg;
    IEnumerator LoadBackGround(string _url)
    {
        UnityWebRequest wr = new UnityWebRequest(_url);
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
            imgBg.sprite = sprite;
        }

    }
    public void InitBackground(string imageURL)
    {
        StartCoroutine(LoadBackGround(imageURL));
    }
}
