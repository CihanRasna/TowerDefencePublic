using UnityEngine;
using Vanta.UI;



public interface ILevelSucceededPanelDelegate
{
    void LevelSucceededPanel_NextButtonTapped(LevelSucceededPanel levelSucceededPanel);
}

public class LevelSucceededPanel : Panel
{

    [HideInInspector] public ILevelSucceededPanelDelegate listener;



#region User Interaction

    public void NextButtonTapped()
    {
        listener.LevelSucceededPanel_NextButtonTapped(this);
    }

#endregion

}