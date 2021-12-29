using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[SelectionBase]
public abstract class Projectile : MonoBehaviour
{
    [SerializeField,HideInInspector] protected float damage;
    [SerializeField,HideInInspector] protected Transform target;
    [SerializeField,HideInInspector] protected GameObject hitParticle;
    
    [HideInInspector] public BaseTower.Type bulletType;
    private Tweener _tweener = null;

    protected virtual void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BaseEnemy>(out var enemy))
        {
            GetComponent<Collider>().enabled = false;
            DoYourOwnShit(enemy);
            _tweener.Kill();
            enemy.TakeDamage(damage);
            enemy.GetStatusEffect(bulletType);
        }
    }

    protected abstract void DoYourOwnShit(BaseEnemy baseEnemy);

    public void InitializeBullet(BaseTower myTower,float myDamage,Transform myTarget, GameObject myParticle)
    {
        bulletType = myTower.towerType;
        damage = myDamage;
        target = myTarget;
        hitParticle = myParticle;
        _tweener = transform.DOMove(target.position, .2f).OnUpdate(()=>
        {
            if (target) _tweener.ChangeEndValue(target.position + Vector3.up, true);
            else
            {
                _tweener.Kill();
                DestroyImmediate(gameObject);
            }
        });
    }
}
