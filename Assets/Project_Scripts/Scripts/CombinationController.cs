using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinationController : MonoBehaviour {
    private static CombinationController instance;
    public static CombinationController GetInstance()
    {
        if (instance == null)
        {
            instance = new CombinationController();
        }
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }
    //UI 的 prefab
    public GameObject prefab_taosheng, prefab_tongfeng, prefab_info, prefab_dice, prefab_dakong, prefab_info_d, tip, prefab_drawLine;

	

}
