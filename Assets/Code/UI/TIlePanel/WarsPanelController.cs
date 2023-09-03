using System;
using CombatSystem.Views;
using UnityEngine;
using UnityEngine.Events;

namespace Code.UI
{
    public class WarsPanelController
    {
        private readonly WarsPanel _view;
        
        public event Action DismissButton;
        public event Action ToOtherTileButton;
        public WarsPanelController(WarsPanel view)
        {
            _view = view; 
            
            _view.DismissButton.onClick.AddListener((() => DismissButton?.Invoke()));
            _view.ToOtherTileButton.onClick.AddListener((() => ToOtherTileButton?.Invoke()));
        }
        
        public GameObject SubscribeEnterToBarracks(UnityAction action)
        {
            _view.EnterToBarracks.onClick.AddListener(action);
            return _view.EnterToBarracks.gameObject;
        }
        
        public GameObject SubscribeExitFromBarracks(UnityAction action)
        {
            _view.ExitFromBarracks.onClick.AddListener(action);
            return _view.ExitFromBarracks.gameObject;
        }

        public DefenderSlotUI[] GetDefenderSlotUI()
        {
            return _view.Slots;
        }
        
        public void DisposeButtons()
        {
            _view.EnterToBarracks.onClick.RemoveAllListeners();
            _view.ExitFromBarracks.onClick.RemoveAllListeners();
            _view.DismissButton.onClick.RemoveAllListeners();
            _view.ToOtherTileButton.onClick.RemoveAllListeners();
        }
    }
}