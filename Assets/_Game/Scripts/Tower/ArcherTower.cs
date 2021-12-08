using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower : BaseTower
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BaseEnemy>(out var enemy))
        {
            var go = Instantiate(projectile, shootingPoint.transform.position, Quaternion.identity);
            go.InitializeBullet(this,damage,bulletSpeed,enemy.transform);
        }
    }
}
