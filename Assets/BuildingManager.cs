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
    public List<BaseTower> TowerPrefabs;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject purchasePanel;

    private int _selectionLayer;
    private Camera _camera;
    
    //Made Internal for Editor shitting
    internal UnityEvent<BaseTower> towerUpgradePanel = new UnityEvent<BaseTower>();
    internal UnityEvent<BuildingPoint> towerBuildingPanel = new UnityEvent<BuildingPoint>();
    internal UnityEvent deleselectPanels = new UnityEvent();

    void Start()
    {
        _selectionLayer = 1 << LayerMask.NameToLayer("SelectionLayer");
        _camera = Camera.main;
    }
    private void HidePanel()
    {
        deleselectPanels.Invoke();
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
        if (Physics.Raycast(ray, out var hit, 1000, _selectionLayer))
        {
            if (hit.collider.TryGetComponent(out SelectionReturner returner))
            {
                var (handledComponent, handledType) = returner.ReturnSelectedFields();

                if (handledType == typeof(BaseTower))
                    towerUpgradePanel.Invoke(handledComponent as BaseTower);

                if (handledType == typeof(BuildingPoint))
                    towerBuildingPanel.Invoke(handledComponent as BuildingPoint);
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