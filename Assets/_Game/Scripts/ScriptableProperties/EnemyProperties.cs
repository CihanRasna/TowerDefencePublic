using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProperties", menuName = "ScriptableObjects/EnemyPropertiesScriptableObject",
    order = 1)]
public class EnemyProperties : ScriptableObject
{
    [TitleGroup("Enemy Properties", alignment: TitleAlignments.Centered)]
    [BoxGroup("Enemy Properties/Status Effect", true, true)]
    public StatusEffects statusEffects;

    [BoxGroup("Enemy Properties/Health", true, true)]
    public float health;

    [BoxGroup("Enemy Properties/Speed", true, true)]
    public float speed;

    [BoxGroup("Enemy Properties/GoldPrize", true, true)]
    public int goldPrize;
}