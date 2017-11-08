using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void doInteraction(Transform player) {
        print("interact");
    }
    
    public virtual void reset(){}
}
