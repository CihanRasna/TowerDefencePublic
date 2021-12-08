using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerProperties", menuName = "ScriptableObjects/TowerPropertiesScriptableObject", order = 2)]
public class TowerProperties : ScriptableObject
{
    public BaseTower.Type towerType;
    public Projectile projectile;
    public float bulletSpeed;
    public float damage;
    public float fireRate;
    public float shootingRange;
}
