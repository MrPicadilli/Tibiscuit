using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class PlayerState
{
    protected PlayerStateData stateData;

    public PlayerState(PlayerStateData data)
    {
        stateData = data;
    }

    public Material GetMaterial()
    {
        return stateData.stateMaterial;
    }
    public float GetSpeed()
    {
        return stateData.stateSpeed;
    }
}



public class NormalState : PlayerState
{
    public NormalState(PlayerStateData data) : base(data) { }
}



public class HumidState : PlayerState
{
    public HumidState(PlayerStateData data) : base(data) { }
}


public class DryState : PlayerState
{
    public DryState(PlayerStateData data) : base(data) { }
}