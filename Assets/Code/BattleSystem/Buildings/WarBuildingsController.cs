using Abstraction;
using BattleSystem.Buildings.Configs;
using BattleSystem.Buildings.Interfaces;
using BattleSystem.Buildings.View;
using BattleSystem.Models;
using UnitSystem.Model;

namespace BattleSystem.Buildings
{
    public class WarBuildingsController : IWarBuildingsContainer//, IOnController, IOnStart
    {

        private WarBuildingHandler _mainTower;
        private MainTowerConfig _config;
        private UnitDefenceModel _towerDefenceModel;
        private IAttackTarget _mainTowerAsTarget;


        public WarBuildingsController(WarBuildingView mainTower, MainTowerConfig config)
        {
            _config = config;
            _towerDefenceModel = new UnitDefenceModel(_config.DefenceData);
            _mainTower = new WarBuildingHandler(12345, mainTower, _towerDefenceModel, (int)_config.Durability);
        }
        
        public void OnStart()
        {
            
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