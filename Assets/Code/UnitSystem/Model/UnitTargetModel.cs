using Abstraction;
using System;

namespace UnitSystem.Model
{
    public class UnitTargetModel
    {
        private ITarget _currentTarget;
        public ITarget CurrentTarget => _currentTarget;

        public Action<ITarget> OnTargetChange;

        public UnitTargetModel() { }

        public void SetTarget(ITarget target)
        { 
            _currentTarget = target;
            OnTargetChange?.Invoke(target);
        }
    }
}
