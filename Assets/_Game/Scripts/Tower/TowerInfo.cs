using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Tower;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfo : MonoBehaviour
{
    [SerializeField] private BaseTower towerPrefab;
    [SerializeField] private Text towerName;
    [SerializeField] private Text towerDamage;
    [SerializeField] private Text towerRadius;
    [SerializeField] private Text towerFireRate;
    [SerializeField] private Text towerPrice;
    [SerializeField] private Image towerImage;
    
}
