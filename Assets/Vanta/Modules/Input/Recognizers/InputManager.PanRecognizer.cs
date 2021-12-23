using UnityEngine;



namespace Vanta.Input
{

    public partial class InputManager : PanRecognizer.IPanRecognizerDataSource
    {
        
        [HideInInspector] public PanRecognizer panRecognizer => _panRecognizer;
        [SerializeField] private PanRecognizer _panRecognizer;
        
        
        
        public Vector2 PanRecognizer_InputPositionForCurrentFrame(PanRecognizer recognizer)
        {
            // return Vector2.zero;
            var mousePosition = UnityEngine.Input.mousePosition;
            return new Vector2(mousePosition.x, mousePosition.y);
        }
        
    }
    
}

