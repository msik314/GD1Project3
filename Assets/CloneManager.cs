using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneManager : MonoBehaviour
{
    [SerializeField] private float maxLife;
    [SerializeField] private GameObject clone;
    [SerializeField] int livesBeforeReset;
    
    private float life;
    private MovementController player;
    private MovementRecord playerMr;
    [SerializeField]private List <CloneController> ccs;
    private Vector3 originalPos;
    private Quaternion originalRot;
    
    // Use this for initialization
    void Awake()
    {
        life = maxLife;

        GameObject p = GameObject.FindWithTag("Player");
        playerMr = p.GetComponent<MovementRecord>();
        player = p.GetComponent<MovementController>();
        player.setManager(this);
        ccs = new List<CloneController>();
        originalPos = player.transform.position;
        originalRot = player.transform.rotation;
    }
    
    // Update is called once per frame
    void Update()
    {
        life -= Time.deltaTime;
        if(life <= 0)
        {
            cycle();
        }
    }
    
    public void cycle()
    {
        if(ccs.Count >= livesBeforeReset - 1)
        {

            for(int i = ccs.Count - 1; i >= 0; --i)
            {
                Destroy(ccs[i].gameObject);
            }
            clear();
        }
        else
        {
            foreach(CloneController clone in ccs)
            {
                clone.reset();
            }
            GameObject c = (GameObject)Instantiate(clone, originalPos, originalRot);
            c.GetComponent<MovementRecord>().copy(playerMr);
            ccs.Add(c.GetComponent<CloneController>());
        }
        player.reset();
        life = maxLife;
    }

	public float getLife(){
		return life;
	}

	public float getMaxLife(){
		return maxLife;
	}
    
    void clear()
    {
        ccs.Clear();
    }
    
    public void reset()
    {
        for(int i = ccs.Count - 1; i >= 0; --i)
        {
            Destroy(ccs[i].gameObject);
        }
        clear();
        player.reset();
        life = maxLife;
    }
}
