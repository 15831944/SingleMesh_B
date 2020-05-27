using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScroll : MonoBehaviour {

    //设置ScrollRect变量

    ScrollRect rect;

    void Start()

    {

        //获取 ScrollRect变量
        //transform.GetChild(0).GetComponent<Text>().text = "我是你爸爸{{{{{{；；；；；、1111111111111111、";
        //transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(("我是你爸爸{{{{{{；；；；；、1111111111111111、".Length / 10 + 1) * 16, 0);

        rect = this.GetComponent<ScrollRect>();

    }

    void Update()

    {

        //在Update函数中调用ScrollValue函数

        ScrollValue();

    }

    private void ScrollValue()

    {

        //当对应值超过1，重新开始从 0 开始

        if (rect.horizontalNormalizedPosition > 1.0f)

        {

            rect.horizontalNormalizedPosition = 0;

        }

        //逐渐递增 ScrollRect 水平方向上的值

        rect.horizontalNormalizedPosition = rect.horizontalNormalizedPosition + 0.05f * Time.deltaTime;

    }
}
