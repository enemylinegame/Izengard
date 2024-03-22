using System.Collections.Generic;
using UnityEngine;

namespace Tools.DragAndDrop
{
    public class DragAndDropController
    {
        private readonly Canvas _mainCanvas;

        private readonly List<IDraggable> _draggableObjects = new ();

        public DragAndDropController(Canvas canvas)
        {
            _mainCanvas = canvas;
        }

        public void AddDraggable(IDraggable draggable)
        {
            if (draggable == null)
                return;

            draggable.Init(_mainCanvas);

            _draggableObjects.Add(draggable);
        }

        public void RemoveDraggable(IDraggable draggable)
        {
            if (draggable == null)
                return;

            _draggableObjects.Remove(draggable);
        }
    }
}
