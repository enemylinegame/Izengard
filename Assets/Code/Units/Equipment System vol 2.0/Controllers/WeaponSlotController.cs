using System;
using System.Collections.Generic;
using UnityEngine;

namespace EquipmentSystem
{
    [Serializable]
    public class WeaponSlotController
    {
        [SerializeField] private WeaponSlot _rightHandSlot;
        [SerializeField] private WeaponSlot _leftHandSlot;
        [SerializeField] private WeaponSlot _twoHandSlot;
        [SerializeField] private BackSlot _leftBackSlot;
        [SerializeField] private BackSlot _rightBackSlot;

        private WeaponSlot _activeSlot;

        private CurrentParameters _allWeaponParameters;
        private float _costInGold;

        public Action<float> OnEquipWeapon;
        public WeaponSlot RightHandSlot => _rightHandSlot;
        public WeaponSlot LeftHandSlot => _leftHandSlot;
        public WeaponSlot TwoHandSlot => _twoHandSlot;
        public BackSlot LeftBackSlot => _leftBackSlot;
        public BackSlot RightBackSlot => _rightBackSlot;
        public WeaponSlot ActiveSlot => _activeSlot;
        public CurrentParameters AllWeaponParameters => _allWeaponParameters;
        public float CostWeaponInGold => _costInGold;

        public List<WeaponModel> EquipWeapon(WeaponModel newWeaponModel)
        {
            List<WeaponModel> previousWeapons = new List<WeaponModel>();
            
            switch (newWeaponModel.WeaponGripTypeID)
            {
                case 1:
                    previousWeapons.Add(_twoHandSlot.UnequipItem());
                    previousWeapons.Add(_rightHandSlot.UnequipItem());
                    _rightHandSlot.EquipItem(newWeaponModel);
                    //_rightBackSlot.SetBackPrefab(newWeaponModel.BackSlotPrefab);
                    _activeSlot = _rightHandSlot;
                    break;
                case 2:
                    previousWeapons.Add(_leftHandSlot.UnequipItem());
                    previousWeapons.Add(_rightHandSlot.UnequipItem());
                    previousWeapons.Add(_twoHandSlot.UnequipItem());
                    _twoHandSlot.EquipItem(newWeaponModel);
                    //_rightBackSlot.SetBackPrefab(newWeaponModel.BackSlotPrefab);
                    _activeSlot = _twoHandSlot;
                    break;
                case 3:
                    previousWeapons.Add(_twoHandSlot.UnequipItem());
                    previousWeapons.Add(_leftHandSlot.UnequipItem());
                    _leftHandSlot.EquipItem(newWeaponModel);
                    //_leftBackSlot.SetBackPrefab(newWeaponModel.BackSlotPrefab);
                    _activeSlot = _leftHandSlot;
                    break;
                default:
                    break;
            }

            CheckSlotsModifications();
            CheckCostInGold();
            return previousWeapons;
        }

        public List<WeaponModel> EquipWeaponOnBack(WeaponModel newWeaponModel)
        {
            List<WeaponModel> previousWeapons = new List<WeaponModel>();

            switch (newWeaponModel.WeaponGripTypeID)
            {
                case 1:
                    _rightBackSlot.SetBackPrefab(newWeaponModel.BackSlotPrefab);
                    previousWeapons.Add(_rightBackSlot.UnequipItem());
                    _rightBackSlot.EquipItem(newWeaponModel);
                    if (_leftHandSlot.ItemModel.WeaponType.Value != WeaponTypes.Staff)
                    {
                        _activeSlot = _rightHandSlot;
                    }
                    break;
                case 2:
                    _rightBackSlot.SetBackPrefab(newWeaponModel.BackSlotPrefab);
                    previousWeapons.Add(_rightBackSlot.UnequipItem());
                    previousWeapons.Add(_leftBackSlot.UnequipItem());
                    _rightBackSlot.EquipItem(newWeaponModel);
                    _activeSlot = _twoHandSlot;
                    break;
                case 3:
                    _leftBackSlot.SetBackPrefab(newWeaponModel.BackSlotPrefab);
                    previousWeapons.Add(_leftBackSlot.UnequipItem());
                    _leftBackSlot.EquipItem(newWeaponModel);
                    if (_leftHandSlot.ItemModel.WeaponType.Value == WeaponTypes.Staff)
                    {
                        _activeSlot = _leftHandSlot;
                    }
                    break;
                default:
                    break;
            }
            CheckSlotsModifications();
            CheckCostInGold();
            return previousWeapons;
        }

