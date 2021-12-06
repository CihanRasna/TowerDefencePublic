using UnityEngine;
using Vanta.Core;



namespace Vanta.Input
{

    public class DirectionRecognizer : VantaBehaviour
    {

        public interface IDirectionRecognizerDataSource
        {
            Vector2 DirectionRecognizer_InputPositionForCurrentFrame(DirectionRecognizer recognizer);
        }

        public interface IDirectionRecognizerDelegate
        {
            void DirectionRecognizer_DidChangeDirection(DirectionRecognizer sender, Direction direction);
        }
        
        public enum Direction
        {
            Unknown = -1,
            Right = 0,
            TopRight = 1,
            Top = 2,
            TopLeft = 3,
            Left = 4,
            BottomLeft = 5,
            Bottom = 6,
            BottomRight = 7
        }
        
        [HideInInspector] public Direction direction => _direction; 
        private Direction _direction = Direction.Unknown;

        [HideInInspector] public float degrees => _degrees;
        private float _degrees = 0;

        [HideInInspector] public IDirectionRecognizerDataSource dataSource; 
        public IDirectionRecognizerDelegate listener;



    #region Life Cycle

        private void Update()
        {
            var dir = Direction.Unknown;
            
            if (dataSource != null)
            {
                var currentPosition = dataSource.DirectionRecognizer_InputPositionForCurrentFrame(this);
                if (InputManager.Instance.IsPointerDown() && currentPosition != Vector2.zero)
                {
                    _degrees = DegreesFromVector(currentPosition);
                    dir = DirectionFromDegrees(_degrees);    
                }
            }
            
            if (dir != _direction)
            {
                _direction = dir;
                if (listener != null)
                {
                    listener.DirectionRecognizer_DidChangeDirection(this, _direction);
                }
            }
        }

    #endregion



    #region Private Methods

        private Direction DirectionFromDegrees(float degrees)
        {
            float directionCount = 8;
            float range = 360.0f / directionCount;
            float offset = range / 2;

            var simulatedDegrees = degrees + offset;
            var rangeIdx = (int)(simulatedDegrees / range) % directionCount;
            return (Direction)rangeIdx;
        }

        private float DegreesFromVector(Vector2 vector)
        {
            if (vector.x == 0)
            {
                return (vector.y > 0) ? 90 : (vector.y == 0) ? 0 : 270;
            }
            else if (vector.y == 0)
            {
                return (vector.x >= 0) ? 0 : 180;
            }

            var degrees = Mathf.Atan(vector.y / vector.x) * Mathf.Rad2Deg;
            if (vector.x < 0 && vector.y < 0)
            {
                degrees += 180;
            }
            else if (vector.x < 0)
            {
                degrees += 180;
            }
            else if (vector.y < 0)
            {
                degrees += 360;
            }

            return degrees;
        }

    #endregion
        
    }

}