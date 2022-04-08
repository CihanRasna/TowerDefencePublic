using System;
using _Game.Scripts.Tower;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct TowerValues
{
    public Text damageLevelText;
    public Text nextDamageLevelText;
    public Text radiusLevelText;
    public Text nextRadiusText;
    public Text fireRateLevelText;
    public Text nextFireRateLevelText;
}

public class TowerUpgradePanel : MonoBehaviour
{
    private BuildingManager _buildingManager;

    [SerializeField] private BaseTower selectedTower;
    [SerializeField] private Text towerName;

    public TowerValues _towerValues;

    private void Start()
    {
        _buildingManager = BuildingManager.Instance;
        _buildingManager.towerUpgradePanel.AddListener(GetSelectedTower);
        _buildingManager.deleselectPanels.AddListener(Deselect);
    }

    private void GetSelectedTower(BaseTower currentTower)
    {
        Deselect();
        currentTower.TowerHasSelected(true);
        selectedTower = currentTower;
        selectedTower.myOutline.OutlineParameters.Color = Color.green;
        SetSelectedTowerPropertiesToButtons();
    }

    private void Deselect()
    {
        if (selectedTower)
        {
            selectedTower.TowerHasSelected(false);
            selectedTower.myOutline.OutlineParameters.Color = Color.white;
            selectedTower = null;
        }
    }

    private void SetSelectedTowerPropertiesToButtons()
    {
        var selectedTower = this.selectedTower;
        var selectedProperties = selectedTower.towerProperties;
        var dmg = selectedTower.damageCurrentLevel * selectedProperties.damageForUpgrade;
        var fireRate = selectedTower.fireRateCurrentLevel * selectedProperties.fireRatePerUpgrade;
        var speed = selectedTower.radiusCurrentLevel * selectedProperties.radiusPerUpgrade;
        towerName.text = selectedTower.towerType + "Tower";
        _towerValues.damageLevelText.text =dmg.ToString();
        _towerValues.fireRateLevelText.text = fireRate.ToString();
        _towerValues.radiusLevelText.text = speed.ToString();
    }

    public void UpgradeSelectedTowerDamage()
    {
        var selectedProperties = selectedTower.towerProperties;
        if (selectedTower.damageCurrentLevel < selectedProperties.damageMaxUpgradeLevel)
        {
            selectedTower.damageCurrentLevel += 1;
            selectedTower.UpgradeDamage(selectedProperties.damageForUpgrade);
            selectedTower.TowerHasSelected(false);
            selectedTower.TowerHasSelected(true);
            _towerValues.damageLevelText.text = selectedTower.damageCurrentLevel.ToString();
        }
    }

    public void UpgradeSelectedTowerFireRate()
    {
        var selectedProperties = selectedTower.towerProperties;
        if (selectedTower.fireRateCurrentLevel < selectedProperties.fireRateMaxUpgradeLevel)
        {
            selectedTower.fireRateCurrentLevel += 1;
            selectedTower.UpgradeFireRate(selectedProperties.fireRatePerUpgrade);
            selectedTower.TowerHasSelected(false);
            selectedTower.TowerHasSelected(true);
            _towerValues.fireRateLevelText.text = selectedTower.fireRateCurrentLevel.ToString();
        }
    }

    public void UpgradeSelectedTowerRadius()
    {
        var selectedProperties = selectedTower.towerProperties;
        if (selectedTower.radiusCurrentLevel < selectedProperties.radiusMaxUpgradeLevel)
        {
            selectedTower.radiusCurrentLevel += 1;
            selectedTower.UpgradeRadius(selectedProperties.radiusPerUpgrade);
            selectedTower.TowerHasSelected(false);
            selectedTower.TowerHasSelected(true);
            _towerValues.radiusLevelText.text = selectedTower.radiusCurrentLevel.ToString();
        }
    }

    public void SellSelectedTower()
    {
    }
}