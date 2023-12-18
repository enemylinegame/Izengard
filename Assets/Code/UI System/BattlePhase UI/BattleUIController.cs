using System;
using UnitSystem;

namespace UI
{
    public class BattleUIController 
    {
        private readonly BattleSceneUI _battleSceneUI;
       
        public event Action OnDefenderSpawn;
        public event Action<IUnitData> OnSpawNewUnit;

        public BattleUIController(BattleSceneUI battleSceneUI)
        {
            _battleSceneUI = battleSceneUI;

            _battleSceneUI.OnDefenderSpawn += SpawnDefender;

            _battleSceneUI.UnitSettings.OnSpawn += SpawnUnits;
        }

        private void SpawnDefender()
        {
            OnDefenderSpawn?.Invoke();
        }

        private void SpawnUnits(int quantity)
        {
            var unitCreateData = _battleSceneUI.UnitSettings.Parametrs.GetData();

            for(int i=0; i < quantity; i++)
            {
                OnSpawNewUnit?.Invoke(unitCreateData);
            }
        }
    }
}
