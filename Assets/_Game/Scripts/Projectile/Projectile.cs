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

    protected virtual void Start()
    {
        
    }

    public void InitializeBullet(float myDamage,float mySpeed,Transform myTarget)
    {
        damage = myDamage;
        speed = mySpeed;
        target = myTarget;
        transform.DOMove(target.transform.position, .2f);
    }
}
