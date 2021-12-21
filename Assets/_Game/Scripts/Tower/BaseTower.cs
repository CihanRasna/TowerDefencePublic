using System;
using System.Collections;
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

    [HideInInspector]public Type towerType;
    [SerializeField] protected BaseEnemy currentEnemy;
    
    [SerializeField] private TowerProperties towerProperties;
    [SerializeField] protected Transform shootingPoint;
    [SerializeField] protected new SphereCollider collider;
    
    protected float damage;
    protected float firePerSecond;
    protected Projectile projectile;

    private Coroutine _fireRoutine;
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

    private void OnMouseUpAsButton()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentEnemy) return;

        if (other.TryGetComponent<BaseEnemy>(out var enemy))
        {
            currentEnemy = enemy;
            _fireRoutine = StartCoroutine(RepeatFire());
            TowerHasTarget();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<BaseEnemy>(out var enemy)) return;
        currentEnemy = null;
        StopCoroutine(_fireRoutine);
    }

    private void InitializeTowerProperties()
    {
        towerType = towerProperties.towerType;
        damage = towerProperties.damage;
        collider.radius = towerProperties.shootingRange;
        firePerSecond = towerProperties.fireRate;
        projectile = towerProperties.projectile;
    }

    private IEnumerator RepeatFire()
    {
        if (!currentEnemy) yield break;
        var level = LevelManager.Instance.currentLevel as Level;
        var transform1 = shootingPoint.transform;
        var go = Instantiate(projectile, transform1.position,  transform1.rotation,level.transform);
        go.InitializeBullet(this,damage,currentEnemy.transform,towerProperties.hitParticle);
        yield return new WaitForSeconds(1 / firePerSecond);
        StartCoroutine(RepeatFire());
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
