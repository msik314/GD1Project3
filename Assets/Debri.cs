using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debri : InteractControl {
    public Vector3 down;
    public Vector3 up;
    public int rate;
    public float duration;

    private bool goingUp = false;
    private bool goingDown = false;
    private float timeStart;
    
    private float i = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (goingUp){
            i += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(down, up, i);
            if(Time.time - timeStart > duration){
                goingUp = false;
                goingDown = true;
                i = 0;
            }
        }
        else if(goingDown){
            i += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(up, down, i);
            if(i >= 1){
                goingDown = false;
                i = 0;
            }
        }
	}

    public override void doInteraction(){
        if (!goingUp && !goingDown){
            goingUp = true;
            timeStart = Time.time;
        }
    }
}
