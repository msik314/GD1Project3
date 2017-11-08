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
		}
	}

	public void reset(){
		print ("OH NO");
		ui.noMoreKeys ();
		for (int i = 0; i < keyNum; i++) {
			Instantiate (key, new Vector3 (transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation);
		}
		keyNum = 0;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
