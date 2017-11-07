using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : InteractControl {

    private bool isGrabbed = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public override void doInteraction(Transform player){
        if (!isGrabbed){
            transform.position = player.position + new Vector3(0,5,0);
            transform.parent = player;
            isGrabbed = true;
        }
        if (isGrabbed){

        }
    }
}
