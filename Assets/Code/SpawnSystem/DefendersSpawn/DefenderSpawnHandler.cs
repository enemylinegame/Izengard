using Tools;
using UnitSystem.Enum;
using UnityEngine.UI;

namespace SpawnSystem
{
    public class DefenderSpawnHandler
    {
        private readonly ISpawnController _spawnController;
        private readonly Button _spawnButton;

        public DefenderSpawnHandler(ISpawnController defendersSpawnController, Button spawnButton) 
        {
            _spawnController = defendersSpawnController;

            _spawnButton = spawnButton;

            _spawnButton.onClick.AddListener(() => SpawnPack());
        }


        private void SpawnPack()
        {    
            _spawnController.SpawnUnit(UnitType.Militiaman);  
        }

    }
}
