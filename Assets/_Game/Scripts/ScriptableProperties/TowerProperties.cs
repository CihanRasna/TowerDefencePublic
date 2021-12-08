using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerProperties", menuName = "ScriptableObjects/TowerPropertiesScriptableObject",
    order = 2)]
public class TowerProperties : ScriptableObject
{
    [Title("Tower Type", titleAlignment: TitleAlignments.Centered), EnumToggleButtons, HideLabel]
    public BaseTower.Type towerType;
    [Space(20)]
    
    [Title("Projectile",titleAlignment:TitleAlignments.Centered)]
    [HideLabel,PreviewField(100, ObjectFieldAlignment.Center)]
    public Projectile projectile;
    public float bulletSpeed;
    public float damage;
    public float fireRate;
    public float shootingRange;
}