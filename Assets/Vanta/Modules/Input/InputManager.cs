using UnityEngine.EventSystems;
using Vanta.Core;
using Vanta.Levels;



namespace Vanta.Input
{
    public partial class InputManager : Singleton<InputManager>, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {

        private bool isPointerDown = false;

        public delegate void InputEvent(PointerEventData evetData);

        public event InputEvent pointerDidDown;
        public event InputEvent pointerDidMove;
        public event InputEvent pointerDidUp;
        
        

    #region Life Cycle

        private void Start()
        {
            LevelManager.Instance.levelDidLoad += LevelDidLoad;
            LevelManager.Instance.levelWillUnload += LevelWillUnload;
            
            /*
             * Assign InputManager as DataSource of available recognizers here.
             * 
             * Example:
             * _panRecognizer.dataSource = this;
             */
        }

    #endregion



    #region Level Events

        private void LevelDidLoad(BaseLevel baseLevel)
        {
            /*
             * Assign components as Delegate of available recognizers here.
             *
             * Example:
             * _panRecognizer.listener = level.player;
             */
        }

        private void LevelWillUnload(BaseLevel baseLevel)
        {
            /*
             * We must set listers of available recognizers to null before level will destroy.
             * 
             * Example:
             * _panRecognizer.listener = null;
             */
        }

    #endregion



    #region Pointer

        public void OnPointerDown(PointerEventData eventData)
        {
            isPointerDown = true;
            pointerDidDown?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            pointerDidMove?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPointerDown = false;
            pointerDidUp?.Invoke(eventData);
        }

        public bool IsPointerDown()
        {
            return isPointerDown;
        }

    #endregion

    }

}