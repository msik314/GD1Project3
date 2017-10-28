using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneManager : MonoBehaviour
{
    [SerializeField] private float maxLife;
    [SerializeField] private GameObject activate;
    [SerializeField] private Vector3 camEnd;
    [SerializeField] private Vector3 camEuler;
    
    private float life;
    private GameObject player;
    private MovementRecord playerMr;
    private MovementRecord cloneMr;
    private CloneController cc;
    
    // Use this for initialization
    void Awake()
    {
        life = maxLife;
        player = GameObject.FindWithTag("Player");
        playerMr = player.GetComponent<MovementRecord>();
        cloneMr = activate.GetComponent<MovementRecord>();
        cc = activate.GetComponent<CloneController>();
    }
    
    // Update is called once per frame
    void Update()
    {
        life -= Time.deltaTime;
        if(life <= 0)
        {
            cycle();
            life = maxLife;
        }
    }
    
    void cycle()
    {
        if(player)
        {
            cloneMr.copy(playerMr);
            Camera.main.transform.parent = null;
            Camera.main.transform.position = camEnd;
            Camera.main.transform.rotation = Quaternion.Euler(camEuler);
            Destroy(player);
            activate.SetActive(true);
        }
        else
        {
            cc.reset();
        }
    }
}
