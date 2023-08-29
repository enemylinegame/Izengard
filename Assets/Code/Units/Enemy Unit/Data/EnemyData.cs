using UnityEngine;
using Wave;

namespace EnemyUnit
{
    public class EnemyData : ScriptableObject
    {
        [field: SerializeField] public EnemyType Type { get; private set; }
        [field: SerializeField] public EnemyStats Stats { get; private set; }
        [field: SerializeField] public GameObject Prefab { get; private set; }
    }
}
