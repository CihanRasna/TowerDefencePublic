using System.Collections;
using _Game.Levels.Base;
using _Game.Scripts.Tower;
using DG.Tweening;
using EPOOutline;
using UnityEngine;
using UnityEngine.UI;
using Vanta.Levels;

public class BuildingPoint : MonoBehaviour
{
    [SerializeField] private MeshRenderer myRenderer;
    [SerializeField] private GameObject buildingVFX;
    [SerializeField] private Canvas myCanvas;
    [SerializeField] internal Outlinable myOutline;
    [SerializeField] private Image radiusIndicatorImage;
    private Tweener imageTweener;
    
    public void SlotHasSelected(bool currentSelected)
    {
        if (currentSelected)
        {
            myCanvas.gameObject.SetActive(true);
            radiusIndicatorImage.fillAmount = 0f;
            imageTweener = radiusIndicatorImage.DOFillAmount(1, .5f).SetEase(Ease.InOutCirc);
        }
        else
        {
            myCanvas.gameObject.SetActive(false);
            imageTweener.Kill();
            imageTweener = null;
        }
    }


    public void TowerBuildingSequence(BaseTower tower)
    {
        StartCoroutine(BuildNewTower(towerToBuild: tower));
    }
    
    private IEnumerator BuildNewTower(BaseTower  towerToBuild)
    {
        myRenderer.enabled = false;
        buildingVFX.SetActive(true);
        var elapsedTime = 0.0f;
        var duration = 2f;
        
        yield return null; // HidePanel issue
        radiusIndicatorImage.fillAmount = 0f;
        myCanvas.gameObject.SetActive(true);
        
        while (true)
        {
            var progress = Mathf.Clamp01(elapsedTime / duration);
            radiusIndicatorImage.fillAmount = progress;
            if (progress >= 1)
            {
                Instantiate(towerToBuild, transform.position, Quaternion.identity,LevelManager.Instance.currentLevel.transform);
                Destroy(gameObject);
                break;
            }
            yield return null;
            elapsedTime += Time.deltaTime;
        }
    }
}
