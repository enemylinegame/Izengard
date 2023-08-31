using UnityEngine;

namespace WaveSystem.View
{
    public class EnemySpawnView :  MonoBehaviour
    {
        [SerializeField] private Transform _poolHolder;

        public Transform PoolHolder => _poolHolder;
    }
}
