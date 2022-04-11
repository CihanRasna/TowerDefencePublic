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
    public Text radiusNextlevelText;
    public Text radiusUpgradePrice;
    public Text fireRateText;
    public Text fireRateNextLevelText;
    public Text fireRateUpgradePrice;
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
        var (dmg, fireRate, radius) = selectedTower.RefValuesForUI();

        _damageMaxUpgrade = selectedTower.damageCurrentLevel == selectedProperties.damageMaxUpgradeLevel;
        _radiusMaxUpgrade = selectedTower.radiusCurrentLevel == selectedProperties.radiusMaxUpgradeLevel;
        _fireRateMaxUpgrade = selectedTower.fireRateCurrentLevel == selectedProperties.fireRateMaxUpgradeLevel;

        var damageMultiplier = selectedProperties.damageForUpgrade;
        var fireRateMultiplier = selectedProperties.fireRatePerUpgrade;
        var radiusMultiplier = selectedProperties.radiusPerUpgrade;


        _towerValues.towerName.text = $"{tower.towerType} Tower";
        _towerValues.damageText.text = $"Current Damage : {dmg}";
        _towerValues.fireRateText.text = $"Current Fire Rate : {fireRate}";
        _towerValues.radiusText.text = $"Current Radius : {radius}";

        if (_damageMaxUpgrade)
        {
            _towerValues.damageNextLevelText.text = $"MAX DAMAGE";
            _towerValues.damageUpgradePrice.text = null;
        }
        else
        {
            _towerValues.damageNextLevelText.text = $"Next Damage : {dmg + damageMultiplier}";
            _towerValues.damageUpgradePrice.text = $"{selectedTower.damageUpgradePrice} $";
        }
           
        _towerValues.fireRateNextLevelText.text = _fireRateMaxUpgrade ? $"MAX FIRE RATE" : $"Next Fire Rate : {fireRate + fireRateMultiplier}";
        _towerValues.radiusNextlevelText.text = _radiusMaxUpgrade ? $"MAX SPEED" : $"Next Radius : {radius + radiusMultiplier}";
    }

    public void UpgradeSelectedTowerDamage()
    {
        var level = LevelManager.Instance.currentLevel as Level;
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
        var level = LevelManager.Instance.currentLevel as Level;
        var selectedProperties = selectedTower.towerProperties;

        if (!_fireRateMaxUpgrade && level.SpendCurrency(100))
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
        var level = LevelManager.Instance.currentLevel as Level;
        var selectedProperties = selectedTower.towerProperties;

        if (!_radiusMaxUpgrade && level.SpendCurrency(100))
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