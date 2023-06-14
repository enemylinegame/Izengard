using UnityEngine;


namespace Wave.Settings
{
    [CreateAssetMenu(fileName = nameof(EnemySpawnSettings), menuName = "Wave/" + nameof(EnemySpawnSettings))]
    public class EnemySpawnSettings : ScriptableObject
    {
        [field: SerializeField] public float PauseBetweenSpawn { get; private set; } = 1.0f;
    }
}