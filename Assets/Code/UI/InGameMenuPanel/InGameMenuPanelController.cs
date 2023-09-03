using System;
using UnityEngine.Events;

namespace Code.UI
{
    public class InGameMenuPanelController
    {
        private InGameMenuPanel _view;

        public event Action ContinueButton;
        public event Action RestartButton;
        public event Action QuitButton;
        public event Action SettingsButton;

        public InGameMenuPanelController(InGameMenuPanelFactory factory)
        {
            _view = factory.GetView(factory.UIElementsConfig.InGameMenu);
            DeactivateMenu();
            
            _view.ContinueButton.onClick.AddListener((() => ContinueButton?.Invoke()));
            _view.RestartButton.onClick.AddListener((() => RestartButton?.Invoke()));
            _view.QuitButton.onClick.AddListener((() => QuitButton?.Invoke()));
            _view.SettingsButton.onClick.AddListener((() => SettingsButton?.Invoke()));
        }

        public void ActivateMenu()
        {
            _view.RootGameObject.SetActive(true);
        }

        public void DeactivateMenu()
        {
            _view.RootGameObject.SetActive(false);
        }

        public void DisposeButtons()
        {
            _view.ContinueButton.onClick.RemoveAllListeners();
            _view.RestartButton.onClick.RemoveAllListeners();
            _view.QuitButton.onClick.RemoveAllListeners();
            _view.SettingsButton.onClick.RemoveAllListeners();
        }
    }
}