using System;
using _Game.Levels.Base;
using _Game.Scripts.Tower;
using UnityEngine;
using UnityEngine.UI;
using Vanta.Levels;

[Serializable]
public struct TowerValues
{
    public Text towerName;
    public Text damageText;
    public Text damageNextLevelText;
    public Text damageUpgradePrice;
    public Text radiusText;
    public Text radiusNextLevelText;
    public Text radiusUpgradePrice;
    public Text fireRateText;
    public Text fireRateNextLevelText;
    public Text fireRateUpgradePrice;
}

public class TowerUpgradePanel : MonoBehaviour
{
    private BuildingManager _buildingManager;

    [SerializeField] private BaseTower selectedTower;

    public TowerValues towerValues;

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
        var (dmg, fireRate, radius) = selectedTower.RefValuesForUI();

        _damageMaxUpgrade = selectedTower.damageCurrentLevel == selectedProperties.damageMaxUpgradeLevel;
        _radiusMaxUpgrade = selectedTower.radiusCurrentLevel == selectedProperties.radiusMaxUpgradeLevel;
        _fireRateMaxUpgrade = selectedTower.fireRateCurrentLevel == selectedProperties.fireRateMaxUpgradeLevel;

        var damageMultiplier = selectedProperties.damageForUpgrade;
        var fireRateMultiplier = selectedProperties.fireRatePerUpgrade;
        var radiusMultiplier = selectedProperties.radiusPerUpgrade;


        towerValues.towerName.text = $"{tower.towerType} Tower";
        towerValues.damageText.text = $"Current Damage : {dmg}";
        towerValues.fireRateText.text = $"Current Fire Rate : {fireRate}";
        towerValues.radiusText.text = $"Current Radius : {radius}";

        if (_damageMaxUpgrade)
        {
            towerValues.damageNextLevelText.text = $"MAX DAMAGE";
            towerValues.damageUpgradePrice.text = null;
        }
        else
        {
            towerValues.damageNextLevelText.text = $"Next Damage : {dmg + damageMultiplier}";
            towerValues.damageUpgradePrice.text = $"{selectedTower.damageUpgradePrice} $";
        }

        if (_fireRateMaxUpgrade)
        {
            towerValues.fireRateNextLevelText.text = $"MAX SPEED";
            towerValues.fireRateUpgradePrice.text = null;
        }
        else
        {
            towerValues.fireRateNextLevelText.text = $"Next Fire Rate : {fireRate + fireRateMultiplier}";
            towerValues.fireRateUpgradePrice.text = $"{selectedTower.fireRateUpgradePrice} $";
        }

        if (_radiusMaxUpgrade)
        {
            towerValues.radiusNextLevelText.text = $"MAX RADIUS";
            towerValues.radiusUpgradePrice.text = null;

        }
        else
        {
            towerValues.radiusNextLevelText.text = $"Next Radius : {radius + radiusMultiplier}";
            towerValues.radiusUpgradePrice.text = $"{selectedTower.radiusUpgradePrice} $";
        }
    }

    public void UpgradeSelectedTowerDamage()
    {
        var level = (Level)LevelManager.Instance.currentLevel;
        var selectedProperties = selectedTower.towerProperties;
        if (!_damageMaxUpgrade && level.SpendCurrency(selectedTower.damageUpgradePrice))
        {
            selectedTower.damageCurrentLevel += 1;
            selectedTower.UpgradeDamage(selectedProperties.damageForUpgrade);
            selectedTower.TowerHasSelected(false);
            selectedTower.TowerHasSelected(true);
            SetSelectedTowerPropertiesToButtons();
        }
    }

    public void UpgradeSelectedTowerFireRate()
    {
        var level = (Level)LevelManager.Instance.currentLevel;
        var selectedProperties = selectedTower.towerProperties;

        if (!_fireRateMaxUpgrade && level.SpendCurrency(selectedTower.fireRateUpgradePrice))
        {
            selectedTower.fireRateCurrentLevel += 1;
            selectedTower.UpgradeFireRate(selectedProperties.fireRatePerUpgrade);
            selectedTower.TowerHasSelected(false);
            selectedTower.TowerHasSelected(true);
            SetSelectedTowerPropertiesToButtons();
        }
    }

    public void UpgradeSelectedTowerRadius()
    {
        var level = (Level)LevelManager.Instance.currentLevel;
        var selectedProperties = selectedTower.towerProperties;

        if (!_radiusMaxUpgrade && level.SpendCurrency(selectedTower.radiusUpgradePrice))
        {
            selectedTower.radiusCurrentLevel += 1;
            selectedTower.UpgradeRadius(selectedProperties.radiusPerUpgrade);
            selectedTower.TowerHasSelected(false);
            selectedTower.TowerHasSelected(true);
            SetSelectedTowerPropertiesToButtons();
        }
    }

    public void SellSelectedTower()
    {
    }
}