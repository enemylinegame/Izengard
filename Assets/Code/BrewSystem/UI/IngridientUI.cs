using BrewSystem.Model;
using System;
using TMPro;
using Tools.DragAndDrop;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BrewSystem.UI
{
    public class IngridientUI : MonoBehaviour, IDraggable, IDisposable
    {
        [SerializeField]
        private Button _clickedButon;
        [SerializeField]
        private TMP_Text _name;
        [SerializeField]
        private Image _icone;
        [SerializeField]
        private GameObject _selectedIcon;

        private int _id;
        public int Id => _id;

        public Action<int> OnClicked;

        public void InitUI(IngridientModel model)
        {
            _id = model.Id;
            _name.text = model.Data.Name;
            _icone.sprite = model.Data.Icon;
            _clickedButon.onClick.AddListener(() => OnClicked?.Invoke(_id));

            ChangeSelection(false);
        }

        public void ChangeSelection(bool state)
        {
            _selectedIcon.SetActive(state);
        }

        #region IDraggable

        [Header("IDraggable")]
        [SerializeField]
        private GameObject _draggableGO;

        private float _dragScaleFactor;
        private Transform _parentDragTransform;
        private RectTransform _rectTransform;

        private Transform _originalParent;

        public IDraggedData Data { get; private set; }

        public bool IsPathEnd { get; private set; }

        public void Init(Canvas canvas)
        {

            Data = new IngridientDragDataModel
            {
                Id = _id,
                Sprite = _icone.sprite
            };

            _draggableGO.GetComponent<Image>().sprite = _icone.sprite;
            _draggableGO.SetActive(false);

            _dragScaleFactor = canvas.scaleFactor;
            _parentDragTransform = canvas.transform;

            _rectTransform = _draggableGO.GetComponent<RectTransform>();
            _originalParent = _rectTransform.parent;
        }
        public void ReachedTarget()
        {
            IsPathEnd = true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log($"{name} : OnPointerDown");
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _dragScaleFactor;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _draggableGO.SetActive(true);
            IsPathEnd = false;

            _rectTransform.SetParent(_parentDragTransform);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _rectTransform.SetParent(_originalParent);
            _rectTransform.anchoredPosition = Vector2.zero;
            _draggableGO.SetActive(false);

            IsPathEnd = true;
        }

        #endregion

        #region IDisposable

        private bool _isDisposed;

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            _clickedButon.onClick.RemoveAllListeners();
        }

        #endregion
    }
}
