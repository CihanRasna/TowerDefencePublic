using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected float speed;
    [SerializeField] protected Transform target;
    [HideInInspector]public BaseTower.Type bulletType;

    protected virtual void Start()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BaseEnemy>(out var enemy))
        {
            enemy.TakeDamage(damage);
            enemy.GetStatusEffect(bulletType);
        }
    }

    public void InitializeBullet(BaseTower myTower,float myDamage,float mySpeed,Transform myTarget)
    {
        bulletType = myTower.towerType;
        damage = myDamage;
        speed = mySpeed;
        target = myTarget;
        transform.DOMove(target.transform.position, .2f);
    }
}
