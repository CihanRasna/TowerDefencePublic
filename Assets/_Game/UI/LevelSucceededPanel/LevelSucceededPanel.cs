using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vanta.UI;



public interface ILevelSucceededPanelDelegate
{
    void LevelSucceededPanel_NextButtonTapped(LevelSucceededPanel levelSucceededPanel);
}

public class LevelSucceededPanel : Panel
{

    [HideInInspector] public ILevelSucceededPanelDelegate listener;
    [SerializeField] private TextMeshProUGUI victoryText;
    [SerializeField] private TextMeshProUGUI restartText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Transform popUpPanel;


    protected override void OnDisplay()
    {
        base.OnDisplay();
        var sequence = DOTween.Sequence();
        var color = backgroundImage.color;
        backgroundImage.color = Color.clear;
        popUpPanel.transform.localScale = Vector3.one * 0.1f;
        sequence.Join(backgroundImage.DOColor(color, 0.5f));
        sequence.Join(popUpPanel.DOScale(Vector3.one, 0.5f));
        sequence.Join(victoryText.DOText("VICTORY", 1f,scrambleMode: ScrambleMode.Uppercase));
        sequence.Join(restartText.DOText("RESTART", 1f, scrambleMode: ScrambleMode.Uppercase));
        sequence.Play();
    }


    #region User Interaction

    public void NextButtonTapped()
    {
        listener.LevelSucceededPanel_NextButtonTapped(this);
    }

#endregion

}