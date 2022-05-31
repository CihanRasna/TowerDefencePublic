using System.Collections;
using TMPro;
using UnityEngine;
using Vanta.Levels;
using Vanta.UI;


public interface IPausePanelDelegate
{
    void PausePanel_ContinueButtonTapped(PausePanel pausePanel);
    void PausePanel_SettingsButtonTapped(PausePanel pausePanel);
    void PausePanel_RestartButtonTapped(PausePanel pausePanel);
}
public class PausePanel : Panel
{

    [SerializeField] private TextMeshProUGUI pauseText;
    
    public IPausePanelDelegate listener;

    protected override void OnDisplay()
    {
        base.OnDisplay();
        pauseText.text = "PAUSED";
    }

    public void ContinueButtonTapped()
    {
        listener.PausePanel_ContinueButtonTapped(this);
    }

    public void SettingsButtonTapped()
    {
        listener.PausePanel_SettingsButtonTapped(this);
    }
    
    public void RestartButtonTapped()
    {
        StartCoroutine(RestartLevel());
    }

    public void GiveUpButtonTapped()
    {
        StartCoroutine(QuitApp());
    }

    private IEnumerator RestartLevel()
    {
        var remainingTime = 3.5f;
        while (true)
        {
            remainingTime -= Time.unscaledDeltaTime;
            if (remainingTime <= 0f)
            {
                break;
            }

            pauseText.text = $"Restart in {(int) (remainingTime)}";

            yield return null;
        }
        yield return null;
        listener.PausePanel_RestartButtonTapped(this);
        Hide();
    }

    private IEnumerator QuitApp()
    {
        var remainingTime = 3.5f;
        while (true)
        {
            remainingTime -= Time.unscaledDeltaTime;
            if (remainingTime <= 0f)
            {
                break;
            }

            pauseText.text = $"Closing {(int) (remainingTime)}";

            yield return null;
        }
        yield return null;
        Application.Quit();
    }
}
