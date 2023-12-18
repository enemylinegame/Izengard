using UnitSystem;
using UnitSystem.Model;

namespace UI
{
    public class BattleSceneUIController
    {
        private readonly UnitSettingsPanel _settingsPanel;

        public BattleSceneUIController(BattleSceneUI battleSceneUI)
        {
            _settingsPanel = battleSceneUI.UnitSettings;
        }

        public IUnitData GetUnitCrationData()
        {
            var unitParams = _settingsPanel.Parametrs;

            var dataModel = new UnitDataModel
            {

            };

            return null;
        }

    }
}
