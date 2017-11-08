using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour {

	public GameObject textBox, lifeSpan, lives,key;
	int border = 20;
	int curKeyNum = 3;

	// Use this for initialization
	void Start () {
		textBox.transform.position = new Vector3 (Screen.width/2 - border, Screen.height - textBox.GetComponent<RectTransform>().rect.height/2 - border, 0);

		lifeSpan.transform.position = new Vector3 (Screen.width - lifeSpan.GetComponent<RectTransform>().rect.width/2 - border, 
		Screen.height - lifeSpan.GetComponent<RectTransform>().rect.height/2 - border, 0);

		lives.transform.position = new Vector3(lives.GetComponent<RectTransform>().rect.width - border,
			lives.GetComponent<RectTransform>().rect.height - border, 0);
		
		for (int i = 0; i < curKeyNum; i++) {

			GameObject tempKey = Instantiate (key,key.transform.parent);
	

			tempKey .transform.position = new Vector3 (key.GetComponent<RectTransform> ().rect.width - border/2 + key.GetComponent<RectTransform> ().rect.width * i,
				lives.GetComponent<RectTransform> ().rect.height / 2 +
				lives.transform.position.y / 2 +
				key.GetComponent<RectTransform> ().rect.height, 0);
		}

		key.SetActive (false);
	}
}
