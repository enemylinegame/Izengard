using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tools.DragAndDrop
{
    public class DragAndDropComponent : MonoBehaviour, IDraggable
    {
        private float _dragScaleFactor;
        private Transform _parentDragTransform;
        private RectTransform _rectTransform;

        private Transform _originalParent;
        private Vector2 _originalPosition;
        public bool IsPathEnd { get; private set; }

        public void Init(Canvas canvas)
        {
            _dragScaleFactor = canvas.scaleFactor;
            
            _parentDragTransform = canvas.transform;

            _rectTransform = GetComponent<RectTransform>();
            _originalParent = _rectTransform.parent;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _originalPosition = _rectTransform.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _dragScaleFactor;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            IsPathEnd = false;

            _rectTransform.SetParent(_parentDragTransform);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (IsPathEnd == false)
            {
                _rectTransform.anchoredPosition = _originalPosition;
            }

            _rectTransform.SetParent(_originalParent);
        }
    }
}
