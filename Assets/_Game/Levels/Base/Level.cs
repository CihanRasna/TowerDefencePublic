using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vanta.Levels;

public class Level : BaseLevel
{
    
    // Override player property of Base Level to return Player instead of Base Character.
    public Player player => _player as Player;
    
    
    
#region Life Cycle

    protected override void Start()
    {
        base.Start();
        
        // TODO: Initialize level

        _state = State.Loaded;
        listener.Level_DidLoad(this);
        Debug.Log("Loaded");
    }

#endregion
    
}
