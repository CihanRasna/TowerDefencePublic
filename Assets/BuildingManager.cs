using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Vanta.Core;
using Vanta.Input;

public class BuildingManager : Singleton<BuildingManager>, PanRecognizer.IPanRecognizerDelegate
{
    [SerializeField] private float panelScaleRatio = 1200f;

    [SerializeField] private BaseTower selectedTower;
    // private BaseTower _oldSelectedTower;

    private Camera _camera;

    [SerializeField] private Text damageLevel;
    [SerializeField] private Text radiusLevel;
    [SerializeField] private Text fireRateLevel;

    private int selectionLayer;
    void Start()
    {
        selectionLayer = 1 << LayerMask.NameToLayer("SelectionLayer");
        _camera = Camera.main;
    }

    private void GetTopOnSelectedTower(BaseTower currentTower)
    {
        HidePanel();
        currentTower.TowerHasSelected(true);
        selectedTower = currentTower;
        selectedTower.myOutline.OutlineParameters.Color = Color.green;
        var towerPos = _camera.WorldToScreenPoint(currentTower.transform.position);
        transform.position = towerPos;
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

    private void HidePanel()
    {
        if (selectedTower)
        {
            selectedTower.TowerHasSelected(false);
            selectedTower.myOutline.OutlineParameters.Color = Color.white;
            selectedTower = null;
            transform.position = new Vector3(1235, 1234, 4563);
        }
    }

    public bool PanRecognizerShouldStartListening(PanRecognizer recognizer)
    {
        return true;
    }

    public void PanRecognizerDidStartListening(PanRecognizer recognizer)
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit,1000,selectionLayer))
        {
            if (hit.collider.TryGetComponent(out SelectionReturner returner))
            {
                GetTopOnSelectedTower((BaseTower)returner.ReturnSelectedParent());
                SetSelectedTowerPropertiesToButtons();
            }
            else
            {
                HidePanel();
            }
        }
    }

    public void PanRecognizerDidChangeValue(PanRecognizer recognizer, float value)
    {
    }

    public void PanRecognizerDidEndListening(PanRecognizer recognizer, bool forced)
    {
    }
}