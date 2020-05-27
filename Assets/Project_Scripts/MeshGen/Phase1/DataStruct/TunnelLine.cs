using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelLine : MonoBehaviour
{
    public Material normalMat, tunnelMat;
    public Transform line;
    public Transform lineXP;
    // Start is called before the first frame update
    void Start()
    {
        line = transform.GetChild(0);
        line.GetComponent<MeshRenderer>().material = new Material(tunnelMat);
        line.GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(1, line.parent.localScale.z / 10));
        DOTween.To(() => line.GetComponent<MeshRenderer>().material.mainTextureOffset, x =>
                            line.GetComponent<MeshRenderer>().material.mainTextureOffset = x, new Vector2(0, 1), 1).SetLoops(-1).SetEase(Ease.Linear);
    }
    public void Set(Line lineTemp) {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
