using System.Collections.Generic;
using UnityEngine;

namespace Izengard.SpawnSystem
{
    [CreateAssetMenu(fileName = nameof(SpawnSettings), menuName = "Spawn/" + nameof(SpawnSettings))]
    public class SpawnSettings : ScriptableObject
    {
        [field: SerializeField] public float SpawnDelayTime { get; private set; } = 1.5f;
        [field: SerializeField] public List<SpawnUnitInfo> SpawnUnits { get; private set; }
    }
}
