using Controllers.Pool;
using System;
using UnityEngine;

namespace EquipmentSystem
{
    [Serializable]
    public class WeaponSlot : EquipmentSlot<WeaponModel>
    {
        [SerializeField] private Transform _accessoryHolderTransform;        
        [SerializeField] protected WeaponSlotTypes _weaponSlotType;
        [SerializeField] private WeaponGripTypes _weaponGripSlot;
        [SerializeField] private Transform _shieldHolderTransform;

        public WeaponSlot SecondSlot;

        private static ItemsPoolController _itemsPoolController;


        public override void EquipItem(WeaponModel newItemModel)
        {
            _itemsPoolController ??= new ItemsPoolController();
            _itemModel = new WeaponModel (newItemModel,HolderTransform, _itemsPoolController);
            if (newItemModel.WeaponType.Value == WeaponTypes.Shield)
            {
                _itemModel.ItemObject.transform.SetParent(_shieldHolderTransform);
                _itemModel.ItemObject.transform.localPosition = Vector3.zero;
                _itemModel.ItemObject.transform.localRotation = Quaternion.identity;
            }
            else
            {
                _itemModel.ItemObject.transform.SetParent(_holderTransform);
                _itemModel.ItemObject.transform.localPosition = Vector3.zero;
                _itemModel.ItemObject.transform.localRotation = Quaternion.identity;
            }


            if (_itemModel.Accessory != null)
            {
                    
                _itemModel.Accessory.transform.SetParent(_accessoryHolderTransform);
                _itemModel.Accessory.transform.localPosition = Vector3.zero;
                _itemModel.Accessory.transform.localRotation = Quaternion.identity;
            }
            
        }

        public override WeaponModel UnequipItem()
        {
            if (!(_itemModel is null))
            {
                _itemModel.ItemObject.SetActive(false);
                if (_itemModel.Accessory != null)
                {
                    _itemModel.Accessory.SetActive(false);
                }
                
            }
            var previousModel = _itemModel;
            _itemModel = null;

            return previousModel;
        }
    }
}
