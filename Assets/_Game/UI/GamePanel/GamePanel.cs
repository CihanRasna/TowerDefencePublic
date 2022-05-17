using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vanta.UI;


public interface IGamePanelDelegate
{
    void GamePanel_PauseButtonTapped(GamePanel gamePanel);
    void GamePanel_EditorButtonTapped(GamePanel gamePanel);
}

public class GamePanel : Panel
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private Text levelIndex;
    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI currencyUI;
    [SerializeField] private TextMeshProUGUI healthUI;

    [HideInInspector] public int maxHealthStatus = 0;

    [HideInInspector] public IGamePanelDelegate listener;


    #region Life Cycle

    protected override void OnDisplay()
    {
        base.OnDisplay();
        DisplaySettingsButton(true);
    }

    protected override void Start()
    {
        base.Start();
        //UpdateProgressBar(0, false);
    }

    #endregion


    #region Level Index

    public void UpdateLevelIndex(int index)
    {
        levelIndex.text = "LEVEL " + (index + 1).ToString();
    }

    #endregion

    #region Health

    public void UpdateHealth(int h)
    {
        healthUI.text = $"{h} / {maxHealthStatus}";
    }

    #endregion

    #region Currency

    public void UpdateCurrency(int currency)
    {
        var dummyText = currencyUI;
        dummyText.DOText(currency.ToString(), 0.2f, true, ScrambleMode.Numerals).OnUpdate((() =>
        {
            currencyUI.text = dummyText.text + "$";
        }));
        
    }

    #endregion


    #region Progress Bar

    public void UpdateProgressBar(float percent, bool animated)
    {
        if (!animated)
        {
            ChangeProgressBarWidth(0);
        }

        iTween.Stop(gameObject);
        iTween.ValueTo(gameObject, iTween.Hash(
                "from", progressBar.fillAmount,
                "to", percent,
                "time", 0.2f,
                "onupdatetarget", gameObject,
                "onupdate", "ChangeProgressBarWidth",
                "easetype", iTween.EaseType.linear
            )
        );
    }

    private void ChangeProgressBarWidth(float value)
    {
        var minWidth = 10.0f;
        var maxWidth = 558.0f;
        var width = Mathf.Lerp(minWidth, maxWidth, value);
        progressBar.rectTransform.offsetMax = new Vector2(-width, progressBar.rectTransform.offsetMax.y);
    }

    #endregion


    #region Settings

    public void DisplaySettingsButton(bool display)
    {
        pauseButton.gameObject.SetActive(display);
    }

    public void PauseButtonTapped()
    {
        listener.GamePanel_PauseButtonTapped(this);
    }

    #endregion


    #region User Interaction

    public void EditorButtonTapped()
    {
        listener.GamePanel_EditorButtonTapped(this);
    }

    #endregion
}