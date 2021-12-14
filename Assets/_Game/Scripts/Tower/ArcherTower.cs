using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ArcherTower : BaseTower
{
    [SerializeField] private AxisConstraint axisConstraint;
    [SerializeField] private Transform myArcher;
    
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

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
