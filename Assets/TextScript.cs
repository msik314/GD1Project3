using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{
    [SerializeField] private float timeFadeIn;
    [SerializeField] private float LengthFadeIn;
    [SerializeField] private float timeFadeOut;
    [SerializeField] private float LengthFadeOut;
    [SerializeField] private AnimationCurve fadeCurve;
    
    private float time;
    private Text text;
    
    // Use this for initialization
    void Awake()
    {
        text = GetComponent<Text>();
        time = 0;
        
        Color color = text.color;
        color.a = fadeCurve.Evaluate(0);
        text.color = color;
    }
    
    // Update is called once per frame
    void Update()
    {
        Color color = text.color;
        if(time < timeFadeIn)
        {
            color.a = fadeCurve.Evaluate(0);
        }
        else if(time <= timeFadeIn + LengthFadeIn)
        {
            float prc = (time - timeFadeIn)/LengthFadeIn;
            color.a = fadeCurve.Evaluate(prc);
        }
        else if(time < timeFadeOut)
        {
            color.a = fadeCurve.Evaluate(1);
        }
        else if(time <= timeFadeOut + LengthFadeOut)
        {
            float prc = (time - timeFadeOut)/LengthFadeOut;
            color.a = fadeCurve.Evaluate(1 - prc);
        }
        else
        {
             color.a = fadeCurve.Evaluate(0);
        }
        text.color = color;
        time += Time.deltaTime;
    }
}
