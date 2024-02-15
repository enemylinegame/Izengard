using System;
using Abstraction;
using UserInputSystem;


namespace BattleSystem.MainTower
{
    public class MainTowerController : IOnController, IOnStart
    {     
        private MainTowerHandler _mainTower;
        private MainTowerConfig _mainTowerConfig;
        private MainTowerDefenceModel _towerDefenceModel;

        private readonly RayCastController _rayCastController;
        public event Action OnMainTowerDestroyed;

        private bool _isDestroyed;
          
        public MainTowerController(
            MainTowerView mainTowerView, 
            MainTowerConfig mainTowerConfig,
            RayCastController rayCastController)
        {
            _mainTowerConfig = mainTowerConfig;
            _towerDefenceModel = new MainTowerDefenceModel(_mainTowerConfig.DefenceData);
            _mainTower = new MainTowerHandler(mainTowerView, _towerDefenceModel, (int)_mainTowerConfig.Durability);
            _mainTower.OnReachedZeroHealth += mainTowerDestroyed;

            _rayCastController = rayCastController;

            Subscribe();
        }

        private void Subscribe()
        {
            _mainTower.OnReachedZeroHealth += mainTowerDestroyed;
            _rayCastController.RightClick += SelectTower;
            _rayCastController.LeftClick += UnselectTower;
        }


        private void Unsubscribe()
        {
            _mainTower.OnReachedZeroHealth -= mainTowerDestroyed;
            _rayCastController.RightClick -= SelectTower;
        }

        private void SelectTower(string id)
        {
            if (id == null)
                return;

            if (_mainTower.Id != id)
                return;

            _mainTower.View.Select();
        }

        private void UnselectTower(string obj)
        {
            _mainTower.View.Unselect();
        }

        private void mainTowerDestroyed(IMainTower building)
        {
            _isDestroyed = true;

            Unsubscribe();

            _mainTower.Disable();
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

                Subscribe();
            }

            _mainTower.Reset();
        }
    }
}