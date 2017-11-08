﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControl : InteractControl {

    public float Duration;

    private bool vulnerable = false;
    private float startTime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (vulnerable){
            if(Time.time - startTime > Duration)
            {
                print("resetting");
                vulnerable = false;
            }
        }
	}

    public override void doInteraction(Transform player) {
        if(!vulnerable){
            print("setting");
            vulnerable = true;
            startTime = Time.time;
        }
        else if (vulnerable){
            print("out");
        }
    }

    public void reset(){
        vulnerable = false;
    }
}