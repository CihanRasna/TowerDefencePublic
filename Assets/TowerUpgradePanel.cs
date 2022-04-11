using System;
using _Game.Scripts.Tower;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct TowerValues
{
    public Text towerName;
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

    public TowerValues _towerValues;

    private bool _damageMaxUpgrade;
    private bool _fireRateMaxUpgrade;
    private bool _radiusMaxUpgrade;

    private void Awake()
    {
        _buildingManager = BuildingManager.Instance;
        _buildingManager.towerUpgradePanel.AddListener(GetSelectedTower);
        _buildingManager.deleselectPanels.AddListener(Deselect);
    }

    private void GetSelectedTower(BaseTower currentTower)
    {
        //Deselect();
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

        gameObject.SetActive(false);
    }

    private void SetSelectedTowerPropertiesToButtons()
    {
        var tower = selectedTower;
        var selectedProperties = tower.towerProperties;
        var towerValues = selectedTower.RefValuesForUI();

        _damageMaxUpgrade = selectedTower.damageCurrentLevel == selectedProperties.damageMaxUpgradeLevel;
        _radiusMaxUpgrade = selectedTower.radiusCurrentLevel == selectedProperties.radiusMaxUpgradeLevel;
        _fireRateMaxUpgrade = selectedTower.fireRateCurrentLevel == selectedProperties.fireRateMaxUpgradeLevel; 

        var dmg = towerValues.damage;
        var fireRate = towerValues.firePerSecond;
        var speed = towerValues.radius;

        var damageMultiplier = selectedProperties.damageForUpgrade;
        var fireRateMultiplier = selectedProperties.fireRatePerUpgrade;
        var speedMultiplier = selectedProperties.radiusPerUpgrade;

        _towerValues.towerName.text = $"{tower.towerType} Tower";
        _towerValues.damageLevelText.text = $"Current Damage : {dmg}";
        _towerValues.fireRateLevelText.text = $"Current Fire Rate : {fireRate}";
        _towerValues.radiusLevelText.text = $"Current Radius : {speed}";

        _towerValues.nextDamageLevelText.text = _damageMaxUpgrade ? $"MAX DAMAGE" : $"Next Damage : {dmg + damageMultiplier}";
        _towerValues.nextFireRateLevelText.text = _fireRateMaxUpgrade ? $"MAX FIRE RATE" : $"Next Fire Rate : {fireRate + fireRateMultiplier}";
        _towerValues.nextRadiusText.text = _radiusMaxUpgrade ? $"MAX SPEED" : $"Next Radius : {speed + speedMultiplier}";
    }

    public void UpgradeSelectedTowerDamage()
    {
        var selectedProperties = selectedTower.towerProperties;
        if (_damageMaxUpgrade) return;
        selectedTower.damageCurrentLevel += 1;
        selectedTower.UpgradeDamage(selectedProperties.damageForUpgrade);
        selectedTower.TowerHasSelected(false);
        selectedTower.TowerHasSelected(true);
        SetSelectedTowerPropertiesToButtons();
    }

    public void UpgradeSelectedTowerFireRate()
    {
        var selectedProperties = selectedTower.towerProperties;

        if (_fireRateMaxUpgrade) return;
        selectedTower.fireRateCurrentLevel += 1;
        selectedTower.UpgradeFireRate(selectedProperties.fireRatePerUpgrade);
        selectedTower.TowerHasSelected(false);
        selectedTower.TowerHasSelected(true);
        SetSelectedTowerPropertiesToButtons();
    }

    public void UpgradeSelectedTowerRadius()
    {
        var selectedProperties = selectedTower.towerProperties;

        if (_radiusMaxUpgrade) return;
        selectedTower.radiusCurrentLevel += 1;
        selectedTower.UpgradeRadius(selectedProperties.radiusPerUpgrade);
        selectedTower.TowerHasSelected(false);
        selectedTower.TowerHasSelected(true);
        SetSelectedTowerPropertiesToButtons();
    }

    public void SellSelectedTower()
    {
    }
}