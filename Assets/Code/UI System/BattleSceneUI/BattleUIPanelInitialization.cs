using UnityEngine;

namespace UI
{
    public class BattleUIPanelInitialization
    {
        public readonly BattleUIController BattleUIController;

        public BattleUIPanelInitialization(UIElementsConfig config, Canvas canvas)
        {
            var battlePanelFactory = new BattlePanelFactory(config, canvas);

            BattleUIController = new BattleUIController(battlePanelFactory);
        }
    }
}
