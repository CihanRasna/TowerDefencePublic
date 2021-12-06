using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vanta.Levels;
using Vanta.UI;



public interface IEditorPanelDelegate
{
    void EditorPanel_LoadLevelButtonTapped(EditorPanel panel, int levelIdx);
    void EditorPanel_CloseButtonTapped(EditorPanel panel);
}

public class EditorPanel : Panel
{

    [SerializeField] private Dropdown levelsDropdown;

    public IEditorPanelDelegate listener;



#region Life Cycle

    protected override void Start()
    {
        base.Start();

        List<string> options = new List<string>();
        for (int i = 0; i < LevelManager.Instance.levels.Count; i++)
        {
            options.Add("#" + (i + 1) + " " + LevelManager.Instance.levels[i].name);
        }

        levelsDropdown.ClearOptions();
        levelsDropdown.AddOptions(options);
    }

    public void LoadLevelButtonTapped()
    {
        int levelIdx = levelsDropdown.value;
        listener.EditorPanel_LoadLevelButtonTapped(this, levelIdx);
    }

    public void CloseButtonTapped()
    {
        listener.EditorPanel_CloseButtonTapped(this);
    }

#endregion

}