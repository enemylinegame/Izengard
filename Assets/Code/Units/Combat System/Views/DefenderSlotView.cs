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
        public event Action<bool, int> OnSelected;

        private DefenderSlotUI _uiSlot;
        private Image _unitIcon;
        private Sprite _emptySprite;
        private Sprite _unitSprite;
        private Toggle _inBarrack;
        private Button _hireButton;
        private Button _dismissButton;
        private Button _iconButton;
        private GameObject _selectedBoard;
        private GameObject _hpBarRoot;
        private RectTransform _hpBar;
        private DefenderUnit _unit;

        private int _number;

        private bool _isInBarrack;
        private bool _isEnabled;
        private bool _isSelected;


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

        public bool IsSelected 
        { 
            get => _isSelected; 
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    _selectedBoard.SetActive(_isSelected);
                    if (_unit != null)
                    {
                        _unit.IsVisualSelection = value;
                    }
                }
            }
        }

        public DefenderUnit Unit { get => _unit; }


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
            _iconButton = _uiSlot.UnitIconButton;
            _iconButton.onClick.AddListener(ImageButtonClick);
            _selectedBoard = _uiSlot.SelectBoard;
            _selectedBoard.SetActive(_isSelected);
            _hpBarRoot = slot.HpBarRoot;
            _hpBar = slot.HpBar;
            
            _hpBarRoot.SetActive(false);
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

        private void ImageButtonClick()
        {
            if (IsUsed)
            {
                IsSelected = !_isSelected;
                OnSelected?.Invoke(_isSelected, _number);
            }
        }

        public void SetUnit(DefenderUnit unit)
        {
            if (unit != null)
            {
                _unitIcon.sprite = _unitSprite;
                _unit = unit;
                _inBarrack.gameObject.SetActive(true);
                IsInBarrack = unit.IsInBarrack;
                _hireButton.gameObject.SetActive(false);
                _dismissButton.gameObject.SetActive(true);
                _unit.OnHealthChanged += HealthChanged;
                _hpBarRoot.SetActive(true);
                UpdateHealth();
            }
            else
            {
                RemoveUnit();
            }
        }

        public void RemoveUnit()
        {
            IsSelected = false;
            _unitIcon.sprite = _emptySprite;
            _hpBarRoot.SetActive(false);
            _unit.OnHealthChanged -= HealthChanged;
            _unit = null;
            _inBarrack.gameObject.SetActive(false);
            _dismissButton.gameObject.SetActive(false);
            _hireButton.gameObject.SetActive(true);
        }

        private void HealthChanged(float maxHealth, float currentHealth)
        {
            if (maxHealth > 0.0f)
            {
                Vector2 anchorMax = _hpBar.anchorMax;
                anchorMax.x = currentHealth / maxHealth;
                _hpBar.anchorMax = anchorMax;
                _hpBar.offsetMax = Vector2.zero;
            }
        }

        private void UpdateHealth()
        {
            IHealthHolder health = _unit.HealthHolder;
            HealthChanged(health.MaxHealth, health.CurrentHealth);
        }

    }
}
