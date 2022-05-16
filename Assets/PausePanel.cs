using Vanta.UI;


public interface IPausePanelDelegate
{
    void PausePanel_ContinueButtonTapped(PausePanel gamePanel);
    void PausePanel_SettingsButtonTapped(PausePanel gamePanel);
    void PausePanel_RestartButtonTapped(PausePanel gamePanel);
    void PausePanel_GiveUpButtonTapped(PausePanel gamePanel);
}
public class PausePanel : Panel
{
    
}
