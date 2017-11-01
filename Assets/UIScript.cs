using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour {

	public GameObject textBox, lifeSpan;
	int border = 20;

	// Use this for initialization
	void Start () {
		textBox.transform.position = new Vector3 (Screen.width/2 - border, Screen.height - textBox.GetComponent<RectTransform>().rect.height/2 - border, 0);
		lifeSpan.transform.position = new Vector3 (Screen.width - lifeSpan.GetComponent<RectTransform>().rect.width/2 - border, 
		Screen.height - lifeSpan.GetComponent<RectTransform>().rect.height/2 - border, 0);

	}
}
