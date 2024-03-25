using BrewSystem.Model;
using System;
using System.Collections.Generic;
using Tools.DragAndDrop;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BrewSystem.UI
{
    public class BrewTankUI : MonoBehaviour, IDropTracker, IDropHandler
    {
        [SerializeField]
        private Transform _ingridientSlotsHandler;
        [SerializeField]
        private GameObject _ingridientSlotPrefab;

        private List<IngridientSlotUI> _slotCollection = new();

        public event Action<int> OnAddedInSlot;

        public void InitUI(int maxIngridientSlots)
        {
            for (int i = 0; i < maxIngridientSlots; i++)
            {
                var slotView = Instantiate(_ingridientSlotPrefab, _ingridientSlotsHandler)
                    .GetComponent<IngridientSlotUI>();

                slotView.InitUI();

                _slotCollection.Add(slotView);
            }
        }

        public void SetObjectInSlot(IngridientDragDataModel dragDataModel)
        {

            for(int i =0; i< _slotCollection.Count; i++)
            {
                var slot = _slotCollection[i];
                
                if (slot.IsOccupied == false)
                {
                    slot.Set(dragDataModel.Sprite);

                    OnAddedInSlot?.Invoke(dragDataModel.Id);

                    break;
                }
            }
        }

        public void ResetUI()
        {
            for (int i = 0; i < _slotCollection.Count; i++)
            {
                var slot = _slotCollection[i];

                slot.Unset();
            }
        }

        #region IDropTracker

        public event Action<IDraggable> OnDragDroped;

        public void OnDrop(PointerEventData eventData)
        {
            var draggedGO = eventData.pointerDrag;
            Debug.Log($"OnDrop");

            if(draggedGO.TryGetComponent(out IDraggable draggable))
            {
                OnDragDroped?.Invoke(draggable);
                SetObjectInSlot((IngridientDragDataModel)draggable.Data);
            }
        }

        #endregion
    }
}
