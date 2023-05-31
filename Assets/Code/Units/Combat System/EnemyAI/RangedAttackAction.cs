using System;
using Wave;


namespace CombatSystem
{
    public class RangedAttackAction : IAction<Damageable>, IOnUpdate
    {
        public event Action<Damageable> OnComplete;
        private readonly IBulletsController _bulletsController;
        private readonly Enemy _enemy;
        private float _actionTime;
        private Damageable _target;
        private bool _isBulletLaunched;
        private bool _haveTarget;

        public RangedAttackAction(IBulletsController bulletsController, Enemy enemy)
        {
            _bulletsController = bulletsController;
            _enemy = enemy;
            _haveTarget = false;
        }

        public void StartAction(Damageable target)
        {
            _actionTime = 1 / _enemy.Stats.AttackSpeed;
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
            else if (_actionTime > 0)
            {
                _actionTime -= deltaTime;
                if (_actionTime < _actionTime / 2 && !_isBulletLaunched) LaunchBullet();
                if (_actionTime < 0) OnComplete?.Invoke(_target);
            }
        }

        private void LaunchBullet()
        {
            _isBulletLaunched = true;
            var bullet = _bulletsController.BulletsPool.GetObjectFromPool();
            bullet.StartFlight(_target.transform.position, _enemy.RootGameObject.transform.position);
            _bulletsController.AddBullet(bullet);
            bullet.BulletFlightIsOver += BulletFlightOver;
        }

        private void BulletFlightOver(IBulletController bullet)
        {
            _bulletsController.RemoveBullet(bullet);
            bullet.BulletFlightIsOver -= BulletFlightOver;
            if (_target != null) _target.MakeDamage(_enemy.Stats.Attack);
        }
    }
}