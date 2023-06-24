using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Units.HireDefendersSystem
{
    public class HireUnitView
    {
        
        public event Action OnCloseButtonClick;
        public event Action<int> OnHireButtonClick;
        
        private HireUnitUIView _hireUnitUIView;
        private Sprite _emptySlotBackground;
        private List<Button> _hireButtons = new List<Button>();
        private List<Image> _hireButtonsImages = new List<Image>();


        public HireUnitView(HireUnitUIView hireUnitUIView)
        {
            _hireUnitUIView = hireUnitUIView;
            _emptySlotBackground = _hireUnitUIView.HireButton.transform.GetComponent<Image>().sprite;
            _hireUnitUIView.HireButton.gameObject.SetActive(false);
        }

        public void Show(List<Sprite> sprites)
        {
            _hireUnitUIView.Root.SetActive(true);
            _hireUnitUIView.CloseButton.onClick.AddListener(CloseButtonClick);
            for (int index = 0; index < sprites.Count; index++)
            {
                ShowButton(index);
                GetImage(index).sprite = sprites[index];
                SubscribeToButton(index);
            }
        }

        public void Hide()
        {
            for (int index = 0; index < _hireButtons.Count; index++)
            {
                _hireButtons[index].onClick.RemoveAllListeners();
                _hireButtonsImages[index].sprite = _emptySlotBackground;
                _hireButtons[index].gameObject.SetActive(false);
            }
            _hireUnitUIView.Root.SetActive(false);
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
            if (index >= _hireButtonsImages.Count)
            {
                AddButtons(index + 1);
            }
            return _hireButtonsImages[index];
        }

        private void SubscribeToButton(int index)
        {
            if (index >= _hireButtons.Count)
            {
                AddButtons(index + 1);
            }
            _hireButtons[index].onClick.AddListener(delegate { HireButtonClick(index); });
        }

        private void ShowButton(int index)
        {
            if (index >= _hireButtons.Count)
            {
                AddButtons(index + 1);
            }
            _hireButtons[index].gameObject.SetActive(true);
        }

        private void AddButtons(int newCapacity)
        {
            Transform prototype = _hireUnitUIView.HireButton.transform;
            Transform container = _hireUnitUIView.ButtonContainer;
            while (_hireButtons.Count < newCapacity)
            {
                Transform newObject = GameObject.Instantiate(prototype, container );
                _hireButtons.Add(newObject.GetComponent<Button>());
                _hireButtonsImages.Add(newObject.GetComponent<Image>());
                newObject.gameObject.SetActive(false);
            }
        }
        
    }
}