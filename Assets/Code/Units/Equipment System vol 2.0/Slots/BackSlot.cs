using System;
using UnityEngine;

namespace EquipmentSystem
{
    [Serializable]
    public class BackSlot : EquipmentSlot<WeaponModel>
    {
        [SerializeField] private Transform _accessoryHolderTransform;
        [SerializeField] private WeaponSlotTypes _weaponSlotType;

        public WeaponSlotTypes WeaponSlotType => _weaponSlotType;

        public override void EquipItem(WeaponModel newItemModel)
        {
            _itemModel = newItemModel;

            if (_itemModel.Accessory != null)
            {
                _itemModel.Accessory.transform.position = _accessoryHolderTransform.position;
                _itemModel.Accessory.transform.SetParent(_accessoryHolderTransform);
                _itemModel.Accessory.transform.localPosition = Vector3.zero;
                _itemModel.Accessory.transform.localRotation = Quaternion.identity;
                _itemModel.Accessory.SetActive(true);
            }

            _itemModel.BackSlotPrefab.SetActive(true);
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

        public void SetBackPrefab(GameObject prefab)
        {            
            prefab.transform.position = _holderTransform.position;
            prefab.transform.SetParent(_holderTransform);
            prefab.transform.localPosition = Vector3.zero;
            prefab.transform.localRotation = Quaternion.identity;
            prefab.SetActive(false);
        }
    }
}
