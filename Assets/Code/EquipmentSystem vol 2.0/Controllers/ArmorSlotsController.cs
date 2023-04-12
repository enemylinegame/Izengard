using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EquipmentSystem
{ 
    [System.Serializable]
    public class ArmorSlotsController 
    {
        [SerializeField] private ArmorSlot _glovesSlot;
        [SerializeField] private ArmorSlot _shouldersSlot;
        [SerializeField] private ArmorSlot _helmetSlot;
        [SerializeField] private ArmorSlot _bootsSlot;
        [SerializeField] private ArmorSlot _bodyarmorSlot;

        private CurrentParameters _allArmorParameters;
        private float _costInGold;
        
        public ArmorSlot GlovesSlot => _glovesSlot;
        public ArmorSlot ShouldersSlot => _shouldersSlot;
        public ArmorSlot HelmetSlot => _helmetSlot;
        public ArmorSlot BootsSlot => _bootsSlot;
        public ArmorSlot BodyArmorSlot => _bodyarmorSlot;
        public CurrentParameters AllArmorParameters => _allArmorParameters;
        public float CostArmorInGold => _costInGold;

        public void ChangeArmor(ArmorModel model)
        {
            switch (model.ArmorSlotTypeID)
            {
                case 1:
                    _bodyarmorSlot.EquipItem(model);
                    break;
                case 2:
                    _glovesSlot.EquipItem(model);
                    break;
                case 3:
                    _bootsSlot.EquipItem(model);
                    break;
                case 4:
                    _helmetSlot.EquipItem(model);
                    break;
                case 5:
                    _shouldersSlot.EquipItem(model);
                    break;
                default:
                    break;
            }
            CheckSlotsModifications();
            CheckCostInGold();
        }

        public ArmorModel UnequipArmor(ArmorModel model)
        {
            ArmorModel unequipModel = null;
            switch (model.ArmorSlotTypeID)
            {
                case 1:
                    if (model == _bodyarmorSlot.ItemModel)
                    {
                        unequipModel= _bodyarmorSlot.UnequipItem();
                    }
                    break;
                case 2:
                    if (model == _glovesSlot.ItemModel)
                    {
                        unequipModel= _glovesSlot.UnequipItem();
                    }
                    break;
                case 3:
                    if (model == _bootsSlot.ItemModel)
                    {
                        unequipModel= _bootsSlot.UnequipItem();
                    }
                    break;
                case 4:
                    if (model == _helmetSlot.ItemModel)
                    {
                        unequipModel= _helmetSlot.UnequipItem();
                    }
                    break;
                case 5:
                    if (model == _shouldersSlot.ItemModel)
                    {
                        unequipModel= _shouldersSlot.UnequipItem();
                    }
                    break;
                default:
                    break;
            }
            CheckSlotsModifications();
            CheckCostInGold();
            return unequipModel;
        }
        public List<ArmorModel> UnequipAll()
        {
            List<ArmorModel> unequipItems = new List<ArmorModel>();
            unequipItems.Add(_glovesSlot.UnequipItem());
            unequipItems.Add(_shouldersSlot.UnequipItem());
            unequipItems.Add(_bootsSlot.UnequipItem());
            unequipItems.Add(_helmetSlot.UnequipItem());
            unequipItems.Add(_shouldersSlot.UnequipItem());
            return unequipItems;
        }

        public void CheckSlotsModifications ()
        {
            
            if (_glovesSlot.ItemModel!=null && _helmetSlot.ItemModel != null && _bodyarmorSlot.ItemModel != null &&
                _shouldersSlot.ItemModel != null && _bootsSlot.ItemModel != null)
            { 
            _allArmorParameters = _glovesSlot.ItemModel.ItemParameters + _helmetSlot.ItemModel.ItemParameters + _bodyarmorSlot.ItemModel.ItemParameters +
                _shouldersSlot.ItemModel.ItemParameters + _bootsSlot.ItemModel.ItemParameters;
            }
            else
                _allArmorParameters = new CurrentParameters();
        }
        public void SetUnitForSlots(UnitView unit)
        {
            _glovesSlot.SetUnitView(unit);
            _helmetSlot.SetUnitView(unit);
            _bodyarmorSlot.SetUnitView(unit);
            _shouldersSlot.SetUnitView(unit);
            _bootsSlot.SetUnitView(unit);
        }
        public void CheckCostInGold()
        {                        
            if (_glovesSlot.ItemModel!=null && _shouldersSlot.ItemModel != null && _bodyarmorSlot.ItemModel != null && _helmetSlot.ItemModel != null && _bootsSlot.ItemModel != null)
            {
                _costInGold = _glovesSlot.ItemModel.CostInGold.Cost + _shouldersSlot.ItemModel.CostInGold.Cost
                    + _helmetSlot.ItemModel.CostInGold.Cost + _bootsSlot.ItemModel.CostInGold.Cost
                    + _bodyarmorSlot.ItemModel.CostInGold.Cost;
            }
            else
                _costInGold = 0;
        }
        public void AwakeAllArmor()
        {
            _glovesSlot.AwakeModel();
            _helmetSlot.AwakeModel();
            _bodyarmorSlot.AwakeModel();
            _shouldersSlot.AwakeModel();
            _bootsSlot.AwakeModel();
            CheckSlotsModifications();
            CheckCostInGold();
        }
    }
}
