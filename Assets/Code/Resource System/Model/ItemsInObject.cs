using System.Collections.Generic;
using EquipmentSystem;
using UnityEngine;

namespace BuildingSystem
{ 
    [System.Serializable]
    public class ItemsInObject 
    {
        [SerializeField] private List<ArmorModel> _armorsInObject;
        [SerializeField] private List<WeaponModel> _weaponsInObject;

        public List<ArmorModel> ArmorsInObject=> _armorsInObject;
        public List<WeaponModel> WeaponsInObject => _weaponsInObject;
    }
}
