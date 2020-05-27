using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointClick : MonoBehaviour {
    public TextMesh textMesh;
    public GameObject panelName;
    public InputField inputName;
    public Button btn_sure;
	// Use this for initialization
	void Awake () {
        panelName = GameObject.Find("Canvas").transform.Find("SetName").gameObject;
        btn_sure = panelName.transform.GetChild(0).GetComponent<Button>();
        inputName = panelName.transform.GetChild(1).GetComponent<InputField>();
        textMesh = transform.GetChild(0).GetComponent<TextMesh>();

	}
}
