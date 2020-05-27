using BS.BL.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ResTest : MonoBehaviour
{
    private string tempEntity, tempPosition;
    public Button btnInit, btnAdd, btnRefresh, btnDominateRes,
        btnDominateType, btnDominateTunnel, btnDominateArea,
        btnTunnelLine, btnWay, btnWayEdit, btnDominateMode,
        btnShowGround, btnInspection, btnFrameSelection, btnTrack;
    public Slider slider;
    public Transform _panel;
    // Start is called before the first frame update
    private static ResTest instance;
    public static ResTest GetInstance() {
        if (instance == null)
        {
            instance = new ResTest();
        }
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //Invoke("LoadInit", 1);
        //InvokeRepeating("LoadEntity", 2, 3);
        btnInit.onClick.AddListener(delegate () {
            StartCoroutine(loadInit());
        });
        btnAdd.onClick.AddListener(delegate () {
            StartCoroutine(loadEntity());
        });
        slider.onValueChanged.AddListener(delegate (float degree) {
            GameObject.Find("JSInterface").GetComponent<JSInterface>().setTrackDegree(degree);
        });
        btnTrack.onClick.AddListener(delegate () { StartCoroutine(loadBack()); });
        //btnRefresh.onClick.AddListener(delegate () {
        //    StartCoroutine(loadPosition());
        //});

    }
    public void SetImageAndText(bool _active, string _text) {
        _panel.gameObject.SetActive(_active);
        _panel.Find("Text").GetComponent<Text>().text = _text;
    }
    void LoadInit() {
        StartCoroutine(loadInit());
    }
    void LoadEntity() {
        StartCoroutine(loadEntity());
    }
    IEnumerator loadInit() {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(Application.streamingAssetsPath + "/initData.json");
        yield return unityWebRequest.SendWebRequest();
        if (unityWebRequest.isDone)
        {
            tempEntity = unityWebRequest.downloadHandler.text;
            GameObject.Find("JSInterface").GetComponent<JSInterface>().initBasicData(tempEntity);
            Debug.Log("初始化数据加载成功");
        }
    }
    IEnumerator loadBack()
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(Application.streamingAssetsPath + "/back.json");
        yield return unityWebRequest.SendWebRequest();
        if (unityWebRequest.isDone)
        {
            tempEntity = unityWebRequest.downloadHandler.text;
            GameObject.Find("JSInterface").GetComponent<JSInterface>().trackPlayBack(tempEntity);
            Debug.Log("轨迹回放成功");
        }
    }
    IEnumerator loadEntity() {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(Application.streamingAssetsPath + "/entity.json");
        yield return unityWebRequest.SendWebRequest();
        if (unityWebRequest.isDone)
        {
            tempEntity = unityWebRequest.downloadHandler.text;
            GameObject.Find("JSInterface").GetComponent<JSInterface>().refreshRes(tempEntity);
            Debug.Log("实体数据加载成功");
        }
    }
    IEnumerator loadPosition()
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(Application.streamingAssetsPath + "/position.json");
        yield return unityWebRequest.SendWebRequest();
        if (unityWebRequest.isDone)
        {
            tempPosition = unityWebRequest.downloadHandler.text;
            GameObject.Find("JSInterface").GetComponent<JSInterface>().refreshResEntity(tempPosition);
            Debug.Log("位置数据加载成功");
        }
    }
}
[System.Serializable]
public class ResTemp
{
    public string id;
    public string road;
    public float distance;
}
[System.Serializable]
public class ResList {
    public List<ResTemp> resTemps;
}
