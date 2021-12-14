using System;
using System.Collections;
using UnityEngine;

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

    [HideInInspector]public Type towerType;
    [SerializeField] protected BaseEnemy currentEnemy;
    
    [SerializeField] private TowerProperties towerProperties;
    [SerializeField] protected Transform shootingPoint;
    [SerializeField] protected new SphereCollider collider;
    
    protected float damage;
    protected float fireRate;
    protected Projectile projectile;

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
    
    private void OnTriggerEnter(Collider other)
    {
        if (currentEnemy) return;
        if (other.TryGetComponent<BaseEnemy>(out var enemy))
        {
            currentEnemy = enemy;
            var go = Instantiate(projectile, shootingPoint.transform.position, shootingPoint.transform.rotation);
            go.InitializeBullet(this,damage,currentEnemy.transform);
            TowerHasTarget();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<BaseEnemy>(out var enemy)) return;
        currentEnemy = null;
    }

    private void InitializeTowerProperties()
    {
        towerType = towerProperties.towerType;
        damage = towerProperties.damage;
        collider.radius = towerProperties.shootingRange;
        fireRate = towerProperties.fireRate;
        projectile = towerProperties.projectile;
    }

    internal void RepeatFire()
    {
        var go = Instantiate(projectile, shootingPoint.transform.position, Quaternion.identity);
        go.InitializeBullet(this,damage,currentEnemy.transform);
    }

    protected virtual void TowerHasTarget()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,towerProperties.shootingRange);
    }
}
