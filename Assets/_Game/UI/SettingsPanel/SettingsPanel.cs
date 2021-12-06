using UnityEngine;
using UnityEngine.UI;
using Vanta.Persist;
using Vanta.UI;



public interface ISettingsPanelDelegate
{
    void SettingsPanel_CloseButtonTapped(SettingsPanel settingsPanel);
}

public class SettingsPanel : Panel
{

    [SerializeField] private Image hapticButton;
    [SerializeField] private Image soundButton;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    [HideInInspector] public ISettingsPanelDelegate listener;



#region Life Cycle

    protected override void OnDisplay()
    {
        base.OnDisplay();
        
        UpdateHapticButtonState();
        UpdateSoundButtonState();
    }

#endregion



#region Navigation

    public void CloseButtonTapped()
    {
        listener.SettingsPanel_CloseButtonTapped(this);
    }

#endregion



#region Haptic

    private void UpdateHapticButtonState()
    {
        var sprite = PersistManager.Instance.hapticsEnabled ? onSprite : offSprite;
        hapticButton.sprite = sprite;
    }

    public void HapticButtonTapped()
    {
        PersistManager.Instance.hapticsEnabled = !PersistManager.Instance.hapticsEnabled;
        UpdateHapticButtonState();
    }

#endregion



#region Sounds

    private void UpdateSoundButtonState()
    {
        var sprite = PersistManager.Instance.soundsEnabled ? onSprite : offSprite;
        soundButton.sprite = sprite;
    }

    public void SoundButtonTapped()
    {
        PersistManager.Instance.soundsEnabled = !PersistManager.Instance.soundsEnabled;
        UpdateSoundButtonState();
    }

#endregion

}