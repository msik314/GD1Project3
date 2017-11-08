using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIScript : MonoBehaviour {
	[SerializeField] Text title;
	[SerializeField] GameObject textBox;
	// Use this for initialization
	void Start () {
		textBox.transform.position = new Vector3 (Screen.width/2,
			Screen.height *3/ 4);
		title.transform.position = new Vector3 (Screen.width/2,
			Screen.height *3/ 4);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
