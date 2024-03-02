using System;
using Abstraction;
using Code.SceneConfigs;
using UserInputSystem;


namespace BattleSystem.MainTower
{
    public class MainTowerController : IOnController, IOnStart
    {
        private MainTowerHandler _mainTower;
        private MainTowerConfig _mainTowerConfig;
        private MainTowerDefenceModel _towerDefenceModel;

        private readonly SceneObjectsHolder _sceneObjects;
        private readonly RayCastController _rayCastController;

        public event Action OnMainTowerDestroyed;

        private bool _isDestroyed;

        public MainTowerController(
            SceneObjectsHolder sceneObjects,
            MainTowerConfig mainTowerConfig,
            RayCastController rayCastController)
        {
            _sceneObjects = sceneObjects;
            _mainTowerConfig = mainTowerConfig;
            _rayCastController = rayCastController;

            _towerDefenceModel = new MainTowerDefenceModel(_mainTowerConfig.DefenceData);
            _mainTower = new MainTowerHandler(_sceneObjects.MainTower, _towerDefenceModel, (int)_mainTowerConfig.Durability);
            _mainTower.OnReachedZeroHealth += mainTowerDestroyed;


            _sceneObjects.MainTowerUI.InitUI();

            Subscribe();
        }

        private void Subscribe()
        {
            _mainTower.OnReachedZeroHealth += mainTowerDestroyed;
            _rayCastController.LeftClick += Select;
            _rayCastController.RightClick += Unselect;
        }


        private void Unsubscribe()
        {
            _mainTower.OnReachedZeroHealth -= mainTowerDestroyed;
            _rayCastController.LeftClick += Select;
            _rayCastController.RightClick -= Unselect;
        }

        private void Select(string id)
        {
            if (id == null)
                return;

            if (_mainTower.Id != id)
            {
                UnselectTower();
                return;
            }

            _mainTower.View.Select();

            _sceneObjects.BattleUI.Hide();
            _sceneObjects.MainTowerUI.Show();
        }

        private void Unselect(string obj) => UnselectTower();

        private void UnselectTower()
        {
            _mainTower.View.Unselect();

            _sceneObjects.BattleUI.Show();
            _sceneObjects.MainTowerUI.Hide();
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
            if (_isDestroyed)
            {
                _mainTower.Enable();

                Subscribe();
            }

            _mainTower.Reset();
        }
    }
}