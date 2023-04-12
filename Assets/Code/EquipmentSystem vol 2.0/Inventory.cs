using UnityEngine;

namespace EquipmentSystem
{ 
[System.Serializable]
    public class Inventory
    {
        [Header("ArmorSlots")]
        [SerializeField] private ArmorSlotsController _armorSlotController;

        [Header("Weapon Slots")]
        [SerializeField] private WeaponSlotController _weaponSlotController;

        private CurrentParameters _inventoryModificationParam;

        private float _costEquipmentinGold;

        public ArmorSlotsController ArmorSlotsController => _armorSlotController;
        public WeaponSlotController WeaponSlotController => _weaponSlotController;
        public CurrentParameters InventoryModificationParam => _inventoryModificationParam;
        public float CostEquipmentinGold => _costEquipmentinGold;

        public void ChangeItem(ItemModel model)
        {
            if (model is ArmorModel)
            {                
                _armorSlotController.ChangeArmor((ArmorModel)model);
            }
            if (model is WeaponModel)
            {
                _weaponSlotController.EquipWeapon((WeaponModel)model);
            }
            _inventoryModificationParam = ArmorSlotsController.AllArmorParameters + WeaponSlotController.AllWeaponParameters;
            CheckCostInventory();
        }
        public ItemModel UnequipItem(ItemModel model)
        {
            ItemModel tempmodel = null;
            if (model is ArmorModel)
            {
                tempmodel=_armorSlotController.UnequipArmor((ArmorModel)model);
            }
            if (model is WeaponModel)
            {
                tempmodel=_weaponSlotController.UnequipWeapon((WeaponModel)model);
            }
            _inventoryModificationParam = ArmorSlotsController.AllArmorParameters + WeaponSlotController.AllWeaponParameters;
            CheckCostInventory();
            return tempmodel;
        }
        public void SetUnitForSlots(UnitView unit)
        {
            _armorSlotController.SetUnitForSlots(unit);
            _weaponSlotController.SetUnitForSlots(unit);
        }
        public void CheckCostInventory()
        {
            
            _costEquipmentinGold = _weaponSlotController.CostWeaponInGold + _armorSlotController.CostArmorInGold;
        }
    }
}
