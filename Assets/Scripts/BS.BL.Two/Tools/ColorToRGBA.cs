using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BS.BL.Two
{
    public class ColorToRGBA
    {
        public static Color GetColor(string _color) {
            Color nowColor;
            ColorUtility.TryParseHtmlString(_color, out nowColor);
            return nowColor;
        }
    }
}
