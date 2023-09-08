using System;
using UnityEngine.AI;

namespace EnemyUnit.Core
{
    public class PlanRouteAction : IAction<Damageable>
    {
        public event Action<Damageable> OnComplete;
        private readonly NavMeshAgent _navMeshAgent;

        public PlanRouteAction(EnemyView view)
        {
            _navMeshAgent = view.NavMesh;
        }

        public void StartAction(Damageable target)
        {
            if (target != null)
            {
                if (_navMeshAgent.gameObject.activeSelf && _navMeshAgent.isOnNavMesh)
                    _navMeshAgent.SetDestination(target.transform.position);
            }
            OnComplete?.Invoke(target);
        }

        public void ClearTarget()
        {
        }
    }
}
