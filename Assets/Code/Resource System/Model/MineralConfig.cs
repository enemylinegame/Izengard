using ResourceSystem.SupportClases;
using UnityEngine;

namespace ResourceSystem
{
    [CreateAssetMenu(fileName = nameof(MineralConfig), menuName = "Resources/Gatherable", order = 1)]
    public class MineralConfig : ScriptableObject
    {
        [field: SerializeField] public GameObject Prefab;
        [field: SerializeField] public string NameOfMine;
        [field: SerializeField] public int ExtractionTime;
        [field: SerializeField] public Sprite Icon;
        [field: SerializeField] public int CurrentMineValue;
        [field: SerializeField] public TierNumber Tier;
        [field: SerializeField] public ResourceType ResourceType;
    }
}