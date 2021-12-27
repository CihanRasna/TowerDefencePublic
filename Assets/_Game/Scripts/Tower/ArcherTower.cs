using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ArcherTower : BaseTower
{
    [SerializeField] private AxisConstraint axisConstraint;
    [SerializeField] private Transform myArcher;

    private void Update()
    {
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
