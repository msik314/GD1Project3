using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRecord : MonoBehaviour
{
    private List<Vector3> targetVelocities;
    private List<Quaternion> targetOrientations;
    private List<bool> jumps; 
    private int index;
    
    public void init()
    {
        targetVelocities = new List<Vector3>();
        targetOrientations = new List<Quaternion>();
        jumps = new List<bool>();
        index = 0;
    }
    
    
    public bool getTarget(out Vector3 target, out Quaternion orientation)
    {
        if(index < 0 || index >= targetVelocities.Count)
        {
            target = Vector3.zero;
            orientation = targetOrientations[targetOrientations.Count - 1];
            return false;
        }
        else
        {
            target = targetVelocities[index];
            orientation = targetOrientations[index];
            ++index;
            return jumps[index - 1];
        }
    }
    
    public void addTarget(Vector3 target, Quaternion orientation, bool jumping)
    {
        targetVelocities.Add(target);
        targetOrientations.Add(orientation);
        jumps.Add(jumping);
    }
    
    public void reset()
    {
        index = 0;
    }
    
    public void clear()
    {
        targetVelocities = new List<Vector3>();
        targetOrientations = new List<Quaternion>();
        jumps = new List<bool>();
    }
    
    public void getLists(out List<Vector3> targets, out List<Quaternion> orientations, out List<bool>jumpsOut)
    {
        targets = targetVelocities;
        orientations = targetOrientations;
        jumpsOut = jumps;
    }
    
    public void copy(MovementRecord mr)
    {
        mr.getLists(out targetVelocities, out targetOrientations, out jumps);
    }
}