        public void SwapWeapon()
        {
            WeaponModel previousRightHandWeapon;
            WeaponModel previousLeftHandWeapon;
            WeaponModel previousTwoHandWeapon;

            switch (_rightBackSlot.ItemModel.WeaponGripTypeID)
            {
                case 1:
                    previousRightHandWeapon = _rightHandSlot.UnequipItem();
                    _rightHandSlot.EquipItem(_rightBackSlot.ItemModel);
                    if (_leftBackSlot.ItemModel != null)
                    {
                        var previousLeftWeapon = _leftBackSlot.UnequipItem();
                        _leftHandSlot.EquipItem(_rightBackSlot.ItemModel);
                        if (previousLeftWeapon != null)
                        {
                            _leftBackSlot.EquipItem(previousLeftWeapon);
                        }
                    }
                    _rightBackSlot.EquipItem(previousRightHandWeapon);
                    break;
                case 2:
                    previousTwoHandWeapon = _twoHandSlot.UnequipItem();
                    _twoHandSlot.EquipItem(_rightBackSlot.ItemModel);
                    if (previousTwoHandWeapon == null)
                    {
                        previousRightHandWeapon = _rightHandSlot.UnequipItem();
                        previousLeftHandWeapon = _leftHandSlot.UnequipItem();

                        _rightBackSlot.EquipItem(previousRightHandWeapon);
                        _leftBackSlot.EquipItem(previousLeftHandWeapon);
                    }
                    else
                    {
                        _rightBackSlot.EquipItem(previousTwoHandWeapon);
                    }
                    break;
                default:
                    break;
            }
            CheckSlotsModifications();
        }

        public void SwapActiveSlot()
        {
            if (_activeSlot.SecondSlot == null) return;

            if (_activeSlot.SecondSlot.ItemModel != null && _activeSlot.SecondSlot.ItemModel.WeaponType.Value != WeaponTypes.Shield)
            {
                _activeSlot = _activeSlot.SecondSlot;
            }
        }

        public void Init()
        {
            _rightHandSlot.SecondSlot = _leftHandSlot;
            _leftHandSlot.SecondSlot = _rightHandSlot;
        }
        public void CheckSlotsModifications()
        {
            
            switch (_activeSlot.ItemModel.WeaponGripTypeID)
            {
                case 1:
                    _allWeaponParameters = RightHandSlot.ItemModel.ItemParameters;
                    if (LeftHandSlot.ItemModel!=null)
                    {
                        _allWeaponParameters = RightHandSlot.ItemModel.ItemParameters + LeftHandSlot.ItemModel.ItemParameters;
                    }
                    
                    
                    break;
                case 2:
                    _allWeaponParameters = TwoHandSlot.ItemModel.ItemParameters;
                    break;
                case 3:
                    if (RightHandSlot.ItemModel != null)
                    {
                        _allWeaponParameters = RightHandSlot.ItemModel.ItemParameters + LeftHandSlot.ItemModel.ItemParameters;
                    }
                    else
                    {
                        _allWeaponParameters =LeftHandSlot.ItemModel.ItemParameters;
                    }
                    
                    break;
                default:
                    break;
            }  
        }
        public List<WeaponModel> UnequipAllWeapon()
        {
            List<WeaponModel> weapons = new List<WeaponModel>();
            weapons.Add(_rightHandSlot.UnequipItem());
            weapons.Add(_leftHandSlot.UnequipItem());
            weapons.Add(_twoHandSlot.UnequipItem());
            weapons.Add(_leftBackSlot.UnequipItem());
            weapons.Add(_rightBackSlot.UnequipItem());            
            _allWeaponParameters = new CurrentParameters();
            _costInGold = 0;
            return weapons;
        }
        public void SetUnitForSlots(UnitView unit)
        {
            _rightHandSlot.SetUnitView(unit);
            _leftHandSlot.SetUnitView(unit);
            _twoHandSlot.SetUnitView(unit);
            _leftBackSlot.SetUnitView(unit);
            _rightBackSlot.SetUnitView(unit);
        }
        public void CheckCostInGold()
        {
            _costInGold = 0;
            if (RightHandSlot.ItemModel!=null)
            {
                _costInGold += RightHandSlot.ItemModel.CostInGold.Cost;
            }
            if (LeftHandSlot.ItemModel != null)
            {
                _costInGold += LeftHandSlot.ItemModel.CostInGold.Cost;
            }
            if (TwoHandSlot.ItemModel != null)
            {
                _costInGold += TwoHandSlot.ItemModel.CostInGold.Cost;
            }
        }
        public WeaponModel UnequipWeapon(WeaponModel model)
        {
            WeaponModel tempModel = null;
           switch (model.WeaponGripTypeID)
            {
                case 1:
                    if (model == _rightHandSlot.ItemModel)
                    {
                        tempModel = (_rightHandSlot.UnequipItem());
                    }                    
                        break;
                case 2:
                    if (model == _leftHandSlot.ItemModel)
                    {
                        tempModel = (_leftHandSlot.UnequipItem());
                    }
                    break;
                case 3:
                    if (model == _twoHandSlot.ItemModel)
                    {
                        tempModel = (_twoHandSlot.UnequipItem());
                    }
                    break;
                default:
                    break;  
            }
            CheckSlotsModifications();
            CheckCostInGold();
            return tempModel;
        }

    }
}
