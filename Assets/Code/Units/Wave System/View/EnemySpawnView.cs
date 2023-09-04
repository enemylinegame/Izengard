using UnityEngine;

namespace WaveSystem.View
{
    public class EnemySpawnView :  MonoBehaviour
    {
        [SerializeField] private Transform _poolHolder;

        public Transform PoolHolder => _poolHolder;

        private void Awake()
        {
            _poolHolder = transform;
        }

    }
}
