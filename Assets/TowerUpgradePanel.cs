using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Tower;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TowerUpgradePanel : MonoBehaviour
{
    private BuildingManager _buildingManager;
    
    [SerializeField] private BaseTower selectedTower;
    [SerializeField] private Text towerName;
    [SerializeField] private Text damageLevel;
    [SerializeField] private Text radiusLevel;
    [SerializeField] private Text fireRateLevel;
    

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
        towerName.text = selectedTower.towerType + "Tower";
        damageLevel.text = selectedTower.damageCurrentLevel.ToString();
        fireRateLevel.text = selectedTower.fireRateCurrentLevel.ToString();
        radiusLevel.text = selectedTower.radiusCurrentLevel.ToString();
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
            damageLevel.text = selectedTower.damageCurrentLevel.ToString();
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
            fireRateLevel.text = selectedTower.fireRateCurrentLevel.ToString();
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
            radiusLevel.text = selectedTower.radiusCurrentLevel.ToString();
        }
    }
}
