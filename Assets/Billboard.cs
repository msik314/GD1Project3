using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
    
    [SerializeField] private bool reverse;
    private Transform pTransform;
    // Use this for initialization
    void Awake()
    {
        pTransform = GameObject.FindWithTag("Player").transform;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(reverse)
        {
            transform.forward = -pTransform.forward;
        }
        else
        {
            transform.forward = pTransform.forward;
        }
    }
}
