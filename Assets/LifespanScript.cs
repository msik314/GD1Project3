using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifespanScript : MonoBehaviour {

	public GameObject clnObj;
	public GameObject textBox;
	private Text curText;
	private CloneManager clnMng;

	// Use this for initialization
	void Start () {
		curText = textBox.GetComponent<Text> ();
		clnMng = clnObj.GetComponent<CloneManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		curText.text = clnMng.getLife ().ToString ();
	}
}
