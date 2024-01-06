using System;
using UnityEngine.UI;

namespace UI
{
    public class BuildingInfoPanelController: IDisposable
    {
        private BuildingInfoPanel _panel;

        public event Action PlusButton;
        public event Action MinusButton;
        
        public BuildingInfoPanelController(BuildingInfoPanelFactory factory)
        {
            _panel = factory.GetView(factory.UIElementsConfig.BuildingInfoPanel);
            
            _panel.Plus.onClick.AddListener(() => PlusButton?.Invoke());
            _panel.Minus.onClick.AddListener(() => MinusButton?.Invoke());

            EnabledPanel(false);
        }
        
        public void ChangeUnitsCount(int currentUnit, int MaxUnits) 
            => _panel.UnitsValue.text = $"{currentUnit}/{MaxUnits}";

        public void SetBuildingInfo(string value)
            => _panel.BuildingInfo.text = value;

        public void SetBuildingImage(Image image)
            => _panel.BuildingImage = image;

        public void ActivationErrorPanel(bool isActive)
        {
            _panel.Error.enabled = isActive;
            _panel.ButtonHolder.SetActive(!isActive);
        }

        public void EnabledPanel(bool isActive)
            => _panel.gameObject.SetActive(isActive);

        public void Dispose()
        {
            _panel.Plus.onClick.RemoveAllListeners();
            _panel.Minus.onClick.RemoveAllListeners();
        }
    }
}