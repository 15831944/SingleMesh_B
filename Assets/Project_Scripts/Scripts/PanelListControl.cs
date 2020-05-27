using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelListControl : MonoBehaviour {
    private ScrollRect scrollRect;
    private Transform content, pageContent;
    private VerticalLayoutGroup verticalLayOutGroup;
	// Use this for initialization
	void Start () {
        scrollRect = GetComponent<ScrollRect>();
        content = scrollRect.content;
        verticalLayOutGroup = content.GetComponent<VerticalLayoutGroup>();
        pageContent = content.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () {
		if(pageContent.childCount > 7)
        {
            verticalLayOutGroup.childControlHeight = true;
        }
        else
        {
            verticalLayOutGroup.childControlHeight = false;
        }
	}
}
