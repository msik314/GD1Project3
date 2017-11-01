using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TestItem : MonoBehaviour
{
    private int numInteractions;
    private MeshRenderer r;
    // Use this for initialization
    void Awake ()
    {
        numInteractions = 0;
        r = GetComponent<MeshRenderer>();
        r.material.color = Color.green;
    }
    
    // Update is called once per frame
    void Update ()
    {
        if(numInteractions > 0)
        {
            r.material.color = Color.green;
        }
        else
        {
            r.material.color = Color.red;
        }
    }
    
    public void interact()
    {
        StartCoroutine(incNum());
    }
    
    IEnumerator  incNum()
    {
        ++numInteractions;
        yield return new WaitForSeconds(1);
        --numInteractions;
    }
}
