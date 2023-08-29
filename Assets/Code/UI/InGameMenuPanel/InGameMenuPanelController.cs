using UnityEngine.Events;

namespace Code.UI
{
    public class InGameMenuPanelController
    {
        private InGameMenuPanel _view;

        public InGameMenuPanelController(InGameMenuPanelFactory factory)
        {
            _view = factory.GetView(factory.UIElementsConfig.InGameMenu);
            DeactivateMenu();
        }

        public void ActivateMenu()
        {
            _view.RootGameObject.SetActive(true);
        }

        public void DeactivateMenu()
        {
            _view.RootGameObject.SetActive(false);
        }

        public void SubscribeContinueButton(UnityAction action)
        {
            _view.ContinueButton.onClick.AddListener(action);
        }
        
        public void SubscribeRestartButton(UnityAction action)
        {
            _view.RestartButton.onClick.AddListener(action);
        }
        
        public void SubscribeQuitButton(UnityAction action)
        {
            _view.QuitButton.onClick.AddListener(action);
        }
        
        public void SubscribeSettingsButton(UnityAction action)
        {
            _view.SettingsButton.onClick.AddListener(action);
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