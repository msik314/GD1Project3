using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRecord : MonoBehaviour
{
    [SerializeField] private List<Vector3> targetVelocities;
    [SerializeField] private List<Quaternion> targetOrientations;
    private int index;
    
    // Use this for initialization
    void Start ()
    {
        targetVelocities = new List<Vector3>();
        targetOrientations = new List<Quaternion>();
        index = 0;
    }
    
    
    public void getTarget(out Vector3 target, out Quaternion orientation)
    {
        if(index < 0 || index >= targetVelocities.Count)
        {
            target = Vector3.zero;
            orientation = Quaternion.identity;
        }
        else
        {
            target = targetVelocities[index];
            orientation = targetOrientations[index];
            ++index;
        }
    }
    
    public void addTarget(Vector3 target, Quaternion orientation)
    {
        targetVelocities.Add(target);
        targetOrientations.Add(orientation);
    }
    
    public void reset()
    {
        index = 0;
    }
    
    public void clear()
    {
        targetVelocities = new List<Vector3>();
        targetOrientations = new List<Quaternion>();
    }
}
