using UnityEngine;


namespace Wave
{
    [CreateAssetMenu(fileName = nameof(EnemySettings), menuName = "Wave/" + nameof(EnemySettings))]
    public class EnemySettings : ScriptableObject
    {
        [field: SerializeField] public EnemyType Type { get; private set; }
        [field: SerializeField] public EnemyStats Stats { get; private set; }
        [field: SerializeField] public GameObject Prefab { get; private set; }
    }
}