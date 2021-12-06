using System;
using UnityEngine;
using Vanta.Core;



namespace Vanta.Input
{

    public class VelocityRecognizer : VantaBehaviour
    {

        public interface IVelocityRecognizerDataSource
        {
            Vector2 VelocityRecognizer_InputPositionForCurrentFrame(VelocityRecognizer recognizer);
        }

        [HideInInspector] public Vector2 velocity => _velocity;
        private Vector2 _velocity;

        [Range(0.01f, 10)] public float damping = 1;
        public bool shouldProcess = true;

        [HideInInspector] public IVelocityRecognizerDataSource dataSource;
        
        private Vector2? lastPosition = null;



    #region Life Cycle

        private void Update()
        {
            if (shouldProcess && dataSource != null)
            {
                var currentPosition = dataSource.VelocityRecognizer_InputPositionForCurrentFrame(this);
                
                if (lastPosition.HasValue)
                {
                    if (lastPosition.Value != currentPosition)
                    {
                        var deltaX = currentPosition.x - lastPosition.Value.x;
                        var deltaY = currentPosition.y - lastPosition.Value.y;
                        var velocitySinceLastFrame = new Vector2(deltaX / Time.deltaTime, deltaY / Time.deltaTime);
                
                        _velocity += velocitySinceLastFrame;
                    }
                }

                lastPosition = currentPosition;
            }
            
            float ratio = (10 - damping) * 0.1f;
            var dampedVelocity = _velocity * ratio;
        
            if (Math.Abs(dampedVelocity.x) < 0.01f) dampedVelocity.x = 0;
            if (Math.Abs(dampedVelocity.y) < 0.01f) dampedVelocity.y = 0;
        
            _velocity = dampedVelocity;
        }

    #endregion



    #region Public Methods

        public void ForceStop()
        {
            _velocity = Vector2.zero;
            lastPosition = null;
        }

    #endregion
        
    }

}
