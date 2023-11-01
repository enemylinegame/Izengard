using UnitSystem.Data;
using UnityEngine;

namespace BattleSystem.Buildings.Configs
{
    [CreateAssetMenu(fileName = nameof(MainTowerConfig), menuName = "Building/" + nameof(MainTowerConfig))]
    public class MainTowerConfig : ScriptableObject
    {
        [field: SerializeField] public uint Durability { get; private set; }
        [field: SerializeField] public UnitDefenceData DefenceData { get; private set; }
    }
}