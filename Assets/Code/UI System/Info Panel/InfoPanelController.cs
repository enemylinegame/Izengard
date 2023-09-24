using DG.Tweening;
using UnityEngine;

namespace Code.UI
{
    public class InfoPanelController
    {
        private readonly InfoPanel _panel;

        private Transform PositionPanel => _panel.PositionPanel;
        public InfoPanelController(InfoPanelFactory factory)
        {
            _panel = factory.GetView(factory.UIElementsConfig.InfoPanel);
            _panel.CloseButton.onClick.AddListener(ClosePanel);
            _panel.OpenButton.onClick.AddListener(OpenPanel);
        }

        private void ClosePanel()
        {
            PositionPanel.DOMoveX(-75, 0.2f, true);
            _panel.OpenButton.gameObject.SetActive(true);
            _panel.CloseButton.gameObject.SetActive(false);
        }
        
        private void OpenPanel()
        {
            PositionPanel.DOMoveX(100, 0.2f, true);
            _panel.OpenButton.gameObject.SetActive(false);
            _panel.CloseButton.gameObject.SetActive(true);
        }
    }
}