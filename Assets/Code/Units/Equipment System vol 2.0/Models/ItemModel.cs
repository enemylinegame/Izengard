using ResourceSystem;
using UnityEngine;

namespace EquipmentSystem
{
    [CreateAssetMenu(fileName = "Equipable Item", menuName = "Interactable Item/Equipable Item", order = 1)]
    public class ItemModel :ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Material _material;
        [SerializeField] private GameObject _itemObject;        
        [SerializeField] private CurrentParameters _itemParameters;
        [SerializeField] private GoldCost _costInGold;
        

        protected int _itemID;

        public int ItemID => _itemID;
        public string Name => _name;
        public Sprite Icon => _icon;
        public Material Material => _material;
        public GameObject ItemObject => _itemObject;        
        public CurrentParameters ItemParameters => _itemParameters;
        public GoldCost CostInGold => _costInGold;        


        public ItemModel (ItemModel _baseItemModel)
        {
            _name = _baseItemModel.Name;
            _itemID = _baseItemModel.ItemID;
            _icon = _baseItemModel.Icon;
            _material = _baseItemModel.Material;
            _itemObject = _baseItemModel.ItemObject;
            _itemParameters = new CurrentParameters(_baseItemModel.ItemParameters);
            _costInGold = _baseItemModel.CostInGold;
        }
        public ItemModel(ItemModel _baseItemModel,Transform position)
        {
            _name = _baseItemModel.Name;
            _itemID = _baseItemModel.ItemID;
            _icon = _baseItemModel.Icon;
            _material = _baseItemModel.Material;
            _itemObject = Instantiate(_baseItemModel.ItemObject,position);
            _itemParameters = new CurrentParameters(_baseItemModel.ItemParameters);
            _costInGold = _baseItemModel.CostInGold;
            
        }


    }
}
