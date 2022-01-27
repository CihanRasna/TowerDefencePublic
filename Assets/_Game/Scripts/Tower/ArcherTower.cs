using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Tower;
using DG.Tweening;
using UnityEngine;

public class ArcherTower : BaseTower
{
    [SerializeField] private AxisConstraint axisConstraint;
    [SerializeField] private Transform myArcher;

    protected override void DamageUpgraded()
    {
    }

    protected override void Update()
    {
        base.Update();
        if (currentEnemy != null)
        {
            SoldierLookAt(currentEnemy.transform);
        }
    }

    private void SoldierLookAt(Component target)
    {
        myArcher.DOLookAt(target.transform.position, Time.deltaTime, axisConstraint);
    }
}