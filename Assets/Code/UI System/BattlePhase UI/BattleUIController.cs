using System;
using UnitSystem;

namespace UI
{
    public class BattleUIController :  IOnController, IDisposable
    {
        private readonly BattleSceneUI _battleSceneUI;

        public event Action OnStartWave;
        public event Action OnStopWave;
        public event Action OnDefenderSpawn;

        public event Action<IUnitData> OnSpawnNewUnit;

        public BattleUIController(BattleSceneUI battleSceneUI)
        {
            _battleSceneUI = battleSceneUI;

            _battleSceneUI.WaveStartButton.onClick.AddListener(() => OnStartWave?.Invoke());
            _battleSceneUI.WaveStopButton.onClick.AddListener(() => OnStopWave?.Invoke());
            _battleSceneUI.DefenderSpawnButton.onClick.AddListener(() => OnDefenderSpawn?.Invoke());

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
            _battleSceneUI.WaveStartButton.onClick.RemoveAllListeners();
            _battleSceneUI.WaveStopButton.onClick.RemoveAllListeners();
            _battleSceneUI.DefenderSpawnButton.onClick.RemoveAllListeners();
        }
    }
}
