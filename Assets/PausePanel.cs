using UnityEngine;
using Vanta.UI;


public interface IPausePanelDelegate
{
    void PausePanel_ContinueButtonTapped(PausePanel pausePanel);
    void PausePanel_SettingsButtonTapped(PausePanel pausePanel);
    void PausePanel_RestartButtonTapped(PausePanel pausePanel);
    void PausePanel_GiveUpButtonTapped(PausePanel pausePanel);
}
public class PausePanel : Panel
{
    public IPausePanelDelegate listener;

    public void ContinueButtonTapped()
    {
        listener.PausePanel_ContinueButtonTapped(this);
    }

    public void SettingsButtonTapped()
    {
        listener.PausePanel_SettingsButtonTapped(this);
    }
}
