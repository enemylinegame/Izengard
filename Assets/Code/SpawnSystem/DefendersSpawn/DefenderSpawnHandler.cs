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

            _battleSceneUIController.OnDefenderSpawn += SpawnPack;

            _battleSceneUIController.OnSpawNewUnit += SpawnUnit;
        }

        private void SpawnUnit(IUnitData unitData)
        {
            _spawnController.SpawnUnit(unitData);
        }

        private void SpawnPack()
        {    
            _spawnController.SpawnUnit(UnitType.Militiaman);  
        }

    }
}
