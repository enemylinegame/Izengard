using Abstraction;
using BattleSystem.Buildings.Configs;
using BattleSystem.Buildings.Interfaces;
using BattleSystem.Buildings.View;
using BattleSystem.Models;
using UnitSystem.Model;


namespace BattleSystem.Buildings
{
    public class WarBuildingsController : IWarBuildingsContainer, IOnController, IOnStart
    {

        private const int MAIN_TOWER_ID = 12345;
        
        private WarBuildingHandler _mainTower;
        private WarBuildingConfig _mainTowerConfig;
        private UnitDefenceModel _towerDefenceModel;
        private IAttackTarget _mainTowerAsTarget;


        public WarBuildingsController(WarBuildingView mainTowerView, WarBuildingConfig mainTowerConfig)
        {
            _mainTowerConfig = mainTowerConfig;
            _towerDefenceModel = new UnitDefenceModel(_mainTowerConfig.DefenceData);
            _mainTower = new WarBuildingHandler(MAIN_TOWER_ID, mainTowerView, _towerDefenceModel, (int)_mainTowerConfig.Durability);
        }
        
        public void OnStart()
        {
            _mainTower.Enable();
        }


        public IAttackTarget GetMainTowerAsAttackTarget()
        {
            if (_mainTowerAsTarget == null)
            {
                //_mainTowerAsTarget = new TargetModel(_mainTower, _mainTower.Id, _mainTower.View.Position);
                _mainTowerAsTarget = new TargetModel(_mainTower, _mainTower.View);
            }
            return _mainTowerAsTarget;
        }
    }
}