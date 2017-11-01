using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextboxScript : MonoBehaviour {



	private Text curText;
	private float timer;
	public GameObject textBox;

	// Use this for initialization
	void Start () {
		timer = 0;
		curText = textBox.GetComponent<Text> ();
	}

	public void setText(string givenText){
		curText.text = givenText;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
