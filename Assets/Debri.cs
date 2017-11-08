using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debri : InteractControl {
    public Vector3 down;
    public Vector3 up;
    public float rate;
    public float duration;

    private bool goingUp = false;
    private bool goingDown = false;
    private bool isClone = false;
    private bool isPlayer = false;
    private MovementController user;
    private CloneController clone;
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
                if (isPlayer){
					
                    user.die();
                    isPlayer = false;
                }
                else if (isClone){
                    clone.die();
                    isClone = false;
                }
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

    public override void doInteraction(Transform player){

        if (!goingUp && !goingDown){
            goingUp = true;
            if(player.gameObject.tag == "Player")
            {
                user = player.gameObject.GetComponent<MovementController>();
                user.canMove = false;
                isPlayer = true;

            }
            else if (player.gameObject.tag == "Clone")
            {
                clone = player.gameObject.GetComponent<CloneController>();
                clone.canMove = false;
                isClone = true;
            }
            i = 0;
            timeStart = Time.time;
        }
		
	}

    public void reset(){
        transform.position = down;
        goingUp = false;
        goingDown = true;
    }
}
