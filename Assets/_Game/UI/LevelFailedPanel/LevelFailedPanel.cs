using UnityEngine;
using Vanta.UI;



public interface ILevelFailedPanelDelegate
{
    void LevelFailedPanel_TryAgainButtonTapped(LevelFailedPanel levelFailedPanel);
}

public class LevelFailedPanel : Panel
{
    [HideInInspector] public ILevelFailedPanelDelegate listener;



#region User Interaction

    public void TryAgainButtonTapped()
    {
        listener.LevelFailedPanel_TryAgainButtonTapped(this);
    }

#endregion

}