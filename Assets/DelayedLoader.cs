using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DelayedLoader : MonoBehaviour {
    [SerializeField] private float loadDelay;
    // Use this for initialization
    int next;
    
    float time = 0;
    void Awake()
    {
        next = SceneManager.GetActiveScene().buildIndex  + 1;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(time >= loadDelay)
        {
            SceneManager.LoadScene(next);
        }
        time += Time.deltaTime;
    }
}
