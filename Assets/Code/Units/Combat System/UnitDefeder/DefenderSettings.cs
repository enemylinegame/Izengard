using System.Collections.Generic;
using ResourceSystem.SupportClases;
using UnityEngine;
using UnityEngine.Serialization;

namespace CombatSystem
{
    [CreateAssetMenu(fileName = nameof(DefenderSettings), menuName = "Defender/" + nameof(DefenderSettings))]
    public class DefenderSettings : ScriptableObject
    {
        [field: SerializeField] public DefenderType Type { get; private set; }
        [field: SerializeField] public DefenderUnitStats UnitStats { get; private set; }
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public GameObject SelectVisualEffectPrefab { get; private set; }
        [field: SerializeField] public List<ResourcePriceModel> BuildingCost;
    }
}