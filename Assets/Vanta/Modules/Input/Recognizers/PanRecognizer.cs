using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Vanta.Core;



namespace Vanta.Input
{
    
    public class PanRecognizer : VantaBehaviour
    {
        
        public interface IPanRecognizerDataSource
        {
            Vector2 PanRecognizer_InputPositionForCurrentFrame(PanRecognizer recognizer);
        }

        public interface IPanRecognizerDelegate
        {
            bool PanRecognizerShouldStartListening(PanRecognizer recognizer);
            void PanRecognizerDidStartListening(PanRecognizer recognizer);
            void PanRecognizerDidChangeValue(PanRecognizer recognizer, float value);
            void PanRecognizerDidEndListening(PanRecognizer recognizer, bool forced);
        }

        public enum Direction
        {
            Horizontal,
            Vertical
        }

        [SerializeField] private Direction _direction = Direction.Horizontal;
        public Direction direction => _direction;

        [Range(1, 100)] public float speed = 1;
        [Range(1, 100)] public float dragSpeed = 1;
        [Range(0, 10)] public float damping = 1;
        [Range(0, 50)] public float velocityLimit;

        private bool isTouching = false;
        private List<float> positionHistory = new List<float>();

        [HideInInspector] public float velocity => _velocity;
        private float _velocity;

        [HideInInspector] public IPanRecognizerDelegate listener;
        [HideInInspector] public IPanRecognizerDataSource dataSource;



    #region Life Cycle

        private void Start()
        {
            var manager = InputManager.Instance;
            manager.pointerDidDown += OnPointerDown;
            manager.pointerDidUp += OnPointerUp;
        }

        private void OnDestroy()
        {
            var manager = InputManager.Instance;
            manager.pointerDidDown -= OnPointerDown;
            manager.pointerDidUp -= OnPointerUp;
        }

        private void Update()
        {
            if (dataSource == null)
            {
                return;
            }
            
            float mousePosition = CurrentInputPosition();
            
            if (isTouching)
            {
                float lastPosition = GetLastPositionFromHistory();
                float delta = mousePosition - lastPosition;
                
                float sign = delta < 0 ? -1 : 1;
                var deltaLimit = 60;
                delta = Math.Min(Math.Abs(delta), 60);
                delta *= sign;

                _velocity = 0;

                listener?.PanRecognizerDidChangeValue(this, delta * dragSpeed * 0.1f);
                AddPositionToHistory(mousePosition);
            }
            else if (_velocity != 0 && damping > 0)
            {
                float safeDamping = damping * (Time.deltaTime / Time.fixedDeltaTime);
                float ratio = (10 - safeDamping) * 0.1f;

                float damped = _velocity * ratio;
                if (Math.Abs(damped) < 0.01f)
                {
                    damped = 0;
                }

                float delta = (_velocity - damped) / Time.deltaTime;
                listener?.PanRecognizerDidChangeValue(this, delta * speed * 4);

                _velocity = damped;
            }
        }

    #endregion
        


    #region Pointer Events

        private void OnPointerDown(PointerEventData eventData)
        {
// #if UNITY_EDITOR
//             if (EventSystem.current.IsPointerOverGameObject())
// #else
//             if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch (0).fingerId))
// #endif
//             {
//                 return;
//             }
            
            if (!(listener?.PanRecognizerShouldStartListening(this) ?? true))
            {
                return;
            }
            
            AddPositionToHistory(CurrentInputPosition());
            isTouching = true;

            listener?.PanRecognizerDidStartListening(this);
        }

        private void OnPointerUp(PointerEventData eventData)
        {
            if (!isTouching)
            {
                return;
            }

            var mousePosition = CurrentInputPosition();
            
            float delta = (mousePosition - positionHistory[positionHistory.Count - 1]) / positionHistory.Count;
            float vel = delta * Time.deltaTime * (speed * 0.5f);
            float sign = vel < 0 ? -1 : 1;
            vel = Math.Min(Math.Abs(vel), velocityLimit) * sign;
                
            float currentSign = _velocity < 0 ? -1 : 1;
            if (currentSign == sign)
            {
                vel += _velocity;
            }

            _velocity = vel;
            isTouching = false;
            positionHistory.Clear();
            
            listener?.PanRecognizerDidEndListening(this, false);
        }

    #endregion



    #region Public Methods

        public void ForceStop()
        {
            _velocity = 0;
            isTouching = false;
            positionHistory.Clear();
            listener?.PanRecognizerDidEndListening(this, true);
        }

    #endregion



    #region Helper

        private float CurrentInputPosition()
        {
            var inputPosition = dataSource.PanRecognizer_InputPositionForCurrentFrame(this);
            return _direction == Direction.Horizontal ? inputPosition.x : inputPosition.y;
        }
        
        private void AddPositionToHistory(float position)
        {
            var count = 6;
            positionHistory.Insert(0, position);
            if (positionHistory.Count > count)
            {
                positionHistory.RemoveRange(count, positionHistory.Count - count);
            }
        }
        
        private float GetLastPositionFromHistory()
        {
            return positionHistory.Count > 0 ? positionHistory[0] : 0;
        }

    #endregion
        
    }   
    
}