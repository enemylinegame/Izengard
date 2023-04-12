using UnityEngine;

namespace ResourceSystem
{
    [CreateAssetMenu(fileName = nameof(MineralConfig), menuName = "Resources/Gatherable", order = 1)]
    public class MineralConfig : ScriptableObject
    {
        public GameObject Prefab =>_prefab;
        public string NameOfMine => _nameOfMine;
        public float ExtractionTime => _extractionTime;
        public ResourceHolder ResourceHolderMine => _resourceHolderMine;
        public Sprite Icon => _icon;
        public float CurrentMineValue => _currentMineValue;
        public TierNumber Tier => thisMineTier;
        public TypeOfMine TypeOfMine => _typeOfMine;

        [SerializeField] 
        private GameObject _prefab;
      [SerializeField]
        private string _nameOfMine;
        [SerializeField]
        private float _extractionTime;
        [SerializeField]
        private ResourceHolder _resourceHolderMine;
        [SerializeField]
        private Sprite _icon;
        [SerializeField]
        private float _currentMineValue;
        [SerializeField]
        private TierNumber thisMineTier;
        [SerializeField]
        private TypeOfMine _typeOfMine;
    }

}
