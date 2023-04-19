using UnityEngine;

namespace ResourceSystem
{
    [CreateAssetMenu(fileName = nameof(MineralConfig), menuName = "Resources/Gatherable", order = 1)]
    public class MineralConfig : ScriptableObject
    {
        /*public Mesh MeshModel => _meshModel;
        public Material MaterialModel => _materialModel;*/

        public GameObject Prefab;
        public string NameOfMine => _nameOfMine;
        public float ExtractionTime => _extractionTime;
        public ResourceHolder ResourceHolderMine => resourceHolderMine;
        public Sprite Icon => _icon;
        public float CurrentMineValue => _currentMineValue;
        public TierNumber Tier => thisMineTier;
        public TypeOfMine TypeOfMine => _typeOfMine;

       /* [SerializeField]
        private Mesh _meshModel;
        [SerializeField]
        private Material _materialModel;*/
      [SerializeField] 
       private GameObject _prefab => Prefab;
      [SerializeField]
        private string _nameOfMine;
        [SerializeField]
        private float _extractionTime;
        [SerializeField]
        private ResourceHolder resourceHolderMine;
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
