using Izengard.Abstraction.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Izengard.EnemySystem
{
    public class EnemyNavigationModel : INavigation<Vector3>
    {
        private readonly NavMeshAgent _navMesh;
        private readonly Vector3 _initPos;
        public EnemyNavigationModel(NavMeshAgent navMesh, Vector3 initPos)
        {
            _navMesh = navMesh;
            _initPos = initPos;
        }

        public void Enable()
        {
            if (_navMesh == null)
                return;

            _navMesh.enabled = true;

            if (_navMesh.isOnNavMesh)
                _navMesh.ResetPath();

            _navMesh.Warp(_initPos);
        }

        public void Disable()
        {
            _navMesh.ResetPath();
            _navMesh.enabled = false;
        }

        public void MoveTo(Vector3 position)
        {
            if (_navMesh.isOnNavMesh)
            {
                _navMesh.SetDestination(position);
            }
        }

        public void Stop()
        {
            if (_navMesh.isOnNavMesh)
            {
                _navMesh.ResetPath();
            }
        }
    }
}
