using UnityEngine;
using WaveSystem.View;

namespace SpawnSystem
{
    public class EnemySpawnerSettings : ScriptableObject
    {
        [field: SerializeField] public EnemySpawnView SpawnerGO { get; private set; }
        [field: SerializeField] public float SpawnRate { get; private set; } = 1.0f;

    }
}
