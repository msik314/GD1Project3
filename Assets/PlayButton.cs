using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    int next;
    // Use this for initialization
    void Awake()
    {
        next = SceneManager.GetActiveScene().buildIndex  + 1;
    }
    
    public void onClick()
    {
        SceneManager.LoadScene(next);
    }
}
