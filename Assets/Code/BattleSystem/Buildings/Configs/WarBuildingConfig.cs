using UnitSystem.Data;
using UnityEngine;

namespace BattleSystem.Buildings.Configs
{
    [CreateAssetMenu(fileName = nameof(WarBuildingConfig), menuName = "Building/" + nameof(WarBuildingConfig))]
    public class WarBuildingConfig : ScriptableObject
    {
        [field: SerializeField] public uint Durability { get; private set; }
        [field: SerializeField] public UnitDefenceData DefenceData { get; private set; }
    }
}