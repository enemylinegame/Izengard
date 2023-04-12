using Controllers.Pool;
using UnityEngine;

namespace EquipmentSystem
{
    [CreateAssetMenu(fileName = "Weapon Item", menuName = "Interactable Item/Weapon Item", order = 1)]
    public class WeaponModel : ItemModel
    {
        [SerializeField]private WeaponTypeParameter _weaponType;
        [SerializeField] private WeaponGripParameter _weaponGripType;
        [SerializeField] private GameObject _accessory;
        [SerializeField] private GameObject _backSlotPrefab;

        public GameObject Accessory => _accessory;
        public GameObject BackSlotPrefab => _backSlotPrefab;
        public int WeaponGripTypeID => (int)_weaponGripType.Value;
        public WeaponTypeParameter WeaponType => _weaponType;
        public WeaponGripParameter WeaponGripType => _weaponGripType;

        public WeaponModel (WeaponModel baseModel): base((ItemModel)baseModel)
        {
            _weaponType = baseModel.WeaponType;
            _weaponGripType = baseModel.WeaponGripType;
            _accessory = baseModel.Accessory;
            _backSlotPrefab = baseModel.BackSlotPrefab;
        }
        public WeaponModel(WeaponModel baseModel,Transform parent) : base((ItemModel)baseModel,parent)
        {
            _weaponType = baseModel.WeaponType;
            _weaponGripType = baseModel.WeaponGripType;
            if (baseModel.Accessory!=null)
            { 
                _accessory = Instantiate(baseModel.Accessory);
            }
            if (baseModel.BackSlotPrefab != null)
            {
                _backSlotPrefab = Instantiate(baseModel.BackSlotPrefab);
            }
            
        }

        public WeaponModel(WeaponModel baseModel, Transform parent, ItemsPoolController pool) : base((ItemModel)baseModel, parent)
        {
            _weaponType = baseModel.WeaponType;
            _weaponGripType = baseModel.WeaponGripType;
            if (baseModel.Accessory != null)
            {
                _accessory = pool.GetObjectFromPool(baseModel);
            }
            if (baseModel.BackSlotPrefab != null)
            {
                _backSlotPrefab = pool.GetObjectFromPool(baseModel);
            }

        }
    }
}
