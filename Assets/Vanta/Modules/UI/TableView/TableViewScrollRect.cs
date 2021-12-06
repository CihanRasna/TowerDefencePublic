using UnityEngine.EventSystems;
using UnityEngine.UI;



namespace Vanta.UI.TableView
{
    
    public class TableViewScrollRect : ScrollRect
    {
        
        public delegate void TableViewScrollEvent(TableViewScrollRect scrollRect, PointerEventData eventData);
        public event TableViewScrollEvent didBeginDragging;
        public event TableViewScrollEvent didDrag;
        public event TableViewScrollEvent didEndDragging;



    #region Drag Events

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            didBeginDragging?.Invoke(this, eventData);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            didDrag?.Invoke(this, eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            didEndDragging?.Invoke(this, eventData);
        }
        
    #endregion
        
    }

}
