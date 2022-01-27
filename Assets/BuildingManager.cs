using System;
using System.Collections.Generic;
using _Game.Levels.Base;
using _Game.Scripts.Tower;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vanta.Core;
using Vanta.Input;
using Vanta.Levels;

public class BuildingManager : Singleton<BuildingManager>, PanRecognizer.IPanRecognizerDelegate
{
    [SerializeField] private List<BaseTower> towerPrefabs;
    [SerializeField] private float panelScaleRatio = 1200f;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject purchasePanel;
    [SerializeField] private BaseTower selectedTower;
    [SerializeField] private BuildingPoint selectedPoint;
    [SerializeField] private Text damageLevel;
    [SerializeField] private Text radiusLevel;
    [SerializeField] private Text fireRateLevel;

    private int selectionLayer;
    private Camera _camera;

    void Start()
    {
        selectionLayer = 1 << LayerMask.NameToLayer("SelectionLayer");
        _camera = Camera.main;
    }

    #region TowerSelection

    private void GetTopOnSelectedTower(BaseTower currentTower)
    {
        HidePanel();
        upgradePanel.SetActive(true);
        currentTower.TowerHasSelected(true);
        selectedTower = currentTower;
        selectedTower.myOutline.OutlineParameters.Color = Color.green;
        var towerPos = _camera.WorldToScreenPoint(currentTower.transform.position);
        transform.position = towerPos;
        SetSelectedTowerPropertiesToButtons();
    }

    private void SetSelectedTowerPropertiesToButtons()
    {
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

    #endregion

    #region SlotSelection

    private void GetTopOnSelectedSlot(BuildingPoint buildingPoint)
    {
        HidePanel();
        purchasePanel.SetActive(true);
        buildingPoint.SlotHasSelected(true);
        selectedPoint = buildingPoint;
        selectedPoint.myOutline.OutlineParameters.Color = Color.green;
        var towerPos = _camera.WorldToScreenPoint(buildingPoint.transform.position);
        transform.position = towerPos;
    }

    public void BuildNewTower(int idx)
    {
        var towerToBuild = towerPrefabs[idx];
        selectedPoint.TowerBuildingSequence(towerToBuild);
        HidePanel();
    }

    #endregion

    private void HidePanel()
    {
        purchasePanel.SetActive(false);
        upgradePanel.SetActive(false);
        if (selectedTower)
        {
            selectedTower.TowerHasSelected(false);
            selectedTower.myOutline.OutlineParameters.Color = Color.white;
            selectedTower = null;
            transform.position = new Vector3(1235, 1234, 4563);
        }

        if (selectedPoint)
        {
            selectedPoint.SlotHasSelected(false);
            selectedPoint.myOutline.OutlineParameters.Color = Color.white;
            selectedPoint = null;
            transform.position = new Vector3(1235, 1234, 4563);
        }
    }

    #region PanRecognizer

    public bool PanRecognizerShouldStartListening(PanRecognizer recognizer)
    {
        var level = LevelManager.Instance.currentLevel as Level;
        if (level.state == BaseLevel.State.Loaded)
        {
            level.StartLevel();
        }
        return level.state == BaseLevel.State.Started;
    }

    public void PanRecognizerDidStartListening(PanRecognizer recognizer)
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 1000, selectionLayer))
        {
            if (hit.collider.TryGetComponent(out SelectionReturner returner))
            {
                var (handledComponent, handledType) = returner.ReturnSelectedFields();

                if (handledType == typeof(BaseTower))
                    GetTopOnSelectedTower(handledComponent as BaseTower);

                if (handledType == typeof(BuildingPoint))
                    GetTopOnSelectedSlot(handledComponent as BuildingPoint);
            }
        }
        else
        {
            HidePanel();
        }
    }

    public void PanRecognizerDidChangeValue(PanRecognizer recognizer, float value)
    {
    }

    public void PanRecognizerDidEndListening(PanRecognizer recognizer, bool forced)
    {
    }

    #endregion
}