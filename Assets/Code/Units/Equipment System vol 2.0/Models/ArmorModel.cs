using UnityEngine;


namespace EquipmentSystem
{
    [CreateAssetMenu(fileName = "Armor Item", menuName = "Interactable Item/Armor Item", order = 1)]
    public class ArmorModel : ItemModel
    {
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        [SerializeField] private ArmorBindingsTypes _armorBindingsType;
        [SerializeField] private ArmorSlotParameter _armorSlotType;
        

        public int ArmorBindingsID => (int)_armorBindingsType;
        public int ArmorSlotTypeID => (int)_armorSlotType.Value;
        public ArmorSlotParameter ArmorSlotType => _armorSlotType;
        public SkinnedMeshRenderer SkinnedMeshRenderer => _skinnedMeshRenderer;

        public ArmorModel(ArmorModel baseModel) : base((ItemModel)baseModel)
        {
            _skinnedMeshRenderer = baseModel.SkinnedMeshRenderer;
            _armorBindingsType = baseModel._armorBindingsType;
            _armorSlotType = new ArmorSlotParameter( baseModel.ArmorSlotType.Value);            
        }
        public ArmorModel(ArmorModel baseModel,Transform position) : base((ItemModel)baseModel,position)
        {
            _skinnedMeshRenderer = ItemObject.GetComponent<SkinnedMeshRenderer>();
            _armorBindingsType = baseModel._armorBindingsType;
            _armorSlotType = new ArmorSlotParameter(baseModel.ArmorSlotType.Value);
            
        }
    }
}
