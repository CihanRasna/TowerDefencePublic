using _Game.Levels.Base;
using UnityEngine;
using Vanta.Core;
using Vanta.Levels;
using Vanta.Persist;

namespace Vanta.UI
{
    public class MenuManager : Singleton<MenuManager>,
        IGamePanelDelegate,
        ISettingsPanelDelegate,
        ILevelFailedPanelDelegate,
        ILevelSucceededPanelDelegate,
        IEditorPanelDelegate,
        IPausePanelDelegate
    {
        [SerializeField] private GamePanel gamePanel;
        [SerializeField] private SettingsPanel settingsPanel;
        [SerializeField] private LevelFailedPanel levelFailedPanel;
        [SerializeField] private LevelSucceededPanel levelSucceededPanel;
        [SerializeField] private TutorialPanel tutorialPanel;
        [SerializeField] private EditorPanel editorPanel;
        [SerializeField] private PausePanel pausePanel;
        

        [SerializeField] [HideInInspector]
        private SafeArea.SimulationDevice simulatingDevice = SafeArea.SimulationDevice.None;


        #region Life Cycle

        protected override void Awake()
        {
            base.Awake();

#if UNITY_EDITOR
            SafeArea.simulationDevice = simulatingDevice;
#endif
        }

        private void Start()
        {
            gamePanel.listener = this;
            settingsPanel.listener = this;
            levelFailedPanel.listener = this;
            levelSucceededPanel.listener = this;
            editorPanel.listener = this;
            pausePanel.listener = this;

            LevelManager.Instance.levelDidLoad += LevelDidLoad;
            LevelManager.Instance.levelDidStart += LevelDidStart;
            LevelManager.Instance.levelDidSuccees += LevelDidSuccess;
            LevelManager.Instance.levelDidFail += LevelDidFail;
        }

        #endregion


        #region Level Events

        private void LevelDidLoad(BaseLevel baseLevel)
        {
            HideAllPanels();
            var level = baseLevel as Level;
            level.currencyChanged += CurrencyChangedUI;
            level.healthChanged += HealthChanged;

            gamePanel.UpdateLevelIndex(PersistManager.Instance.displayingLevelIdx);
            gamePanel.UpdateProgressBar(0, false);
            gamePanel.Display();
            gamePanel.maxHealthStatus = level.health;
            gamePanel.UpdateHealth(level.health);
            gamePanel.UpdateCurrency(level.currency);

            //tutorialPanel.Display();
        }

        private void CurrencyChangedUI(int currency)
        {
            gamePanel.UpdateCurrency(currency);
        }

        private void HealthChanged(int h)
        {
            gamePanel.UpdateHealth(h);
        }

        private void LevelDidStart(BaseLevel baseLevel)
        {
            tutorialPanel.Hide();
        }

        private void LevelDidSuccess(BaseLevel baseLevel)
        {
            HideAllPanels();
            var level = baseLevel as Level;
            level.currencyChanged -= CurrencyChangedUI;
            level.healthChanged -= HealthChanged;
            levelSucceededPanel.Display();
        }

        private void LevelDidFail(BaseLevel baseLevel)
        {
            HideAllPanels();
            var level = baseLevel as Level;
            level.currencyChanged -= CurrencyChangedUI;
            level.healthChanged -= HealthChanged;
            levelFailedPanel.Display();
        }

        #endregion


        #region User Interaction Logic

        public void GamePanel_PauseButtonTapped(GamePanel gamePanel)
        {
            GameManager.Instance.PauseGame();
            this.gamePanel.Hide();
            tutorialPanel.Hide();
            pausePanel.Display();
        }

        public void GamePanel_EditorButtonTapped(GamePanel gamePanel)
        {
            editorPanel.Display();
        }

        public void SettingsPanel_VolumeChanged(SettingsPanel settingsPanel, float value)
        {
            AudioManager.Instance.ChangeVolume(value);
        }
        
        public void SettingsPanel_EffectVolumeChanged(SettingsPanel settingsPanel, float value)
        {
            AudioManager.Instance.ChangeEffectVolume(value);
        }

        public void SettingsPanel_CloseButtonTapped(SettingsPanel settingsPanel)
        {
            GameManager.Instance.ResumeGame();
            this.settingsPanel.Hide();
            //gamePanel.DisplaySettingsButton(true);
            if (LevelManager.Instance.currentLevel.state == BaseLevel.State.Unknown ||
                LevelManager.Instance.currentLevel.state == BaseLevel.State.Loading ||
                LevelManager.Instance.currentLevel.state == BaseLevel.State.Loaded)
            {
                //tutorialPanel.Display();
            }
        }

        public void LevelFailedPanel_TryAgainButtonTapped(LevelFailedPanel levelFailedPanel)
        {
            LevelManager.Instance.RestartCurrentLevel();
        }

        public void LevelSucceededPanel_NextButtonTapped(LevelSucceededPanel levelSucceededPanel)
        {
            LevelManager.Instance.LoadNextLevel();
        }

        public void EditorPanel_LoadLevelButtonTapped(EditorPanel panel, int levelIdx)
        {
            LevelManager.Instance.LoadLevel(levelIdx);
        }

        public void EditorPanel_CloseButtonTapped(EditorPanel panel)
        {
            editorPanel.Hide();
        }
        
        public void PausePanel_ContinueButtonTapped(PausePanel pausePanel)
        {
            GameManager.Instance.ResumeGame();
            this.pausePanel.Hide();
            gamePanel.Display();
            if (LevelManager.Instance.currentLevel.state == BaseLevel.State.Unknown ||
                LevelManager.Instance.currentLevel.state == BaseLevel.State.Loading ||
                LevelManager.Instance.currentLevel.state == BaseLevel.State.Loaded)
            {
                //gamePanel.Display();
            }
        }

        public void PausePanel_SettingsButtonTapped(PausePanel pausePanel)
        {
            settingsPanel.Display();
        }

        public void PausePanel_RestartButtonTapped(PausePanel pausePanel)
        {
            LevelManager.Instance.RestartCurrentLevel();
            GameManager.Instance.ResumeGame();
        }

        #endregion


        #region Private Methods

        private void HideAllPanels()
        {
            gamePanel.Hide();
            settingsPanel.Hide();
            levelSucceededPanel.Hide();
            levelFailedPanel.Hide();
            tutorialPanel.Hide();
            editorPanel.Hide();
        }

        #endregion

        
    }
}