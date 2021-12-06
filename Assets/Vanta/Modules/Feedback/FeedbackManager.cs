using UnityEngine;
using Vanta.Core;
using Vanta.Levels;
using Vanta.Persist;



namespace Vanta.Feedback
{
    public class FeedbackManager : Singleton<FeedbackManager>
    {
        private float lastHapticTime = 0;
        private float hapticDelay = 0.1f;

        [Header("Level Success")] 
        public AudioSource levelSuccess;



    #region Life Cycle

        private void Start()
        {
            LevelManager.Instance.levelDidSuccees += LevelDidSuccess;
        }

    #endregion



    #region Level Events

        private void LevelDidSuccess(BaseLevel baseLevel)
        {
            PlaySound(levelSuccess);
            GenerateHaptic(HapticTypes.Success);
        }

    #endregion



    #region Sounds

        public void PlaySound(AudioSource audioSource)
        {
            if (!PersistManager.Instance.soundsEnabled)
            {
                return;
            }

            audioSource.Play();
        }

    #endregion



    #region Haptics

        public void GenerateHaptic(HapticTypes type)
        {
            if (!PersistManager.Instance.hapticsEnabled)
            {
                return;
            }

            float now = Time.time;
            if (now - lastHapticTime >= hapticDelay)
            {
                lastHapticTime = now;
                FeedbackGenerator.Haptic(type);
            }
        }

    #endregion

    }

}