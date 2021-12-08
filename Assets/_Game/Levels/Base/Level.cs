using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;
using Vanta.Levels;

public class Level : BaseLevel
{
    public SplineComputer spline;
    public Player player => _player as Player;

    #region Life Cycle

    protected override void Start()
    {
        base.Start();
        
        // TODO: Initialize level

        _state = State.Loaded;
        listener.Level_DidLoad(this);
        Debug.Log("Loaded");
        _state = State.Started;
        listener.Level_DidStart(this);
    }

#endregion
    
}
