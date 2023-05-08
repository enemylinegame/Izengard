using System;
using UnityEngine;
using UnityEngine.UI;


namespace CombatSystem.Views
{
    public sealed class DefenderSlotView
    {

        public event Action<int> OnHireClick;
        public event Action<int> OnDissmisClick;
        public event Action<bool, int> OnInBarrackChanged;

        private DefenderSlotUI _uiSlot;
        private Image _unitIcon;
        private Sprite _emptySprite;
        private Sprite _unitSprite;
        private Toggle _inBarrack;
        private Button _hireButton;
        private Button _dismissButton;

        private IDefenderUnitView _unit;
        private int _number;

        private bool _isInBarrack;
        private bool _isEnabled;


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

        public bool IsUsed { get => _unit != null; }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (_isEnabled != value)
                {
                    _uiSlot.gameObject.SetActive(value);
                    _isEnabled = value;
                }
            }
        }

        public IDefenderUnitView DefenderUnitView { get => _unit; }


        public DefenderSlotView(DefenderSlotUI slot, Sprite unitSprite, int number)
        {
            _uiSlot = slot;
            _unitSprite = unitSprite;
            _unitIcon = _uiSlot.UnitIcon;
            _emptySprite = _unitIcon.sprite;
            _inBarrack = _uiSlot.SendToBarrackToggle;
            _inBarrack.isOn = _isInBarrack;
            _inBarrack.onValueChanged.AddListener(InBarrackStateChanged);
            _hireButton = _uiSlot.HireButton;
            _hireButton.onClick.AddListener(HireClick);
            _dismissButton = _uiSlot.DismissButton;
            _dismissButton.onClick.AddListener(DissmisClick);
            _dismissButton.gameObject.SetActive(false);
            _inBarrack.gameObject.SetActive(false);

            _number = number;
            _isEnabled = true;
        }


        private void HireClick()
        {
            OnHireClick?.Invoke(_number);
        }

        private void DissmisClick()
        {
            OnDissmisClick?.Invoke(_number);
        }

        private void InBarrackStateChanged(bool isOn)
        {
            if (_isInBarrack != isOn)
            {
                _isInBarrack = isOn;
                OnInBarrackChanged?.Invoke(isOn, _number);
            }
        }

        public void SetUnit(IDefenderUnitView unit)
        {
            if (unit != null)
            {
                _unitIcon.sprite = unit.GetSprite();
                _unit = unit;
                _inBarrack.gameObject.SetActive(true);
                IsInBarrack = unit.IsInsideBarrack;
                _hireButton.gameObject.SetActive(false);
                _dismissButton.gameObject.SetActive(true);
            }
            else
            {
                RemoveUnit();
            }
        }

        public void RemoveUnit()
        {
            _unitIcon.sprite = _emptySprite;
            _unit = null;
            _inBarrack.gameObject.SetActive(false);
            _dismissButton.gameObject.SetActive(false);
            _hireButton.gameObject.SetActive(true);
        }

    }
}
