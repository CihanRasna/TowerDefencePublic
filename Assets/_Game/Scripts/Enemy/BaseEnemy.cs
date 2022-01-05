using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using EPOOutline;
using Sirenix.OdinInspector;
using UnityEngine;
using Vanta.Levels;

[SelectionBase]
public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField] private EnemyProperties enemyProperties;
    private StatusEffects statusEffects;

    [field: SerializeField] public SplineFollower splineFollower { private set; get; }
    [SerializeField] private Outlinable myOutline;

    [SerializeField] protected float health;
    protected int goldPrize;
    private IEnumerator _currentStatusEffect = null;

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
        InitializeEnemyLogic();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<BaseTower>(out var tower))
        {
            myOutline.enabled = false;
        }
    }

    private void InitializeEnemyLogic()
    {
        var level = LevelManager.Instance.currentLevel as Level;
        var currentSpline = level.spline;
        splineFollower.spline = currentSpline;
        splineFollower.followSpeed = enemyProperties.speed;
        health = enemyProperties.health;
        goldPrize = enemyProperties.goldPrize;
        statusEffects = enemyProperties.statusEffects;
    }

    public void TakeDamage(float dmg)
    {
        myOutline.enabled = true;
        health -= dmg;
        health = Mathf.Clamp(health, 0, enemyProperties.health);
        if (health == 0) Destroy(gameObject);
    }

    #region GetStatusEffects

    public void GetStatusEffect(BaseTower.Type towerType)
    {
        StopAllCoroutines();
        _currentStatusEffect = null;

        _currentStatusEffect = towerType switch
        {
            BaseTower.Type.Ice => GetFreezeEffect(),
            BaseTower.Type.Fire => GetFireEffect(),
            BaseTower.Type.Poison => GetPoisonEffect(),
            BaseTower.Type.Teleport => GetTeleportEffect(),
            _ => null
        };

        if (_currentStatusEffect == null) return;
        StartCoroutine(_currentStatusEffect);
    }

    #endregion

    #region StatusEffects

    private IEnumerator GetFreezeEffect()
    {
        var normalSpeed = enemyProperties.speed;
        var ratio = (100 - statusEffects.freezeRatio) / 100f;
        var newSpeed = normalSpeed * ratio;
        splineFollower.followSpeed = newSpeed;
        yield return new WaitForSeconds(statusEffects.freezeTime);
        splineFollower.followSpeed = normalSpeed;
    }

    private IEnumerator GetFireEffect()
    {
        var elapsedTime = 0.0f;
        var duration = statusEffects.burnTime;
        var burnRatio = statusEffects.burnRatio;
        var initialHealth = health;
        var healthRatio = initialHealth * 0.01f;
        
        while (true)
        {
            var progress = Mathf.Clamp01(elapsedTime / duration);
            health = initialHealth - (burnRatio * progress) * healthRatio;
            if (progress >= 1)
            {
                break;
            }

            yield return null;
            elapsedTime += Time.deltaTime;
        }
    }

    private IEnumerator GetPoisonEffect()
    {
        yield return null;
    }

    private IEnumerator GetTeleportEffect()
    {
        yield return null;
    }

    #endregion
}