using Abstraction;
using UnityEngine;

namespace BattleSystem.Models
{
    public class TargetModel : IAttackTarget
    {
        
        private enum MobilityType
        {
            Static,
            Dynamic
        }

        private readonly IDamageable _damageable;
        private readonly ITarget _viewTarget;
        private readonly Vector3 _position;
        private readonly int _id;
        private readonly MobilityType _mobilityType;
        

        public TargetModel(IDamageable damageable, ITarget target)
        {
            _damageable = damageable;
            _viewTarget = target;
            _id = _viewTarget.Id;
            _mobilityType = MobilityType.Dynamic;
        }

        public TargetModel(IDamageable damageable, int id, Vector3 position)
        {
            _damageable = damageable;
            _id = id;
            _position = position;
            _mobilityType = MobilityType.Static;
        }

        #region IAttackTarget

        public bool IsAlive => _damageable.IsAlive;

        public void TakeDamage(IDamage damage)
        {
            _damageable.TakeDamage(damage);
        }

        public int Id => _id;

        public Vector3 Position
        {
            get
            {
                if (_mobilityType == MobilityType.Dynamic)
                {
                    return _viewTarget.Position;
                }
                else
                {
                    return _position;
                }
            }
        }

        #endregion

    }
}