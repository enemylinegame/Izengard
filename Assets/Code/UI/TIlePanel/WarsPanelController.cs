using CombatSystem.Views;
using UnityEngine;
using UnityEngine.Events;

namespace Code.UI
{
    public class WarsPanelController
    {
        private readonly WarsPanel _view;

        public WarsPanelController(WarsPanel view)
        {
            _view = view; 
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
        
        public void SubscribeDismissButton(UnityAction action)
        {
            _view.DismissButton.onClick.AddListener(action);
        }
        
        public void SubscribeToOtherTileButton(UnityAction action)
        {
            _view.ToOtherTileButton.onClick.AddListener(action);
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