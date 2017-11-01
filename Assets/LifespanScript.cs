using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifespanScript : MonoBehaviour {

	public GameObject lifespanObj;
	public GameObject cloneManager;
	private Image lsImg;
	private CloneManager clmgr;

	// Use this for initialization
	void Start () {
		lsImg = lifespanObj.GetComponent<Image> ();
		clmgr = cloneManager.GetComponent<CloneManager> ();


		initLifespanObj ();
	}

	private void initLifespanObj(){
		lifespanObj.GetComponent<Image> ().type = Image.Type.Filled;
		lifespanObj.GetComponent<Image> ().fillMethod = Image.FillMethod.Radial360;

	}
	
	// Update is called once per frame
	void Update () {
		lsImg.fillAmount = clmgr.getLife() / clmgr.getMaxLife ();
		//print (clmgr.getLife() / clmgr.getMaxLife ());

		if (Input.GetKeyDown (KeyCode.O)) {
			print (lifespanObj.GetComponent<Image> ().fillAmount);
			print (lifespanObj.GetComponent<Image> ().type);
			print (lifespanObj.GetComponent<Image> ().fillMethod);

		}

	}
}
