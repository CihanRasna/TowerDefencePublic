using UnityEngine;



namespace Vanta.Input
{

    public partial class InputManager : DirectionRecognizer.IDirectionRecognizerDataSource
    {
 
        // [HideInInspector] public DirectionRecognizer directionRecognizer => _directionRecognizer;
        // [SerializeField] private DirectionRecognizer _directionRecognizer;
        
        
        
        public Vector2 DirectionRecognizer_InputPositionForCurrentFrame(DirectionRecognizer recognizer)
        {
            return Vector2.zero;
            // return GetJoystickPosition();
        }
    }

}