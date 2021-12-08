using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;
using Vanta.Levels;

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
    
    [SerializeField] private TowerProperties towerProperties;
    [SerializeField] protected Transform shootingPoint;
    [SerializeField] protected SphereCollider collider;
    
    protected float damage;
    protected float bulletSpeed;
    protected float fireRate;
    protected Projectile projectile;

    private void OnEnable()
    {
        collider.radius = towerProperties.shootingRange;
    }

    protected virtual void Awake()
    {
        InitializeTowerProperties();
    }

    protected virtual void Start()
    {
        
    }

    private void InitializeTowerProperties()
    {
        towerType = towerProperties.towerType;
        damage = towerProperties.damage;
        bulletSpeed = towerProperties.bulletSpeed;
        collider.radius = towerProperties.shootingRange;
        fireRate = towerProperties.fireRate;
        projectile = towerProperties.projectile;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,towerProperties.shootingRange);
    }
}
