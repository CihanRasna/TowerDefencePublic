using _Game.Levels.Base;
using UnityEngine;
using Vanta.Levels;

public class TowerBuildingPanel : MonoBehaviour
{
    private BuildingManager _buildingManager;
    [SerializeField] private BuildingPoint selectedPoint;
    
    private void Start()
    {
        _buildingManager = BuildingManager.Instance;
        _buildingManager.towerBuildingPanel.AddListener(GetSelectedSlot);
        _buildingManager.deleselectPanels.AddListener(Deselect);
    }

    private void GetSelectedSlot(BuildingPoint buildingPoint)
    {
        //Deselect();
        buildingPoint.SlotHasSelected(true);
        selectedPoint = buildingPoint;
        selectedPoint.myOutline.OutlineParameters.Color = Color.green;
    }

    private void Deselect()
    {
        if (selectedPoint)
        {
            selectedPoint.SlotHasSelected(false);
            selectedPoint.myOutline.OutlineParameters.Color = Color.white;
            selectedPoint = null;
        }
        gameObject.SetActive(false);
    }

    public void BuildNewTower(int idx)
    {
        var level = LevelManager.Instance.currentLevel as Level;
        var towerToBuild = _buildingManager.towerPrefabs[idx];
        if (level.SpendCurrency(towerToBuild.towerProperties.towerPurchasePrice))
        {
            selectedPoint.TowerBuildingSequence(towerToBuild);
            Deselect();
        }
    }
}
