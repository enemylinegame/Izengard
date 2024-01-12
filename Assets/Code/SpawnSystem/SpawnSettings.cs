using UnitSystem.Data;
using UnitSystem.Enum;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnSystem
{
    [CreateAssetMenu(fileName = nameof(SpawnSettings), menuName = "Spawn/" + nameof(SpawnSettings))]
    public class SpawnSettings : ScriptableObject
    {
        [field: SerializeField] public float MaxSpawnRadius { get; private set; } = 2f;
        [field: SerializeField] public UnitFactionType SpawnFaction { get; private set; }
        [field: SerializeField] public List<UnitCreationData> UnitsCreationData { get; private set; }
    }
}
