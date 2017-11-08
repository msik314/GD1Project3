using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInteract : InteractControl {

	[SerializeField] TextboxScript txtscript;
	[SerializeField] string textToPrint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public override void doInteraction(Transform player) {
		if (player.gameObject.tag == "Player") {
			txtscript.setText (textToPrint);
		}
	
	}
}
