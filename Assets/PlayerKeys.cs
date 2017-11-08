using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeys : MonoBehaviour {

	int keyNum;
	[SerializeField] GameObject key;
	[SerializeField] UIScript ui;

	// Use this for initialization
	void Start () {
		keyNum = 0;
	}
		
	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Key") {
			Destroy (col.gameObject);
			keyNum++;
			ui.pickUpKey ();
			print ("FOUND A KLEY");
		}
	}

	public void reset(){
		print ("OH NO");
		ui.noMoreKeys ();
		Instantiate (key, transform.position,transform.rotation);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
