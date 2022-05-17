using UnityEngine;
using UnityEngine.UI;
using Vanta.UI;


public interface ISettingsPanelDelegate
{
    void SettingsPanel_VolumeChanged(SettingsPanel settingsPanel, float value);
    void SettingsPanel_EffectVolumeChanged(SettingsPanel settingsPanel, float value);
    void SettingsPanel_CloseButtonTapped(SettingsPanel settingsPanel);
}

public class SettingsPanel : Panel
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider soundEffectSlider;

    [HideInInspector] public ISettingsPanelDelegate listener;

    #region Life Cycle

    protected override void Start()
    {
        base.Start();
        volumeSlider.onValueChanged.AddListener(UpdateAudioButtonValue);
        soundEffectSlider.onValueChanged.AddListener(UpdateSoundEffectButtonValue);
    }

    protected override void OnDisplay()
    {
        base.OnDisplay();
    }

    #endregion


    #region Navigation

    public void CloseButtonTapped()
    {
        listener.SettingsPanel_CloseButtonTapped(this);
    }

    #endregion


    #region Sounds

    private void UpdateAudioButtonValue(float value)
    {
        listener.SettingsPanel_VolumeChanged(this,value);
    }
    private void UpdateSoundEffectButtonValue(float value)
    {
        listener.SettingsPanel_VolumeChanged(this,value);
    }

    #endregion
}