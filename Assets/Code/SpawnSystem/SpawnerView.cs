using System.Collections.Generic;
using UnityEngine;

namespace SpawnSystem
{
    public class SpawnerView : MonoBehaviour
    {
        [SerializeField] private Transform _poolHolder;
        [SerializeField] private SpawnSettings spawnSettings;
        [SerializeField] private List<Transform> spawnPoints;

        public Transform PoolHolder => _poolHolder;
        public SpawnSettings SpawnSettings => spawnSettings;
        public List<Transform> SpawnPoints => spawnPoints; 
    }
}
