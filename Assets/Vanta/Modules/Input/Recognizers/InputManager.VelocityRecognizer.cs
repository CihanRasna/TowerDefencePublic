using UnityEngine;



namespace Vanta.Input
{
    
    public partial class InputManager : VelocityRecognizer.IVelocityRecognizerDataSource
    {
        
        // [HideInInspector] public VelocityRecognizer velocityRecognizer => _velocityRecognizer;
        // [SerializeField] private VelocityRecognizer _velocityRecognizer;
        
        

    #region Velocity Recognizer Data Source

        public Vector2 VelocityRecognizer_InputPositionForCurrentFrame(VelocityRecognizer recognizer)
        {
            return Vector2.zero;
            // return GetJoystickPosition();
        }

    #endregion
        
    }

}
