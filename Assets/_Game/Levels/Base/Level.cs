using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Enemy;
using Dreamteck.Splines;
using Sirenix.OdinInspector;
using UnityEngine;
using Vanta.Levels;

public class Level : BaseLevel
{
    public SplineComputer spline;
    public Player player => _player as Player;

    [SerializeField] private BaseEnemy enemy;
    [SerializeField] private int spawnCount;


    #region Life Cycle

    private void OnValidate()
    {
    }

    protected override void Start()
    {
        base.Start();

        // TODO: Initialize level

        _state = State.Loaded;
        listener.Level_DidLoad(this);
        Debug.Log("Loaded");
        _state = State.Started;
        listener.Level_DidStart(this);
        Aaa();
    }

    [Button]
    private void SpawnEnemy()
    {
        var e = Instantiate(enemy);
        e.transform.parent = transform;
    }

    [Button]
    private void Aaa()
    {
        StartCoroutine(SpawnMany());
    }

    private IEnumerator SpawnMany()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var e = Instantiate(enemy);
            e.transform.parent = transform;
            yield return new WaitForSeconds(.5f);
        }
    }

    #endregion
}