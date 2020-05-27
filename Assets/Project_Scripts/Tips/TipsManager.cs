using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsManager : MonoBehaviour {
    public GameObject licenseText;
    public Button btnCancel, btnSure;
    public Text textContent;
	// Use this for initialization
	void Start () {
        btnCancel.onClick.AddListener(delegate () { CancelEvent(); });
        btnSure.onClick.AddListener(delegate () { SureEvent(); });
    }
    private void CancelEvent()
    {
        gameObject.SetActive(false);
    }
    private void SureEvent() {
        Debug.Log("推出");
        Application.Quit();
    }
    public void SetTextContent(string _content) {
        textContent.text = _content;
    }
}
