using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneManager : MonoBehaviour
{
    [SerializeField] private float maxLife;
    [SerializeField] private GameObject clone;
    [SerializeField] int livesBeforeReset;
	[SerializeField] private UIScript ui;

    private float life;
    private MovementController player;
    private MovementRecord playerMr;
    [SerializeField]private List <CloneController> ccs;
    private Vector3 originalPos;
    private Quaternion originalRot;
    
    private List<InteractControl> interactables;
    bool hasFired;
    
    // Use this for initialization
    void Awake()
    {
        life = maxLife;
		ui.setRemainingLives (livesBeforeReset);
        GameObject p = GameObject.FindWithTag("Player");
        playerMr = p.GetComponent<MovementRecord>();
        player = p.GetComponent<MovementController>();
        player.setManager(this);
        ccs = new List<CloneController>();
        originalPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        originalRot = player.transform.rotation;
        interactables = new List<InteractControl>();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Interactable");
        foreach(GameObject g in objects)
        {
            interactables.Add(g.GetComponent<InteractControl>());
        }
        GameObject[] fires = GameObject.FindGameObjectsWithTag("Fire");
        foreach(GameObject g in fires)
        {
            interactables.Add(g.GetComponent<InteractControl>());
        }
        hasFired = false;
    }

    // Update is called once per frame
    void Update()
    {
        life -= Time.deltaTime;
        if(life <= 0 && !hasFired)
        {
            player.die();
            hasFired = true;
        }
    }

    public void cycle()
    {
        foreach(InteractControl i in interactables)
        {
            i.reset();
        }
        
        if(ccs.Count >= livesBeforeReset - 1)
        {
            for(int i = ccs.Count - 1; i >= 0; --i)
            {
                Destroy(ccs[i].gameObject);
            }
			ui.setRemainingLives (livesBeforeReset);
            clear();
        }
        else
        {
            foreach(CloneController clone in ccs)
            {
                clone.setOriginalPos(originalPos);
                clone.reset();
            }
            GameObject c = (GameObject)Instantiate(clone, originalPos, originalRot);
            c.GetComponent<MovementRecord>().copy(playerMr);
            ccs.Add(c.GetComponent<CloneController>());
			ui.setRemainingLives (livesBeforeReset - ccs.Count);
        }
        player.reset();
        life = maxLife;
        hasFired = false;
    }

	public float getLife(){
		return life;
	}

	public float getMaxLife(){
		return maxLife;
	}

	public int getLivesLeft()
	{
		return livesBeforeReset-ccs.Count;
	}

    void clear()
    {
        ccs.Clear();
    }

    public void reset()
    {
        foreach(InteractControl i in interactables)
        {
            i.reset();
        }
        
        for(int i = ccs.Count - 1; i >= 0; --i)
        {
            Destroy(ccs[i].gameObject);
        }
        clear();
        player.reset();
        life = maxLife;
        hasFired = false;
    }
}
