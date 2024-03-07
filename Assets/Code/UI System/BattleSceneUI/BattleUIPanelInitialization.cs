using AudioSystem;
using UnityEngine;

namespace UI
{
    public class BattleUIPanelInitialization
    {
        public readonly BattleUIController BattleUIController;

        public BattleUIPanelInitialization(
            UIElementsConfig config, 
            Canvas canvas,
            AudioController audioController)
        {
            var battlePanelFactory = new BattlePanelFactory(config, canvas);

            BattleUIController = new BattleUIController(battlePanelFactory, audioController);
        }
    }
}
