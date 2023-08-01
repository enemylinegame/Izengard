using System;
using System.Collections.Generic;
using ResourceSystem;
using ResourceSystem.SupportClases;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Units.HireDefendersSystem
{
    public class HireUnitView
    {
        // временная UI панель выбора защитника для найма
        
        public event Action OnCloseButtonClick;
        public event Action<int> OnHireButtonClick;
        
        private HireUnitUIView _hireUnitUIView;
        private Sprite _emptySlotBackground;
        private List<HireUnitSlotUiView> _slots = new();
        private bool _isVisible;

        public HireUnitView(HireUnitUIView hireUnitUIView)
        {
            _hireUnitUIView = hireUnitUIView;
            _emptySlotBackground = _hireUnitUIView.SlotPrototype.Icon.sprite;
            _hireUnitUIView.SlotPrototype.gameObject.SetActive(false);
            _isVisible = false;
            _hireUnitUIView.Root.SetActive(false);
        }

        public void Show(List<(Sprite, string, List<ResourcePriceModel>)> unitDescriptor)
        {
            if (_isVisible) return;
            _hireUnitUIView.Root.SetActive(true);
            _isVisible = true;
            _hireUnitUIView.CloseButton.onClick.AddListener(CloseButtonClick);
            for (int index = 0; index < unitDescriptor.Count; index++)
            {
                ShowButton(index);
                GetImage(index).sprite = unitDescriptor[index].Item1;
                _slots[index].UnitName.text = unitDescriptor[index].Item2;
                _slots[index].Cost.text = unitDescriptor[index].Item3.Find(model => model.ResourceType == ResourceType.Gold)?.Cost.ToString() ?? "0";
                SubscribeToButton(index);
            }
        }

        public void Hide()
        {
            for (int index = 0; index < _slots.Count; index++)
            {
                _slots[index].HireButton.onClick.RemoveAllListeners();
                _slots[index].Icon.sprite = _emptySlotBackground;
                _slots[index].gameObject.SetActive(false);
            }
            _hireUnitUIView.Root.SetActive(false);
            _isVisible = false;
            _hireUnitUIView.CloseButton.onClick.RemoveAllListeners();
        }

        private void CloseButtonClick()
        {
            OnCloseButtonClick?.Invoke();
        }

        private void HireButtonClick(int index)
        {
            OnHireButtonClick?.Invoke(index);
            //Hide();
        }

        private Image GetImage(int index)
        {
            if (index >= _slots.Count)
            {
                AddButtons(index + 1);
            }
            return _slots[index].Icon;
        }

        private void SubscribeToButton(int index)
        {
            if (index >= _slots.Count)
            {
                AddButtons(index + 1);
            }
            _slots[index].HireButton.onClick.AddListener(delegate { HireButtonClick(index); });
        }

        private void ShowButton(int index)
        {
            if (index >= _slots.Count)
            {
                AddButtons(index + 1);
            }
            _slots[index].gameObject.SetActive(true);
        }

        private void AddButtons(int newCapacity)
        {
            Transform prototype = _hireUnitUIView.SlotPrototype.transform;
            while (_slots.Count < newCapacity)
            {
                Transform newObject = GameObject.Instantiate(prototype, prototype.parent );
                _slots.Add(newObject.GetComponent<HireUnitSlotUiView>());
                newObject.gameObject.SetActive(false);
            }
        }

    }
}