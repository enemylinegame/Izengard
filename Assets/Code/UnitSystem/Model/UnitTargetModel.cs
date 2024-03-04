using Abstraction;
using System;

namespace UnitSystem.Model
{
    public class UnitTargetModel
    {
        private readonly IAttackTarget _default = new NoneTarget();

        private IAttackTarget _currentTarget;

        public IAttackTarget CurrentTarget => _currentTarget;

        public event Action<IAttackTarget> OnTargetChange;

        public UnitTargetModel() 
        {
            _currentTarget = _default;
        }

        public void SetTarget(IAttackTarget target)
        { 
            _currentTarget = target;

            OnTargetChange?.Invoke(target);
        }

        public void ResetTarget()
        {
            _currentTarget = _default;
        }
    }
}
