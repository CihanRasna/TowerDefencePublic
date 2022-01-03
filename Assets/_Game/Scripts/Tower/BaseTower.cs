using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Vanta.Levels;

[SelectionBase]
public abstract class BaseTower : MonoBehaviour
{
    public enum Type
    {
        Arrow,
        Ice,
        Fire,
        Magic,
        Teleport
    }

    [ShowInInspector] protected Queue<BaseEnemy> _potentialNextEnemies = new Queue<BaseEnemy>();
    [HideInInspector] public Type towerType;
    [SerializeField] protected BaseEnemy currentEnemy;

    [SerializeField] public TowerProperties towerProperties;
    [SerializeField] protected Transform shootingPoint;
    [SerializeField] protected new SphereCollider collider;

    protected float damage;
    protected float firePerSecond;
    protected Projectile projectile;
    protected float projectileEffectZone;

    [HideInInspector] public int damageCurrentLevel = 1;
    [HideInInspector] public int fireRateCurrentLevel = 1;
    [HideInInspector] public int radiusCurrentLevel = 1;

    private IEnumerator _fireRoutine;
    private float _lastFireTime;

    private void OnEnable()
    {
        collider.radius = towerProperties.shootingRange;
    }

    protected virtual void Awake()
    {
        towerProperties = Instantiate(towerProperties);
    }

    protected virtual void Start()
    {
        InitializeTowerProperties();
        _fireRoutine = RepeatFire();
    }

    protected virtual void Update()
    {
        _lastFireTime += Time.deltaTime;
        if (!currentEnemy && _potentialNextEnemies.Count > 0)
        {
            if (_potentialNextEnemies.Peek() != null)
            {
                if (_fireRoutine != null)
                {
                    StopCoroutine(_fireRoutine);
                    _fireRoutine = null;
                }

                currentEnemy = _potentialNextEnemies.Peek();
                StartCoroutine(_fireRoutine = RepeatFire());
            }
            else
            {
                _potentialNextEnemies.Dequeue();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BaseEnemy>(out var enemy))
        {
            if (!_potentialNextEnemies.Contains(enemy))
            {
                _potentialNextEnemies.Enqueue(enemy);
            }
            // if (!currentEnemy)
            // {
            //     currentEnemy = enemy;
            //     _fireRoutine = null;
            //     StartCoroutine(_fireRoutine = RepeatFire());
            //     TowerHasTarget();
            // }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<BaseEnemy>(out var enemy)) return;

        if (_potentialNextEnemies.Count > 0)
        {
            _potentialNextEnemies.Dequeue();
        }

        if (_fireRoutine != null)
        {
            StopCoroutine(_fireRoutine);
            _fireRoutine = null;
        }

        currentEnemy = null;
    }

    private void InitializeTowerProperties()
    {
        damageCurrentLevel = 1;
        fireRateCurrentLevel = 1;
        radiusCurrentLevel = 1;
        towerType = towerProperties.towerType;
        damage = towerProperties.damage;
        collider.radius = towerProperties.shootingRange;
        firePerSecond = towerProperties.fireRate;
        projectile = towerProperties.projectile;
        projectileEffectZone = towerProperties.projectileEffectZone;
    }

    private IEnumerator RepeatFire()
    {
        yield return new WaitUntil(() => _lastFireTime > 1 / firePerSecond);
        Debug.Log(Time.time - _lastFireTime);
        _lastFireTime = 0f;
        var e = currentEnemy;
        if (!currentEnemy) yield break;
        var level = LevelManager.Instance.currentLevel as Level;
        var transform1 = shootingPoint.transform;
        var go = Instantiate(projectile, transform1.position, transform1.rotation, level.transform);
        go.InitializeBullet(this, damage, projectileEffectZone, currentEnemy.transform, towerProperties.hitParticle);
        // yield return new WaitForSeconds(1 / firePerSecond);
        // Debug.Log("A");
        yield return null;
        StartCoroutine(_fireRoutine = RepeatFire());
    }

    protected virtual void TowerHasTarget()
    {
    }

    protected virtual void DamageUpgraded()
    {
    }

    public void UpgradeDamage(float value)
    {
        damage = towerProperties.damage += value;
        DamageUpgraded();
    }

    public void UpgradeFireRate(float value)
    {
        firePerSecond = towerProperties.fireRate += value;
    }

    public void UpgradeRadius(float value)
    {
        collider.radius = towerProperties.shootingRange += value;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, towerProperties.shootingRange);
    }
}