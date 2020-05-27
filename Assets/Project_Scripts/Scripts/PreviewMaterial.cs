using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewMaterial : MonoBehaviour {
    private JToken tempJT;
    public Material top_mat, side_mat, bottom_mat;
    private string bottom_url, top_url, side_url;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void GetTextureByURL(string request)
    {
        TextureData textureData = JsonUtility.FromJson<TextureData>(request);
        if (textureData.sideType == "top")
        {
            top_url = textureData.textureURL;
            CreateMatByTexture(LoadTexture(top_url), top_mat);
        }
        if (textureData.sideType == "side")
        {
            side_url = textureData.textureURL;
            CreateMatByTexture(LoadTexture(side_url), side_mat);
        }
        if (textureData.sideType == "bottom")
        {
            bottom_url = textureData.textureURL;
            CreateMatByTexture(LoadTexture(bottom_url), bottom_mat);
        }
    }
    private void CreateMatByTexture(Texture texture,Material mat)
    {
        mat.mainTexture = texture;
    }
    Texture LoadTexture(string _url)
    {
        Texture texture_Temp;
        WWW www = new WWW(_url);
        if (www.isDone && string.IsNullOrEmpty(www.error))
        {
            texture_Temp = www.texture;
            return texture_Temp;
        }
        return null;
    }
}
[System.Serializable]
public class TextureData
{
    public string sideType;
    public string textureURL;
}
