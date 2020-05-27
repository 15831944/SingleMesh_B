using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class TestElement : MonoBehaviour
{
    public GameObject first, second;
    private Button firstBtn;
    public GameObject secondPanel, secondLine;

    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        InitView();
    }
    void InitView()
    {
        firstBtn = first.GetComponent<Button>();
        firstBtn.onClick.AddListener(delegate () { SetUIByLevel(1); });
    }
    private void OnEnable()
    {
        SetUIByLevel(0);
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 vector3Temp = Camera.main.WorldToScreenPoint(target.position);
        transform.position = new Vector3(vector3Temp.x, vector3Temp.y, 0);
    }
    
    /// <summary>
    /// 根据当前层级显示UI
    /// </summary>
    /// <param name="level">0-第一层级（默认层级），1-第二层级</param>
    public void SetUIByLevel(int level)
    {

        switch (level)
        {
            case 0:
                Debug.Log("显示第一层级");
                secondPanel.SetActive(false);
                secondLine.GetComponent<Image>().DOFillAmount(0, 0.3f).OnComplete(delegate ()
                {
                    first.SetActive(level == 0 ? true : false);
                    second.SetActive(level == 1 ? true : false);
                }).SetEase(Ease.Linear);
                break;
            case 1:
                Debug.Log("显示第二层级");
                foreach (Transform item in transform.parent)
                {
                    if (item != this.transform)
                    {
                        item.GetComponent<TestElement>().SetUIByLevel(0);
                    }
                }
                first.SetActive(level == 0 ? true : false);
                second.SetActive(level == 1 ? true : false);
                secondLine.GetComponent<Image>().DOFillAmount(1, 0.3f).OnComplete(delegate ()
                {
                    secondPanel.SetActive(true);
                }).SetEase(Ease.Linear);
                break;
            default:
                break;
        }
    }
}