using System;
using UnityEngine;
using UnityEngine.AI;
using Wave;


namespace CombatSystem
{
    public class CheckAttackDistance : IAction<Damageable>
    {
        private readonly float _attackMaxDistance;
        public event Action<Damageable> OnComplete;
        private readonly Enemy _unit;
        private readonly NavMeshAgent _navMeshAgent;


        public CheckAttackDistance(Enemy unit, NavMeshAgent navMeshAgent, float attackMaxDistance)
        {
            _unit = unit;
            _navMeshAgent = navMeshAgent;
            _attackMaxDistance = attackMaxDistance;
        }

        public void StartAction(Damageable target)
        {
            OnComplete?.Invoke(CheckDistance(target));
        }

        private Damageable CheckDistance(Damageable target)
        {
            if (target == null)
            {
                return null;
            }
            var unitPosition = _unit.Prefab.transform.position;
            var targetPosition = target.transform.position;
            var distance = Vector3.Distance(targetPosition, unitPosition);
            if (distance < _attackMaxDistance)
            {
                if (_navMeshAgent.gameObject.activeInHierarchy && _navMeshAgent.isOnNavMesh)
                {
                    _navMeshAgent.ResetPath();
                    return target;
                }
            }
            return null;
        }
    }
}