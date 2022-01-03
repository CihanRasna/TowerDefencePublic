using UnityEngine;
using Vanta.Core;



namespace Vanta
{

    public class GameManager : Singleton<GameManager>
    {

        private float defaultTimeScale;



    #region Life Cycle

        private void Start()
        {
            // Application.targetFrameRate = 60;
            defaultTimeScale = Time.timeScale;
        }

    #endregion
        
        

    #region Play / Pause

        public void PauseGame()
        {
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            Time.timeScale = defaultTimeScale;
        }

        public bool IsPaused() => Time.timeScale == 0;

    #endregion

    }

}