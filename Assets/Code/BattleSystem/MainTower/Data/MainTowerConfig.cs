using UnityEngine;

namespace BattleSystem.MainTower
{
    [CreateAssetMenu(fileName = nameof(MainTowerConfig), menuName = "Building/" + nameof(MainTowerConfig))]
    public class MainTowerConfig : ScriptableObject
    {
        [field: SerializeField] public uint Durability { get; private set; }
        [field: SerializeField] public ToweDefenceData DefenceData { get; private set; }
    }
}