using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SelectImage : MonoBehaviour, IPointerDownHandler
{
    public GameObject in_prefab;
    //实例化后的对象
    private GameObject inistateObj;
    private void Start()
    {
        inistateObj = Instantiate(in_prefab);
    }
    /// <summary>
    /// 加载模型文件
    /// </summary>
    /// <param name="_path"></param>
    public void InstanctPrefab(string _path)
    {
        inistateObj = Instantiate(in_prefab);
        inistateObj.SetActive(true);
        //StartCoroutine(LoadPrefab("http://127.0.0.1:8080/modal/modal.assetbundle"));
    }
    IEnumerator LoadPrefab(string _path)
    {
        WWW www = new WWW(_path);
        yield return www;
        if(www.isDone && string.IsNullOrEmpty(www.error))
        {
            AssetBundle assetbundle = www.assetBundle;
            inistateObj = Instantiate(assetbundle.LoadAsset<GameObject>("Person.prefab"));
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.LogError("1111111111" + Input.mousePosition);
        }
    }
    //实现鼠标按下的接口
    public void OnPointerDown(PointerEventData eventData)
    {
        inistateObj.SetActive(true);

        //将当前需要被实例化的对象传递到管理器中
        SelectObjManager.Instance.AttachNewObject(inistateObj);
    }
}