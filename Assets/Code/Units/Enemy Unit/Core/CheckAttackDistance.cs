using System;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyUnit.Core
{
    public class CheckAttackDistance : IAction<Damageable>
    { 
        private readonly EnemyModel _model;
        private readonly EnemyView _view;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly float _attackMaxDistance;

        public event Action<Damageable> OnComplete;

        public CheckAttackDistance(EnemyModel model, EnemyView view, float attackMaxDistance)
        {
            _model = model;
            _view = view;
            _navMeshAgent = _view.NavMesh;
            _attackMaxDistance = attackMaxDistance;
        }

        public void StartAction(Damageable target)
        {
            OnComplete?.Invoke(CheckDistance(target));
        }
        
        public void ClearTarget()
        {
            
        }

        private Damageable CheckDistance(Damageable target)
        {
            if (target == null)
            {
                return null;
            }
            var unitPosition = _view.transform.position;
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