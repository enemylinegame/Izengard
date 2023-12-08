using System;
using Abstraction;


namespace BattleSystem.MainTower
{
    public class MainTowerController : IOnController, IOnStart
    {     
        private MainTowerHandler _mainTower;
        private MainTowerConfig _mainTowerConfig;
        private MainTowerDefenceModel _towerDefenceModel;

        public event Action OnMainTowerDestroyed; 
            
          
        public MainTowerController(MainTowerView mainTowerView, MainTowerConfig mainTowerConfig)
        {
            _mainTowerConfig = mainTowerConfig;
            _towerDefenceModel = new MainTowerDefenceModel(_mainTowerConfig.DefenceData);
            _mainTower = new MainTowerHandler(mainTowerView, _towerDefenceModel, (int)_mainTowerConfig.Durability);
            _mainTower.OnReachedZeroHealth += mainTowerDestroyed;
        }
        
        public void OnStart()
        {
            _mainTower.Enable();
        }

        public IAttackTarget GetMainTower() => _mainTower.View;


        private void mainTowerDestroyed(IMainTower building)
        {
            building.OnReachedZeroHealth -= mainTowerDestroyed;

            building.Disable();
            OnMainTowerDestroyed?.Invoke();
        }
        
    }
}