using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextboxScript : MonoBehaviour {

	public GameObject textBox;

	private Text curText;
	private float timer;
	private Color txtBoxInitClr;

	private float fadeTime = 1.25f;
	private float duration = 3;
	private float curTime = 0;


	private bool curFading = false;

	// Use this for initialization
	void Start () {
		timer = 0;
		curText = textBox.GetComponentInChildren<Text> ();
		textBox.GetComponent<Image>().CrossFadeAlpha(0,0,false);
		curText.CrossFadeAlpha(0,0,false);

	}

	public void setText(string givenText){
		curText.text = givenText;
		curTime = Time.time;
		fade ();
	}

	private void fade(){
		if (curFading == false) {
			curFading = true;

			textBox.GetComponent<Image>().CrossFadeAlpha(1,fadeTime,false);
			curText.CrossFadeAlpha(1,fadeTime,false);
			
		}
	}

	void Update(){
		if (curFading) {
			if (Time.time > curTime + fadeTime + duration) {
				textBox.GetComponent<Image>().CrossFadeAlpha(0,fadeTime,false);
				curText.CrossFadeAlpha(0,fadeTime,false);
				curFading = false;
			}
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			setText ("Testing! Pressed P!");
		}
	}


	

}
