using System;
using Abstraction;
using UnitSystem.Model;


namespace BattleSystem.MainTower
{
    public class MainTowerController : IOnController, IOnStart
    {     
        private MainTowerHandler _mainTower;
        private MainTowerConfig _mainTowerConfig;
        private UnitDefenceModel _towerDefenceModel;
        private IAttackTarget _mainTowerAsTarget;

        public event Action OnMainTowerDestroyed; 
            
           
        public MainTowerController(MainTowerView mainTowerView, MainTowerConfig mainTowerConfig)
        {
            _mainTowerConfig = mainTowerConfig;
            _towerDefenceModel = new UnitDefenceModel(_mainTowerConfig.DefenceData);
            _mainTower = new MainTowerHandler(mainTowerView, _towerDefenceModel, (int)_mainTowerConfig.Durability);
            _mainTower.OnReachedZeroHealth += BuildingDestroyed;
        }
        
        public void OnStart()
        {
            _mainTower.Enable();
        }


        public IAttackTarget GetMainTower() => _mainTower.View;


        private void BuildingDestroyed(IMainTower building)
        {
            building.OnReachedZeroHealth -= BuildingDestroyed;

            if (building.Id == _mainTower.Id)
            {
                OnMainTowerDestroyed?.Invoke();
            }
            
        }
        
    }
}