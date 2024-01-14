using System;
using UnitSystem;

namespace UI
{
    public class BattleUIController :  IOnController, IDisposable
    {
        private readonly BattleSceneUI _battleSceneUI;

        public event Action OnStartBattle;
        public event Action OnPauseBattle;
        public event Action OnResetBattle;

        public event Action<IUnitData> OnSpawnNewUnit;

        public BattleUIController(BattleSceneUI battleSceneUI)
        {
            _battleSceneUI = battleSceneUI;

            _battleSceneUI.StartButton.onClick.AddListener(() => OnStartBattle?.Invoke());
            _battleSceneUI.PauseButton.onClick.AddListener(() => OnPauseBattle?.Invoke());
            _battleSceneUI.ResetButton.onClick.AddListener(() => OnResetBattle?.Invoke());

            _battleSceneUI.UnitSettings.OnSpawn += SpawnUnits;
        }

        private void SpawnUnits(int quantity)
        {
            var unitCreateData = _battleSceneUI.UnitSettings.Parametrs.GetData();

            for(int i=0; i < quantity; i++)
            {
                OnSpawnNewUnit?.Invoke(unitCreateData);
            }
        }

        public void Dispose()
        {
            _battleSceneUI.StartButton.onClick.RemoveAllListeners();
            _battleSceneUI.PauseButton.onClick.RemoveAllListeners();
            _battleSceneUI.ResetButton.onClick.RemoveAllListeners();
        }
    }
}
