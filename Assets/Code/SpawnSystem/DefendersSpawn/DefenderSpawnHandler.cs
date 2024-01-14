using UI;
using UnitSystem;
using UnitSystem.Enum;

namespace SpawnSystem
{
    public class DefenderSpawnHandler
    {
        private readonly ISpawnController _spawnController;
        private readonly BattleUIController _battleSceneUIController;

        public DefenderSpawnHandler(ISpawnController defendersSpawnController, BattleUIController  battleSceneUIController) 
        {
            _spawnController = defendersSpawnController;

            _battleSceneUIController = battleSceneUIController;

            _battleSceneUIController.OnSpawnNewUnit += SpawnUnit;
        }

        private void SpawnUnit(IUnitData unitData)
        {
            if (unitData.Faction != UnitFactionType.Defender)
                return;

            _spawnController.SpawnUnit(unitData);
        }
    }
}
