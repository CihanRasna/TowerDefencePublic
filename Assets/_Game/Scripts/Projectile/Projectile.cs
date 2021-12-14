using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[SelectionBase]
public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected Transform target;
    [HideInInspector] public BaseTower.Type bulletType;
    private Tweener _tweener = null;

    protected virtual void Start()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BaseEnemy>(out var enemy))
        {
            _tweener.Kill();
            transform.parent = enemy.transform;
            enemy.TakeDamage(damage);
            enemy.GetStatusEffect(bulletType);
        }
    }

    public void InitializeBullet(BaseTower myTower,float myDamage,Transform myTarget)
    {
        bulletType = myTower.towerType;
        damage = myDamage;
        target = myTarget;
        _tweener = transform.DOMove(target.transform.position, .2f);
    }
}
