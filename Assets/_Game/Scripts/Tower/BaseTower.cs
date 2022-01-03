using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EPOOutline;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Vanta.Levels;
using Random = UnityEngine.Random;

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

    public enum ShootingType
    {
        First,
        Random,
        Last
    }

    [ShowInInspector] protected List<BaseEnemy> _potentialNextEnemies = new List<BaseEnemy>();
    [HideInInspector] public Type towerType;
    [HideInInspector] public ShootingType shootingType;
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
    
    public Outlinable myOutline;

    
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
    }

    protected virtual void Update()
    {
        _lastFireTime += Time.deltaTime;
        
        if (_lastFireTime > 1 / firePerSecond && _potentialNextEnemies.Count > 0)
        {
            if (currentEnemy)
            {
                _lastFireTime = 0f;
                RepeatFire();
            }
            else if (!currentEnemy)
            {
                _lastFireTime = 0f;
                ShootingBehaviour();
                RepeatFire();
            }
        }
        
        // if (_potentialNextEnemies.Count > 0 && !_potentialNextEnemies[0])
        // {
        //     _potentialNextEnemies.Remove(currentEnemy);
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BaseEnemy>(out var enemy))
        {
            if (!_potentialNextEnemies.Contains(enemy))
            {
                _potentialNextEnemies.Add(enemy);
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
            _potentialNextEnemies.Remove(enemy);
        }
        currentEnemy = null;
    }

    private void InitializeTowerProperties()
    {
        damageCurrentLevel = 1;
        fireRateCurrentLevel = 1;
        radiusCurrentLevel = 1;
        towerType = towerProperties.towerType;
        shootingType = towerProperties.shootingType;
        damage = towerProperties.damage;
        collider.radius = towerProperties.shootingRange;
        firePerSecond = towerProperties.fireRate;
        projectile = towerProperties.projectile;
        projectileEffectZone = towerProperties.projectileEffectZone;
    }

    private void RepeatFire()
    {
        var level = LevelManager.Instance.currentLevel as Level;
        var transform1 = shootingPoint.transform;
        var go = Instantiate(projectile, transform1.position, transform1.rotation, level.transform);
        go.InitializeBullet(this, damage, projectileEffectZone, currentEnemy.transform, towerProperties.hitParticle);
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


    private void ShootingBehaviour()
    {
        _potentialNextEnemies.RemoveAll(e => e == null);
        currentEnemy = shootingType switch
        {
            ShootingType.First => ShootFirstOne(),
            ShootingType.Random => ShootRandom(),
            ShootingType.Last =>ShootLastOne(),
            _ => null
        };
    }

    private BaseEnemy ShootFirstOne()
    {
        return _potentialNextEnemies[0];
    }

    private BaseEnemy ShootLastOne()
    {
        var currentList = _potentialNextEnemies;
        if (currentList.Count > 0)
        {
            return _potentialNextEnemies.ToList()[_potentialNextEnemies.Count - 1];
        }

        return null;
    }

    private BaseEnemy ShootRandom()
    {
        return _potentialNextEnemies.ToList()[Random.Range(0,_potentialNextEnemies.Count-1)];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, towerProperties.shootingRange);
    }
}