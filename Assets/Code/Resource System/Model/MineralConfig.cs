using ResourceSystem.SupportClases;
using UnityEngine;

namespace ResourceSystem
{
    [CreateAssetMenu(fileName = nameof(MineralConfig), menuName = "Resources/Gatherable", order = 1)]
    public class MineralConfig : ScriptableObject
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private string _nameOfMine;
        [SerializeField] private int _extractionTime;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _currentMineValue;
        [SerializeField] private TierNumber _thisMineTier;
        [SerializeField] private ResourceType _resourceType;
        public GameObject Prefab => _prefab;
        public string NameOfMine => _nameOfMine;
        public int ExtractionTime => _extractionTime;
        public Sprite Icon => _icon;
        public int CurrentMineValue
        {
            get { return _currentMineValue; }
            set { _currentMineValue = value; }
        }
        public TierNumber Tier => _thisMineTier;
        public ResourceType ResourceType => _resourceType;

    }
}