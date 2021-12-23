using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Vanta.Core;
using Vanta.Input;

public class BuildingManager : Singleton<BuildingManager>, PanRecognizer.IPanRecognizerDelegate
{
    [SerializeField] private float panelScaleRatio = 1200f;
    [SerializeField] private BaseTower selectedTower;

    private Camera _camera;

    [SerializeField] private Text damageLevel;
    [SerializeField] private Text radiusLevel;
    [SerializeField] private Text fireRateLevel;


    void Start()
    {
        _camera = Camera.main;
    }

    private void GetTopOnSelectedTower(BaseTower currentTower)
    {
        selectedTower = currentTower;
        var towerPos = _camera.WorldToScreenPoint(currentTower.transform.position);
        transform.position = towerPos;

        var distance = Vector3.Distance(transform.position, _camera.transform.position);
        transform.localScale = Vector3.one * distance / panelScaleRatio;
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
            Debug.Log("A");
            selectedTower.damageCurrentLevel += 1;
            selectedTower.UpgradeDamage(selectedProperties.damageForUpdate);
            damageLevel.text = selectedTower.damageCurrentLevel.ToString();
        }
    }

    public void UpgradeSelectedTowerFireRate()
    {
        var selectedProperties = selectedTower.towerProperties;
        if (selectedTower.fireRateCurrentLevel < selectedProperties.fireRateMaxUpgradeLevel)
        {
            selectedTower.fireRateCurrentLevel += 1;
            selectedTower.UpgradeFireRate(selectedProperties.fireRatePerUpdate);
            fireRateLevel.text = selectedTower.fireRateCurrentLevel.ToString();
        }
    }

    public void UpgradeSelectedTowerRadius()
    {
        var selectedProperties = selectedTower.towerProperties;
        if (selectedTower.radiusCurrentLevel < selectedProperties.radiusMaxUpgradeLevel)
        {
            selectedTower.radiusCurrentLevel += 1;
            selectedTower.UpgradeRadius(selectedProperties.radiusPerUpdate);
            radiusLevel.text = selectedTower.radiusCurrentLevel.ToString();
        }
    }

    private void HidePanel()
    {
        selectedTower = null;
        transform.position = new Vector3(1235, 1234, 4563);
    }

    public bool PanRecognizerShouldStartListening(PanRecognizer recognizer)
    {
        return true;
    }

    public void PanRecognizerDidStartListening(PanRecognizer recognizer)
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            if (hit.collider.TryGetComponent(out BaseTower baseTower))
            {
                GetTopOnSelectedTower(baseTower);
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