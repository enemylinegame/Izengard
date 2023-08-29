using UnityEngine.Events;
using UnityEngine.UI;

namespace Code.UI
{
    public class EndGameScreenPanelController
    {
        private EndGameScreenPanel _view;
        
        public EndGameScreenPanelController(EndGameScreenPanelFactory factory)
        {
            _view = factory.GetView(factory.UIElementsConfig.EndGameScreenPanel);
        }

        public void SubscribeRestartButton(UnityAction action)
        {
            _view.RestartBtn.onClick.AddListener(action);
        }
        
        public void SubscribeBackToMenuButton(UnityAction action)
        {
            _view.BackToMenuBtn.onClick.AddListener(action);
        }

        public Image GetBackGroundGameOverScreen()
        {
            return _view.BackGroundGameOverScreen;
        }
        
        public void UnsubscribeRestartButton()
        {
            _view.RestartBtn.onClick.RemoveAllListeners();
        }
    }
}