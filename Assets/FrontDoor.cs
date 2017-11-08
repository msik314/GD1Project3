using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FrontDoor : InteractControl {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void doInteraction(Transform player)
    {
        if (player.gameObject.GetComponent<PlayerKeys>().keyNum >= 3) {
            print("game's done!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
