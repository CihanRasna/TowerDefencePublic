using System;
using UnityEngine;
using Vanta.Core;
using Random = UnityEngine.Random;



namespace Vanta.Utilities
{

    public class CameraController : VantaBehaviour
    {

        public enum State
        {
            Idle = 0,
            PlayerAttack = 1,
            LevelSucceed = 2,
            LevelFail = 3
        }

        [HideInInspector]
        public State state
        {
            get => _state;
            set
            {
                _state = value;

                var settings = SettingForState(state);
                previousSettings = currentSettings;
                currentSettings = settings;

                ApplySettings();
            }
        }

        private State _state;

        [Serializable]
        public struct Settings
        {
            public Vector3 offset;
            public Vector3 rotation;
            public float entryDuration;
            public float releaseDuration;
            public bool usePreviousReleaseDuration;
            public Transform target;
            public iTween.EaseType curve;
        }

        [Header("Smoothness")] [Space(10)] public float positionSmoothness;
        public float rotationSmoothness;
        public bool smoothPositionX;
        public bool smoothPositionY;
        public bool smoothPositionZ;

        public State initialState;

        [Header("Settings")] [Space(10)] [Space(10)]
        public Settings idleSettings;

        [Space(10)] public Settings playerAttackSettings;
        [Space(10)] public Settings levelSucceedSettings;
        [Space(10)] public Settings levelFailSettings;

        private Camera _camera;
        private Vector3 currentOffset;
        private Vector3 currentRotation;

        private Settings? previousSettings;
        private Settings currentSettings;
        private Vector3 lastAppliedStartOffset;
        private Vector3 lastAppliedStartRotation;

        private float shakeDuration;
        private float shakeMagnitude;
        private float decreaseFactor = 1;
        private Vector3 currentShake;

        public bool isAnimating => _isAnimating;
        private bool _isAnimating = false;
        private Action stateCompletion;



    #region Life Cycle

        private void Start()
        {
            _camera = Camera.main;

            _state = initialState;
            currentSettings = SettingForState(_state);
            currentOffset = currentSettings.offset;
            currentRotation = currentSettings.rotation;

            var initialCameraPosition = currentSettings.target.transform.position + currentSettings.offset;
            MoveCamera(initialCameraPosition, 0);
            RotateCamera(currentSettings.rotation, 0);
        }

        private void LateUpdate()
        {
            if (currentSettings.target == null)
            {
                return;
            }

            var position = currentSettings.target.position + currentOffset;

            if (shakeDuration > 0)
            {
                position += Random.insideUnitSphere * shakeMagnitude;
                shakeDuration -= Time.deltaTime * decreaseFactor;
            }

            MoveCamera(position, positionSmoothness);
            RotateCamera(currentRotation, rotationSmoothness);
        }

    #endregion



    #region Shake

        public void PerformShake(float amount, float duration)
        {
            shakeDuration = duration;
            shakeMagnitude = amount;
        }

    #endregion



    #region Camera

        private void MoveCamera(Vector3 position, float smooth)
        {
            var finalPosition = position;
            if (smooth > 0)
            {
                finalPosition = Vector3.Lerp(_camera.transform.position, position, smooth * Time.deltaTime);
                if (!smoothPositionX) finalPosition.x = position.x;
                if (!smoothPositionY) finalPosition.y = position.y;
                if (!smoothPositionZ) finalPosition.z = position.z;
            }

            _camera.transform.position = finalPosition;
        }

        private void RotateCamera(Vector3 rotation, float smooth)
        {
            var finalRotation = Quaternion.Euler(rotation);
            if (smooth > 0)
            {
                finalRotation = Quaternion.RotateTowards(_camera.transform.localRotation, finalRotation,
                    smooth * Time.deltaTime);
            }

            _camera.transform.localRotation = finalRotation;
        }

    #endregion



    #region Settings

        public void SetState(State newState, Action completion)
        {
            stateCompletion = completion;
            state = newState;
        }

        private Settings SettingForState(State state)
        {
            var settings = idleSettings;
            if (state == State.PlayerAttack)
            {
                settings = playerAttackSettings;
            }
            else if (state == State.LevelSucceed)
            {
                settings = levelSucceedSettings;
            }
            else if (state == State.LevelFail)
            {
                settings = levelFailSettings;
            }

            return settings;
        }

        private void ApplySettings()
        {
            iTween.Stop(gameObject);

            float duration = currentSettings.entryDuration;
            var curve = currentSettings.curve;
            if (currentSettings.usePreviousReleaseDuration && previousSettings.HasValue)
            {
                duration = previousSettings.Value.releaseDuration;
                curve = previousSettings.Value.curve;
            }

            lastAppliedStartOffset = currentOffset;
            lastAppliedStartRotation = currentRotation;

            _isAnimating = true;

            iTween.ValueTo(gameObject, iTween.Hash(
                "from", 0,
                "to", 1,
                "time", duration,
                "onupdatetarget", gameObject,
                "onupdate", "OnOffsetAnimationTween",
                "oncompletetarget", gameObject,
                "oncomplete", "OnOffsetAnimationComplete",
                "easetype", curve,
                "looptype", iTween.LoopType.none
            ));
        }

        private void OnOffsetAnimationTween(float value)
        {
            currentOffset = Vector3.Lerp(lastAppliedStartOffset, currentSettings.offset, value);
            currentRotation = Vector3.Lerp(lastAppliedStartRotation, currentSettings.rotation, value);
        }

        private void OnOffsetAnimationComplete()
        {
            _isAnimating = false;

            stateCompletion?.Invoke();
            stateCompletion = null;
        }

    #endregion

    }

}