using Abstraction;
using System;
using UnityEngine;

namespace UnitSystem.Model
{
    public class UnitTargetModel
    {
        private readonly IAttackTarget _default = new NoneTarget();

        private IAttackTarget _currentTarget;
        private Vector3 _prevTargetPosition;

        public IAttackTarget CurrentTarget => _currentTarget;

        public event Action<IAttackTarget> OnTargetChange;

        public UnitTargetModel() 
        {
            _currentTarget = _default;
        }

        public void SetTarget(IAttackTarget target)
        { 
            _currentTarget = target;

            _prevTargetPosition = _currentTarget.Position;
            
            OnTargetChange?.Invoke(target);
        }

        public void ResetTarget()
        {
            _currentTarget = _default;
        }

        public bool IsTargetChangePosition()
        {
            bool checkResult = _currentTarget.Position != _prevTargetPosition;

            _prevTargetPosition = _currentTarget.Position;

            return checkResult;
        }
    }
}
