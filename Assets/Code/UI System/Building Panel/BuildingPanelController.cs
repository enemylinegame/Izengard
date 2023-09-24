using DG.Tweening;
using UnityEngine;

namespace Code.UI
{
    public class BuildingPanelController
    {
        private readonly BuildingPanel _panel;
        
        private Transform PositionPanel => _panel.PositionPanel;

        public BuildingPanelController(BuildingPanelFactory factory)
        {
            _panel = factory.GetView(factory.UIElementsConfig.BuildingPanel);
            
            _panel.CloseButton.onClick.AddListener(ClosePanel);
            _panel.OpenButton.onClick.AddListener(OpenPanel);
        }
        
        private void ClosePanel()
        {
            PositionPanel.DOMoveY(-50, 0.2f, true);
            _panel.OpenButton.gameObject.SetActive(true);
            _panel.CloseButton.gameObject.SetActive(false);
        }
        
        private void OpenPanel()
        {
            PositionPanel.DOMoveY(90, 0.2f, true);
            _panel.OpenButton.gameObject.SetActive(false);
            _panel.CloseButton.gameObject.SetActive(true);
        }
    }
}