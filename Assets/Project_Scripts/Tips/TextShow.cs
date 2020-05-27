using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TextShow : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public Transform textContent;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnMouseEnter() {
        Debug.Log("222222222");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("11111111");
        textContent.gameObject.SetActive(true);
        textContent.position = Input.mousePosition;
        textContent.GetChild(0).GetComponent<Text>().text = GetComponent<Text>().text;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textContent.gameObject.SetActive(false);
    }
}
