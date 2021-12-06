using UnityEngine;
using Vanta.Core;



namespace Vanta.Persist
{
    
    public class PersistManager : Singleton<PersistManager>
    {
        
        private static string currentLevelIndexKey = "CurrentLevelIndex";
        private int _currentLevelIdx = 0;
        public int currentLevelIdx
        {
            get => _currentLevelIdx;
            set
            {
                _currentLevelIdx = value;
                PlayerPrefs.SetInt(currentLevelIndexKey, _currentLevelIdx);
                PlayerPrefs.Save();
            }
        }


        private static string displayingLevelIndexKey = "DisplayingLevelIndex";
        private int _displayingLevelIdx = 0;
        public int displayingLevelIdx
        {
            get => _displayingLevelIdx;
            set
            {
                _displayingLevelIdx = value;
                PlayerPrefs.SetInt(displayingLevelIndexKey, _displayingLevelIdx);
                PlayerPrefs.Save();
            }
        }
        
        
        private static string soundsEnabledKey = "SoundsEnabled";
        private bool _soundsEnabled = true;
        public bool soundsEnabled
        {
            get => _soundsEnabled;
            set
            {
                _soundsEnabled = value;
                PlayerPrefs.SetInt(soundsEnabledKey, _soundsEnabled ? 1 : 0);
                PlayerPrefs.Save();
            }
        }
        

        private static string hapticsEnabledKey = "HapticsEnabled";
        private bool _hapticsEnabled = true;
        public bool hapticsEnabled
        {
            get => _hapticsEnabled;
            set
            {
                _hapticsEnabled = value;
                PlayerPrefs.SetInt(hapticsEnabledKey, _hapticsEnabled ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        
        private static string recurringNotificationsSetKey = "RecurringNotificationsSet";
        private bool _recurringNotificationsSet = false;
        public bool recurringNotificationsSet
        {
            get => _recurringNotificationsSet;
            set
            {
                _recurringNotificationsSet = value;
                PlayerPrefs.SetInt(recurringNotificationsSetKey, _recurringNotificationsSet ? 1 : 0);
                PlayerPrefs.Save();
            }
        }



    #region Life Cycle

        protected override void Awake()
        {
            base.Awake();
            
            _currentLevelIdx = PlayerPrefs.GetInt(currentLevelIndexKey, 0);
            _displayingLevelIdx = PlayerPrefs.GetInt(displayingLevelIndexKey, 0);
            _soundsEnabled = PlayerPrefs.GetInt(soundsEnabledKey, 1) != 0;
            _hapticsEnabled = PlayerPrefs.GetInt(hapticsEnabledKey, 1) != 0;
            _recurringNotificationsSet = PlayerPrefs.GetInt(recurringNotificationsSetKey, 0) != 0;
        }

    #endregion
        
    }

}