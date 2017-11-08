using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour {

	public GameObject textBox, lifeSpan, lives,key;

	private int border = 20;
	private int curKeyNum = 0;
	private List<GameObject> keys;

	// Use this for initialization
	void Start () {
		keys = new List<GameObject> ();
		textBox.transform.position = new Vector3 (Screen.width/2 - border, Screen.height - textBox.GetComponent<RectTransform>().rect.height/2 - border, 0);

		lifeSpan.transform.position = new Vector3 (Screen.width - lifeSpan.GetComponent<RectTransform>().rect.width/2 - border, 
		Screen.height - lifeSpan.GetComponent<RectTransform>().rect.height/2 - border, 0);

		lives.transform.position = new Vector3(lives.GetComponent<RectTransform>().rect.width - border,
			lives.GetComponent<RectTransform>().rect.height - border, 0);

		loadKeys ();

		key.SetActive (false);
	}
	public void pickUpKey(){
		curKeyNum++;
		loadKeys ();
	}
	public void noMoreKeys(){
		curKeyNum = 0;
		loadKeys ();
	}
	private void loadKeys(){
		for (int i = 0; i < curKeyNum; i++) {
			if (keys.Count <= i) {
				GameObject tempKey = Instantiate (key, key.transform.parent);
				keys.Add (tempKey);
			} 
				keys [i].SetActive (true);

			keys[i].transform.position = new Vector3 (key.GetComponent<RectTransform> ().rect.width - border / 2 + key.GetComponent<RectTransform> ().rect.width * i,
				lives.GetComponent<RectTransform> ().rect.height / 2 +
				lives.transform.position.y / 2 +
				key.GetComponent<RectTransform> ().rect.height, 0);

		}
		for (int i = curKeyNum; i < keys.Count; i++)
			keys [i].SetActive (false);
	}
}
