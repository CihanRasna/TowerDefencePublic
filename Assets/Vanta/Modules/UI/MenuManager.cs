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
        IEditorPanelDelegate
    {
        
        [SerializeField] private GamePanel _gamePanel;
        [SerializeField] private SettingsPanel _settingsPanel;
        [SerializeField] private LevelFailedPanel _levelFailedPanel;
        [SerializeField] private LevelSucceededPanel _levelSucceededPanel;
        [SerializeField] private TutorialPanel _tutorialPanel;
        [SerializeField] private EditorPanel _editorPanel;
        
        [SerializeField] [HideInInspector] private SafeArea.SimulationDevice _simulatingDevice = SafeArea.SimulationDevice.None;


    
    #region Life Cycle
    
        protected override void Awake()
        {
            base.Awake();
            
#if UNITY_EDITOR
            SafeArea.simulationDevice = _simulatingDevice;
#endif
        }

        private void Start()
        {
            _gamePanel.listener = this;
            _settingsPanel.listener = this;
            _levelFailedPanel.listener = this;
            _levelSucceededPanel.listener = this;
            _editorPanel.listener = this;

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
        
            _gamePanel.UpdateLevelIndex(PersistManager.Instance.displayingLevelIdx);
            _gamePanel.UpdateProgressBar(0, false);
            _gamePanel.Display();
        
            _tutorialPanel.Display();
        }

        private void LevelDidStart(BaseLevel baseLevel)
        {
            _tutorialPanel.Hide();
        }

        private void LevelDidSuccess(BaseLevel baseLevel)
        {
            HideAllPanels();
            _levelSucceededPanel.Display();
        }

        private void LevelDidFail(BaseLevel baseLevel)
        {
            HideAllPanels();
            _levelFailedPanel.Display();
        }

    #endregion
    
    
    
    #region User Interaction Logic

        public void GamePanel_SettingsButtonTapped(GamePanel gamePanel)
        {
            GameManager.Instance.PauseGame();
            _gamePanel.DisplaySettingsButton(false);
            _tutorialPanel.Hide();
            _settingsPanel.Display();
        }

        public void GamePanel_EditorButtonTapped(GamePanel gamePanel)
        {
            _editorPanel.Display();
        }

        public void SettingsPanel_CloseButtonTapped(SettingsPanel settingsPanel)
        {
            GameManager.Instance.ResumeGame();
            _settingsPanel.Hide();
            _gamePanel.DisplaySettingsButton(true);
            if (LevelManager.Instance.currentLevel.state == BaseLevel.State.Unknown ||
                LevelManager.Instance.currentLevel.state == BaseLevel.State.Loading ||
                LevelManager.Instance.currentLevel.state == BaseLevel.State.Loaded)
            {
                _tutorialPanel.Display();
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
            _editorPanel.Hide();
        }

    #endregion
    
    
    
    #region Private Methods

        private void HideAllPanels()
        {
            _gamePanel.Hide();
            _settingsPanel.Hide();
            _levelSucceededPanel.Hide();
            _levelFailedPanel.Hide();
            _tutorialPanel.Hide();
            _editorPanel.Hide();
        }
    
    #endregion

    }

}