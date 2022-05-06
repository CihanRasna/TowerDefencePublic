using UnityEngine;
using UnityEngine.UI;

public class TowerPropertiesVisualizer : MonoBehaviour
{

    [SerializeField] private BuildingManager buildingManager;
    
    [SerializeField] private Text towerName;
    [SerializeField] private Text towerDamage;
    [SerializeField] private Text towerRange;
    [SerializeField] private Text towerFireRate;
    [SerializeField] private Text buyPrice;
    [SerializeField] private int showIdx = -1;

    private void OnValidate()
    {
        if (showIdx == -1) return;
        var tower = buildingManager.towerPrefabs[showIdx];
        towerName.text = $"{tower.towerProperties.towerType} Tower";
        towerDamage.text = $"{tower.towerProperties.damage} DMG";
        towerRange.text = $"{tower.towerProperties.shootingRadius} Meter";
        towerFireRate.text = $"{tower.towerProperties.fireRate} Hit/Sec";
        buyPrice.text = $"{tower.towerProperties.towerPurchasePrice} $";
    }
}
