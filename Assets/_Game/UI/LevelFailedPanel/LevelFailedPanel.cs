using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vanta.Core;
using Vanta.UI;



public interface ILevelFailedPanelDelegate
{
    void LevelFailedPanel_TryAgainButtonTapped(LevelFailedPanel levelFailedPanel);
}

public class LevelFailedPanel : Panel
{
    [HideInInspector] public ILevelFailedPanelDelegate listener;
    [SerializeField] private TextMeshProUGUI defeatText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Transform popUpPanel;

    protected override void OnDisplay()
    {
        base.OnDisplay();
        //GameManager.Instance.PauseGame();
        var sequence = DOTween.Sequence();
        var color = backgroundImage.color;
        backgroundImage.color = Color.clear;
        popUpPanel.transform.localScale = Vector3.one * 0.1f;
        sequence.Join(backgroundImage.DOColor(color, 0.5f));
        sequence.Join(popUpPanel.DOScale(Vector3.one, 0.5f));
        sequence.Join(defeatText.DOText("DEFEAT", 0.5f,scrambleMode: ScrambleMode.Uppercase));
        sequence.Play();
    }


    #region User Interaction

    public void TryAgainButtonTapped()
    {
        listener.LevelFailedPanel_TryAgainButtonTapped(this);
    }

#endregion

}