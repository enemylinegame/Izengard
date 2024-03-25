using UnityEngine;
using UnityEngine.EventSystems;

namespace Tools.DragAndDrop
{
    public interface IDraggable : IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler 
    {
        IDraggedData Data { get; }

        bool IsPathEnd { get; }

        public void Init(Canvas canvas);

        public void ReachedTarget();
    }
}
