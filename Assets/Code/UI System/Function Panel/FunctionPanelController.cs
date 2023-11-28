using System;

namespace Code.UI
{
    public class FunctionPanelController : IDisposable
    {
        private readonly FunctionPanel _panel;

        public event Action BuildingButton;
        public event Action UpgradeButton;
        public event Action RepairButton;
        public event Action PlumpButton;

        public FunctionPanelController(FunctionPanelFactory factory)
        {
            _panel = factory.GetView(factory.UIElementsConfig.FunctionPanel);
            
            _panel.Building.onClick.AddListener((() => BuildingButton?.Invoke()));
            _panel.Upgrade.onClick.AddListener((() => UpgradeButton?.Invoke()));
            _panel.Repair.onClick.AddListener((() => RepairButton?.Invoke()));
            _panel.Plump.onClick.AddListener((() => PlumpButton?.Invoke()));
        }

        public void Dispose()
        {
            _panel.Building.onClick.RemoveListener((() => BuildingButton?.Invoke()));
            _panel.Upgrade.onClick.RemoveListener((() => UpgradeButton?.Invoke()));
            _panel.Repair.onClick.RemoveListener((() => RepairButton?.Invoke()));
            _panel.Plump.onClick.RemoveListener((() => PlumpButton?.Invoke()));
        }
    }
}