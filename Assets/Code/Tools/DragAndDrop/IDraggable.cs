using UnityEngine;
using UnityEngine.EventSystems;

namespace Tools.DragAndDrop
{
    public interface IDraggable : IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler 
    {
        bool IsPathEnd { get; }

        public void Init(Canvas canvas);

        public void ReachedTarget();
    }
}
