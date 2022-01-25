using DG.Tweening;
using EPOOutline;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPoint : MonoBehaviour
{
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
}
