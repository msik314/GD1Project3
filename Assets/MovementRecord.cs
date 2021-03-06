﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRecord : MonoBehaviour
{
    private List<Vector3> targetVelocities;
    private List<Vector2> targetOrientations;
    private List<byte> actions;
    private int index;
    
    public void init()
    {
        targetVelocities = new List<Vector3>();
        targetOrientations = new List<Vector2>();
        actions = new List<byte>();
        index = 0;
    }
    
    
    public byte getTarget(out Vector3 target, out Vector2 orientation)
    {
        if(index < 0 || index >= targetVelocities.Count)
        {
            target = Vector3.zero;
            orientation = targetOrientations[targetOrientations.Count - 1];
            return 0;
        }
        else
        {
            target = targetVelocities[index];
            orientation = targetOrientations[index];
            ++index;
            return actions[index - 1];
        }
    }
    
    public void addTarget(Vector3 target, Vector2 orientation, byte acting)
    {
        targetVelocities.Add(target);
        targetOrientations.Add(orientation);
        actions.Add(acting);
    }
    
    public void reset()
    {
        index = 0;
    }
    
    public void clear()
    {
        targetVelocities = new List<Vector3>();
        targetOrientations = new List<Vector2>();
        actions = new List<byte>();
    }
    
    public void getLists(out List<Vector3> targets, out List<Vector2> orientations, out List<byte>actionsOut)
    {
        targets = targetVelocities;
        orientations = targetOrientations;
        actionsOut = actions;
    }
    
    public void copy(MovementRecord mr)
    {
        mr.getLists(out targetVelocities, out targetOrientations, out actions);
    }
}
