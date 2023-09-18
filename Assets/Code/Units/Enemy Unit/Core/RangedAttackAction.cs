using System;
using CombatSystem;

namespace EnemyUnit.Core
{
    public class RangedAttackAction : IAction<Damageable>, IOnUpdate
    {
        private readonly EnemyModel _model;
        private readonly EnemyView _view;

        private readonly IBulletsController _bulletsController;
        private float _actionTime;
        private float _timeCounter;
        private Damageable _target;
        private bool _isBulletLaunched;
        private bool _haveTarget;
        
        public event Action<Damageable> OnComplete;

        public RangedAttackAction(
            EnemyModel model, 
            EnemyView view, 
            IBulletsController bulletsController)
        {
            _bulletsController = bulletsController;
            _model = model;
            _view = view;
            _haveTarget = false;
        }

        public void StartAction(Damageable target)
        {
            _actionTime = 1 / _model.Stats.AttackSpeed;
            _timeCounter = _actionTime;
            _isBulletLaunched = false;
            _target = target;
            _haveTarget = true;
        }
        
        public void ClearTarget()
        {
            _target = null;
            _haveTarget = false;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_haveTarget)
            {
                OnComplete?.Invoke(null);
            }
            else if (_timeCounter > 0)
            {
                _timeCounter -= deltaTime;
                if (_timeCounter < _actionTime / 2 && !_isBulletLaunched) LaunchBullet();
                if (_timeCounter <= 0) OnComplete?.Invoke(_target);
            }
        }

        private void LaunchBullet()
        {
            _isBulletLaunched = true;
            var bullet = _bulletsController.BulletsPool.GetObjectFromPool();
            bullet.StartFlight(_target.transform.position, _view.transform.position);
            _bulletsController.AddBullet(bullet);
            bullet.BulletFlightIsOver += BulletFlightOver;
        }

        private void BulletFlightOver(IBulletController bullet)
        {
            _bulletsController.RemoveBullet(bullet);
            bullet.BulletFlightIsOver -= BulletFlightOver;
            if (_target != null)
            {
                _target.MakeDamage(_model.Stats.Attack, _view);
            }
        }
    }
}