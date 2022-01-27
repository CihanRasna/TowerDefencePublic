using System;
using System.Collections.Generic;
using _Game.Scripts.Tower;
using UnityEngine;

[Serializable]
public class SelectionReturner : MonoBehaviour
{
    [SerializeField] private Component selectableParent;

    public Tuple<Component, Type> ReturnSelectedFields()
    {
        return selectableParent.GetType().BaseType == typeof(BaseTower)
            ? new Tuple<Component, Type>(selectableParent, selectableParent.GetType().BaseType)
            : new Tuple<Component, Type>(selectableParent, selectableParent.GetType());
    }
}