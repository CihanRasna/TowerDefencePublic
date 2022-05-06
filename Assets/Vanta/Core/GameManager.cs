using UnityEngine;

namespace Vanta.Core
{

    public class GameManager : Singleton<GameManager>
    {

        private float _defaultTimeScale;



    #region Life Cycle

        private void Start()
        {
            Application.targetFrameRate = 60;
            _defaultTimeScale = Time.timeScale;
        }

    #endregion
        
        

    #region Play / Pause

        public void PauseGame()
        {
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            Time.timeScale = _defaultTimeScale;
        }

        public bool IsPaused() => Time.timeScale == 0;

    #endregion

    }

}