using UnitSystem.Data;
using System.Collections.Generic;
using UnityEngine;
using Abstraction;

namespace SpawnSystem
{
    [CreateAssetMenu(fileName = nameof(SpawnSettings), menuName = "Spawn/" + nameof(SpawnSettings))]
    public class SpawnSettings : ScriptableObject
    {
        [field: SerializeField] public float MaxSpawnRadius { get; private set; } = 2f;
        [field: SerializeField] public FactionType SpawnFaction { get; private set; }
        [field: SerializeField] public List<UnitCreationData> UnitsCreationData { get; private set; }
    }
}
