using Abstraction;
using UnityEngine;

namespace SpawnSystem
{
    public class SpawnerView : BaseGameObject
    {
        [SerializeField] private Transform _poolHolder;
        [SerializeField] private SpawnSettings _spawnSettings;
        [SerializeField] private Transform _spawnersContainer;

        public Transform PoolHolder => _poolHolder;
        public SpawnSettings SpawnSettings => _spawnSettings;
        public Transform SpawnersContainer => _spawnersContainer;
    }
}
