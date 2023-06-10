using CombatSystem.Interfaces;
using UnityEngine;


namespace CombatSystem
{
    public class DefenderTargetSelector
    {
        private readonly DefenderUnitStats _stats;
        private readonly DefenderTargetsHolder _targetHolder;
        private readonly Transform _transform;

        
        public DefenderTargetSelector(GameObject defenderGameObject, DefenderUnitStats stats, 
            DefenderTargetsHolder holder)
        {
            _transform = defenderGameObject.transform;
            _stats = stats;
            _targetHolder = holder;
        }
        
        
        /// <summary>
        /// Select target. Set selected target
        /// to DefenderTargetsHolder.CurrentTarget.
        /// null target - no any target. 
        /// </summary>
        /// <returns> selected target </returns>
        public IDamageable SelectTarget()
        {
            IDamageable selectedTarget = _targetHolder.CurrentTarget;

            if (selectedTarget == null || selectedTarget.IsDead)
            {
                if (selectedTarget != null)
                {
                    selectedTarget = null;
                    _targetHolder.CurrentTarget = null;
                }

                if (_targetHolder.AttackingTargets.Count > 0)
                {
                    selectedTarget = _targetHolder.AttackingTargets[0];
                    _targetHolder.CurrentTarget = selectedTarget;
                }
                else
                {
                    if (_targetHolder.TargetsInRange.Count > 0)
                    {
                        selectedTarget = _targetHolder.TargetsInRange[0];
                        _targetHolder.CurrentTarget = selectedTarget;
                    }
                }
            }
            else
            {
                if ( !_targetHolder.AttackingTargets.Contains(_targetHolder.CurrentTarget))
                {
                    if (_targetHolder.AttackingTargets.Count > 0)
                    {
                        selectedTarget = _targetHolder.AttackingTargets[0];
                        _targetHolder.CurrentTarget = selectedTarget;
                    }
                }
            }

            return selectedTarget;
        }

        public bool IsCurrentTargetInRange()
        {
            return IsTargetInRange(_targetHolder.CurrentTarget);
        }

        public bool IsTargetInRange(IDamageable target)
        {
            bool isInrange = false;

            if (target != null)
            {
                isInrange = _targetHolder.TargetsInRange.Contains(_targetHolder.CurrentTarget);
                // Vector3 position = _transform.position;
                // position.y = 0.0f;
                // Vector3 targetPosition = target.Position;
                // targetPosition.y = 0.0f;
                //
                // isInrange = (targetPosition - position).sqrMagnitude <= _stats.AttackRange * _stats.AttackRange;
            }
            
            return isInrange;
        }
        
    }
}