using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;
using Vanta.Levels;


public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField] private EnemyProperties enemyProperties;

    [SerializeField] protected SplineFollower splineFollower;
    [SerializeField] protected float health;

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        InitializeEnemyLogic();
    }

    protected void InitializeEnemyLogic()
    {
        var level = LevelManager.Instance.currentLevel as Level;
        var currentSpline = level.spline;
        splineFollower.spline = currentSpline;
        splineFollower.followSpeed = enemyProperties.speed;
        health = enemyProperties.health;
    }

    public void TakeDamage(float dmg)
    {
        Debug.Log("asd");
        if(health > 0) health -= dmg;
    }
}
