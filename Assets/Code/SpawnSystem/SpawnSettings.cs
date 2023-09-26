using Izengard.UnitSystem.Data;
using Izengard.UnitSystem.Enum;
using System.Collections.Generic;
using UnityEngine;

namespace Izengard.SpawnSystem
{
    [CreateAssetMenu(fileName = nameof(SpawnSettings), menuName = "Spawn/" + nameof(SpawnSettings))]
    public class SpawnSettings : ScriptableObject
    {
        [field: SerializeField] public UnitFactionType SpawnFaction { get; private set; }
        [field: SerializeField] public int UnitPoolCopacity { get; private set; } = 5;
        [field: SerializeField] public float SpawnDelayTime { get; private set; } = 1.5f;
        [field: SerializeField] public List<UnitCreationData> UnitsCreationData { get; private set; }
    }
}
