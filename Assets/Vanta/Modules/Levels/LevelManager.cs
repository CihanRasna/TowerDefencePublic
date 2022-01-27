using System.Collections.Generic;
using _Game.Levels.Base;
using UnityEngine;
using Vanta.Core;
using Vanta.Persist;
using Random = UnityEngine.Random;



namespace Vanta.Levels
{

    public class LevelManager : Singleton<LevelManager>, ILevelDelegate
    {

        public List<LevelInfo> levels => _levels;
        [HideInInspector] [SerializeField] private List<LevelInfo> _levels;

        public BaseLevel currentLevel => _currentLevel;
        private BaseLevel _currentLevel;

        [SerializeField] private bool forceLevel = false;
        [SerializeField] private int forceLevelIdx = -1;
        [SerializeField] private bool forceSameLevel = false;

        public GameObject defaultEnvironment;
        private GameObject currentEnvironment;

        // Events
        public delegate void LevelEvent(BaseLevel baseLevel);
        public delegate void LevelEvent1i(BaseLevel baseLevel, int arg1);

        public event LevelEvent levelDidLoad;
        public event LevelEvent levelDidStart;
        public event LevelEvent levelDidSuccees;
        public event LevelEvent levelDidFail;
        public event LevelEvent levelWillUnload;



    #region Life Cycle
        
        private void Start()
        {
            int lastPlayedLevelIdx = PersistManager.Instance.currentLevelIdx;

#if UNITY_EDITOR
            if (forceLevel)
            {
                lastPlayedLevelIdx = forceLevelIdx;
            }
#endif

            LoadLevel(lastPlayedLevelIdx);
        }

    #endregion
        


    #region Loading

        public void LoadLevel(LevelInfo levelInfo)
        {
            var levelIdx = levels.IndexOf(levelInfo);
            LoadLevel(levelIdx);
        }

        public void LoadLevel(int levelIdx)
        {
            var totalLevelCount = levels.Count;
            
            if (levelIdx >= totalLevelCount)
            {
                levelIdx = Random.Range(0, totalLevelCount);
            }

            UnloadCurrentLevel();

            var levelInfo = levels[levelIdx];
            var currentLevelObj = Instantiate(levelInfo.prefab);
            var level = currentLevelObj.GetComponent<BaseLevel>();
            level.listener = this;
            _currentLevel = level;

            if (defaultEnvironment != null)
            {
                currentEnvironment = Instantiate(defaultEnvironment);   
            }

            PersistManager.Instance.currentLevelIdx = levelIdx;
        }

        private void UnloadCurrentLevel()
        {
            if (_currentLevel != null)
            {
                levelWillUnload?.Invoke(_currentLevel);
                Destroy(_currentLevel.gameObject);

                if (currentEnvironment != null)
                {
                    Destroy(currentEnvironment);
                    currentEnvironment = null;
                }
            }
        }

        public void RestartCurrentLevel()
        {
            LoadLevel(PersistManager.Instance.currentLevelIdx);
        }

        public void LoadNextLevel()
        {
            PersistManager.Instance.displayingLevelIdx++;
         
            bool isUnityEditor = false;
#if UNITY_EDITOR
            isUnityEditor = true;
#endif
            if (!isUnityEditor || !forceSameLevel)
            {
                PersistManager.Instance.currentLevelIdx++;
            }
         
            LoadLevel(PersistManager.Instance.currentLevelIdx);
        }

    #endregion



    #region Level Delegate

        public void Level_DidLoad(BaseLevel baseLevel) => levelDidLoad?.Invoke(_currentLevel);
        public void Level_DidStart(BaseLevel baseLevel)
        {
            var level = baseLevel as Level;
            level.InvokeEnemy();
            levelDidStart?.Invoke(_currentLevel);
        }

        public void Level_DidSuccess(BaseLevel baseLevel, float score) => levelDidSuccees.Invoke(_currentLevel);
        public void Level_DidFail(BaseLevel baseLevel) => levelDidFail?.Invoke(_currentLevel);

    #endregion

    }

}