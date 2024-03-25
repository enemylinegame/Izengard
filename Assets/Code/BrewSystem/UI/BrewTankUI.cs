using System;
using Tools.DragAndDrop;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BrewSystem.UI
{
    public class BrewTankUI : MonoBehaviour, IDropTracker, IDropHandler
    {
        public event Action<IDraggable> OnDragDroped;

        public void OnDrop(PointerEventData eventData)
        {
            var draggedGO = eventData.pointerDrag;
            Debug.Log($"OnDrop");

            if(draggedGO.TryGetComponent(out IDraggable draggable))
            {
                OnDragDroped?.Invoke(draggable);
            }
        }
    }
}
