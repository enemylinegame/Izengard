using System;
using Abstraction;
using BattleSystem.Buildings.Configs;
using BattleSystem.Buildings.Interfaces;
using BattleSystem.Buildings.View;
using UnitSystem.Model;


namespace BattleSystem.Buildings
{
    public class WarBuildingsController : IWarBuildingsContainer, IOnController, IOnStart
    {     
        private WarBuildingHandler _mainTower;
        private WarBuildingConfig _mainTowerConfig;
        private UnitDefenceModel _towerDefenceModel;
        private IAttackTarget _mainTowerAsTarget;

        public event Action OnMainTowerDestroyed; 
            
            

        public WarBuildingsController(WarBuildingView mainTowerView, WarBuildingConfig mainTowerConfig)
        {
            _mainTowerConfig = mainTowerConfig;
            _towerDefenceModel = new UnitDefenceModel(_mainTowerConfig.DefenceData);
            _mainTower = new WarBuildingHandler(mainTowerView, _towerDefenceModel, (int)_mainTowerConfig.Durability);
            _mainTower.OnReachedZeroHealth += BuildingDestroyed;
        }
        
        public void OnStart()
        {
            _mainTower.Enable();
        }


        public IAttackTarget GetMainTower() => _mainTower.View;


        private void BuildingDestroyed(IWarBuilding building)
        {
            building.OnReachedZeroHealth -= BuildingDestroyed;

            if (building.Id == _mainTower.Id)
            {
                OnMainTowerDestroyed?.Invoke();
            }
            
        }
        
    }
}