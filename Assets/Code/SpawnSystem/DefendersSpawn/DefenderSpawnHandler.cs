using UI;
using UnitSystem.Enum;

namespace SpawnSystem
{
    public class DefenderSpawnHandler
    {
        private readonly ISpawnController _spawnController;
        private readonly BattleSceneUI _battleUI;

        public DefenderSpawnHandler(ISpawnController defendersSpawnController, BattleSceneUI battleUI) 
        {
            _spawnController = defendersSpawnController;

            _battleUI = battleUI;

            _battleUI.OnDefenderSpawnClick += SpawnPack;
        }


        private void SpawnPack()
        {    
            _spawnController.SpawnUnit(UnitType.Militiaman);  
        }

    }
}
