using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Code.UI
{
    public class EndGameScreenPanelController
    {
        private EndGameScreenPanel _view;
        
        public event Action RestartButton;
        public event Action BackToMenuButton;
        
        public EndGameScreenPanelController(EndGameScreenPanelFactory factory)
        {
            _view = factory.GetView(factory.UIElementsConfig.EndGameScreenPanel);
            
            _view.RestartButton.onClick.AddListener((() => RestartButton?.Invoke()));
            _view.BackToMenuButton.onClick.AddListener((() => BackToMenuButton?.Invoke()));
        }

        public Image GetBackGroundGameOverScreen()
        {
            return _view.BackGroundGameOverScreen;
        }
        
        public void DisposeButtons()
        {
            _view.RestartButton.onClick.RemoveAllListeners();
        }
    }
}