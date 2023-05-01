using System;
using UnityEngine;
using UnityEngine.UI;


namespace CombatSystem.Views
{
    public sealed class DefenderSlotView
    {

        public event Action<int> OnHireDissmisClick;
        public event Action<bool, int> OnInBarrackChanged;

        private Transform _slotRoot;
        private Image _unitIcon;
        private Sprite _emptySprite;
        private Sprite _unitSprite;
        private Toggle _inBarrack;
        private Button _hireDismissButton;

        private readonly int _number;
        private bool _isInBarrack;
        

        public bool IsInBarrack
        {
            get 
            {
                return _inBarrack;
            }
            set
            {
                _isInBarrack = value;
                _inBarrack.isOn = value;
            }
        }


        public DefenderSlotView(Transform slotRoot, Sprite unitSprite, int number)
        {
            _slotRoot = slotRoot;
            _unitSprite = unitSprite;
            _unitIcon = _slotRoot.GetComponent<Image>();
            _emptySprite = _unitIcon.sprite;
            _inBarrack = _slotRoot.GetComponent<Toggle>();
            _inBarrack.isOn = _isInBarrack;
            _inBarrack.onValueChanged.AddListener(InBarrackStateChanged);
            _hireDismissButton = _slotRoot.GetComponent<Button>();
            _hireDismissButton.onClick.AddListener(HireDissmisClick);
            _number = number;
        }


        private void HireDissmisClick()
        {
            OnHireDissmisClick?.Invoke(_number);
        }

        private void InBarrackStateChanged(bool isOn)
        {
            Debug.Log($"DefnderSlotView->InBarrackChanged: _isInBarrack = {_isInBarrack}; isOn = {isOn};");
            if (_isInBarrack != isOn)
            {
                _isInBarrack = isOn;
                OnInBarrackChanged?.Invoke(isOn, _number);
            }
        }

        public void SetUnit(bool isUnitExist)
        {
            if (isUnitExist)
            {
                _unitIcon.sprite = _unitSprite;
            }
            else
            {
                _unitIcon.sprite = _emptySprite;
            }

            IsInBarrack = false;
        }

    }
}
