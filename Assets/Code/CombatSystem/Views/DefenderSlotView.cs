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

        private Image _unitIcon;
        private Sprite _emptySprite;
        private Sprite _unitSprite;
        private Toggle _inBarrack;
        private Button _hireButton;
        private Button _dismissButton;

        private IDefenderUnitView _unit;
        private int _number;
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

        public bool IsUsed { get => _unit != null; }

        public IDefenderUnitView DefenderUnitView { get => _unit; }


        public DefenderSlotView(DefenderSlotUI slotRoot, Sprite unitSprite, int number)
        {
            _unitSprite = unitSprite;
            _unitIcon = slotRoot.UnitIcon;
            _emptySprite = _unitIcon.sprite;
            _inBarrack = slotRoot.SendToBarrackToggle;
            _inBarrack.isOn = _isInBarrack;
            _inBarrack.onValueChanged.AddListener(InBarrackStateChanged);
            _hireButton = slotRoot.HireButton;
            _hireButton.onClick.AddListener(HireClick);
            _dismissButton = slotRoot.DismissButton;
            _dismissButton.onClick.AddListener(DissmisClick);
            _dismissButton.gameObject.SetActive(false);

            _number = number;
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
            _unitIcon.sprite = _unitSprite;
            _unit = unit;
            _inBarrack.gameObject.SetActive(true);
            IsInBarrack = false;
            _hireButton.gameObject.SetActive(false);
            _dismissButton.gameObject.SetActive(true);
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
