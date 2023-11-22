using UnityEngine;
using UnityEngine.AI;

namespace UnitSystem.Model
{
    public class UnitNavigationModel 
    {
        private readonly NavMeshAgent _navMesh;
        private readonly Vector3 _initPos;

        public UnitNavigationModel(NavMeshAgent navMesh, Vector3 initPos)
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

            //_navMesh.Warp(_initPos);
        }

        public void Disable()
        {
            if (_navMesh.isOnNavMesh)
            {
                _navMesh.ResetPath();
            }
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

        public bool CheckForPathComplete()
        {
            if (!_navMesh.isOnNavMesh)
            {
                return false;
            }

            return _navMesh.pathStatus == NavMeshPathStatus.PathComplete;
        }
    }
}
