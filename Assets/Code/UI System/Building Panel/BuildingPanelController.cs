using System.Collections.Generic;
using DG.Tweening;
using NewBuildingSystem;
using UnityEngine;
using UnityEngine.Events;

namespace UI
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

        public void NewBuilds(ObjectData data, UnityAction action)
        {
            var hud = Object.Instantiate(_panel.ObjectHUD.GetComponent<BuildingHUD>(), _panel.GridPanel);
            hud.Image.sprite = data.Image;
            hud.spawn.onClick.AddListener(action);
        }
        
        private void ClosePanel()
        {
            PositionPanel.DOMoveY(-70, 0.2f, true);
            _panel.OpenButton.gameObject.SetActive(true);
            _panel.CloseButton.gameObject.SetActive(false);
        }
        
        private void OpenPanel()
        {
            PositionPanel.DOMoveY(110, 0.2f, true);
            _panel.OpenButton.gameObject.SetActive(false);
            _panel.CloseButton.gameObject.SetActive(true);
        }
    }
}