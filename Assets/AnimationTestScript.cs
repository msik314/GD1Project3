using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTestScript : MonoBehaviour {

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		print (anim);

	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown (KeyCode.L)) {
			print ("DEBUG: Pressed L");
			//anim.Play ("Death");
			anim.SetFloat("timeRemaining", 0);
		}
		if (Input.GetKeyDown (KeyCode.O)) {
			print ("DEBUG: Pressed O");
			//anim.Play ("Death");
			anim.SetFloat("curVelocity", 2);
		}
	}
}
