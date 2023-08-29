using UnityEngine;
using UnityEngine.AI;

namespace EnemyUnit
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMesh;
        [SerializeField] private Animator _animator;

        public NavMeshAgent NavMesh => _navMesh;
        public Animator Animator => _animator;
    }
}
