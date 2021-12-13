using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;
using Vanta.Levels;


public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField] private EnemyProperties enemyProperties;

    [field: SerializeField] public SplineFollower splineFollower { private set; get; }
    // [SerializeField] protected SplineFollower splineFollower;
    [SerializeField] protected float health;
    private IEnumerator _currentDebuff = null;

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
        if(health > 0) health -= dmg;
    }

    #region GetStatusEffects

    public void GetStatusEffect(BaseTower.Type towerType)
    {
        StopAllCoroutines();

        _currentDebuff = towerType switch
        {
            BaseTower.Type.Ice => GetFreezeEffect(),
            BaseTower.Type.Fire => GetFireEffect(),
            BaseTower.Type.Magic => GetMagicEffect(),
            BaseTower.Type.Teleport => GetTeleportEffect(),
            _ => null
        };

        if (_currentDebuff == null) return;
        StartCoroutine(_currentDebuff);
    }

    #endregion

    #region StatusEffects

    private IEnumerator GetFreezeEffect()
    {
        yield return null;
    }

    private IEnumerator GetFireEffect()
    {
        yield return null;
    }

    private IEnumerator GetMagicEffect()
    {
        yield return null;
    }

    private IEnumerator GetTeleportEffect()
    {
        yield return null;
    }

    #endregion
   
}
