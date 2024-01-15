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

        private bool _isDestroyed;
          
        public MainTowerController(MainTowerView mainTowerView, MainTowerConfig mainTowerConfig)
        {
            _mainTowerConfig = mainTowerConfig;
            _towerDefenceModel = new MainTowerDefenceModel(_mainTowerConfig.DefenceData);
            _mainTower = new MainTowerHandler(mainTowerView, _towerDefenceModel, (int)_mainTowerConfig.Durability);
            _mainTower.OnReachedZeroHealth += mainTowerDestroyed;

        }

        private void mainTowerDestroyed(IMainTower building)
        {
            _isDestroyed = true;

            building.OnReachedZeroHealth -= mainTowerDestroyed;

            building.Disable();
            OnMainTowerDestroyed?.Invoke();
        }

        public void OnStart()
        {
            _isDestroyed = false;

            _mainTower.Enable();
        }

        public IAttackTarget GetMainTower() => _mainTower.View;
        
        public void Reset()
        {
            if(_isDestroyed)
            {
                _mainTower.Enable();

                _mainTower.OnReachedZeroHealth += mainTowerDestroyed;
            }

            _mainTower.Reset();
        }
    }
}