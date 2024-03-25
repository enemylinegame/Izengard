using System;

namespace Tools.DragAndDrop
{
    public interface IDropTracker
    {
        event Action<IDraggable> OnDragDroped;
    }
}
