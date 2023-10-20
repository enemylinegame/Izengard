using Abstraction;
using System;
using UnityEngine;

namespace UnitSystem.Model
{
    public class UnitTargetModel
    {
        private readonly ITarget _default = new NoneTarget();

        private ITarget _currentTarget;
        private Vector3 _prevTargetPosition;

        public ITarget CurrentTarget => _currentTarget;

        public Action<ITarget> OnTargetChange;

        public UnitTargetModel() 
        {
            _currentTarget = _default;
        }

        public void SetTarget(ITarget target)
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
            bool checkResult = false;
            
            if(_currentTarget.Position != _prevTargetPosition)
            {
                checkResult = true;
            }
            
            _prevTargetPosition = _currentTarget.Position;

            return checkResult;
        }
    }
}
