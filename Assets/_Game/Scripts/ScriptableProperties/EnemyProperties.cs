using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProperties", menuName = "ScriptableObjects/EnemyPropertiesScriptableObject", order = 1)]
public class EnemyProperties : ScriptableObject
{
    public StatusEffects statusEffects;
    public float health;
    public float speed;
    public int goldPrize;
}
