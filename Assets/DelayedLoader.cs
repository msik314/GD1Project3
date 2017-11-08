using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DelayedLoader : MonoBehaviour
{
    [SerializeField] private float loadDelay;
    // Use this for initialization
    
    float time = 0;
    void Awake()
    {
    }
    
    // Update is called once per frame
    void Update()
    {
        if(time >= loadDelay)
        {
            SceneManager.LoadScene(1);
        }
        time += Time.deltaTime;
    }
}
