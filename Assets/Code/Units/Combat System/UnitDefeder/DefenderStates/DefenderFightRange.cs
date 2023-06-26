using System;
using CombatSystem.Interfaces;

namespace CombatSystem.DefenderStates
{
    public class DefenderFightRange : DefenderFight
    {
        private readonly IBulletsController _bulletsController;
        private bool _isBulletLaunched;

        public DefenderFightRange(DefenderUnit defenderUnit, Action<DefenderState> setStateDelegate,
            DefenderUnitStats stats, DefenderTargetsHolder holder, DefenderTargetSelector selector,
            IDamageable myDamageable, DefenderTargetFinder finder, IBulletsController bulletsController) :
            base(defenderUnit,  setStateDelegate, stats,  holder,  selector, myDamageable, finder)
        {
            _bulletsController = bulletsController;
        }

        protected override void AttackTarget(IDamageable target)
        {
            target.MakeDamage(_stats.AttackDamage, _myDamagable);
            LaunchBullet(target);
        }
        
        private void LaunchBullet(IDamageable target)
        {
            _isBulletLaunched = true;
            var bullet = _bulletsController.BulletsPool.GetObjectFromPool();
            bullet.StartFlight(target.Position, _defenderUnit.DefenderGameObject.transform.position);
            _bulletsController.AddBullet(bullet);
            bullet.BulletFlightIsOver += BulletFlightOver;
        }

        private void BulletFlightOver(IBulletController bullet)
        {
            _bulletsController.RemoveBullet(bullet);
            bullet.BulletFlightIsOver -= BulletFlightOver;
            // if (_target != null)
            // {
            //     _target.MakeDamage(_enemy.Stats.Attack, _enemy.MyDamagable);
            // }
        }
        
    }
}