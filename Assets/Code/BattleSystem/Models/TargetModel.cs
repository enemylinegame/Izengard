using Abstraction;
using UnityEngine;

namespace BattleSystem.Models
{
    public class TargetModel : IAttackTarget
    {

        private readonly IDamageable _damageable;
        private readonly ITarget _viewTarget;

        public TargetModel(IDamageable damageable, ITarget target)
        {
            _damageable = damageable;
            _viewTarget = target;
        }

        #region IAttackTarget

        public bool IsAlive => _damageable.IsAlive;

        public void TakeDamage(IDamage damage)
        {
            _damageable.TakeDamage(damage);
        }

        public int Id => _viewTarget.Id;

        public Vector3 Position => _viewTarget.Position;

        #endregion

    }
}