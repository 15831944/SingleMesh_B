using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestElementPoint : MonoBehaviour
{
    public TestElement testElement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        testElement.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        testElement.gameObject.SetActive(true);
    }
}
