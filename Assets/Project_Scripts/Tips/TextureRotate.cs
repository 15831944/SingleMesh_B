using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureRotate {

    public static Texture2D HorizontalFlipPic(Texture2D texture2d)
    {
        int width = texture2d.width;  //图片原本的宽度
        int height = texture2d.height;  //图片原本的高度
        Texture2D newTexture = new Texture2D(height, width); //实例化一个新的texture，高度是原来的宽度，宽度是原来的高度
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Color color = texture2d.GetPixel(i, j);
                newTexture.SetPixel(-j, width - 1 - i, color);
            }
        }
        newTexture.Apply();
        return newTexture;
    }
}
