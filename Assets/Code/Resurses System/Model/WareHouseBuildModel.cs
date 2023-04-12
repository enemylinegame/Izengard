using ResurseSystem;
using EquipmentSystem;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem
{
    [System.Serializable]    
    public class WareHouseBuildModel : BuildingModel
    {
        public System.Action<ResurseHolder> AddReusurseAction;
        public System.Action<Item—arrierHolder> AddItemAction;

        public ResurseStock WareHouseStock=>_warehouseStock;

        [SerializeField] private ResurseStock _warehouseStock;
        [SerializeField] private GlobalResurseStock _globalResurseStock;

        public WareHouseBuildModel (WareHouseBuildModel baseBuilding):base(baseBuilding)
        {
            _warehouseStock = new ResurseStock(baseBuilding.WareHouseStock);

        }

        public void AddInStock(ResurseHolder holder)
        {
            //AddReusurseAction?.Invoke(holder);
            _globalResurseStock.AddResurseToGlobalResurseStock(holder);
        }
        public void AddInStock(Item—arrierHolder holder)
        {
            //AddItemAction?.Invoke(holder);
            _globalResurseStock.AddItemToGlobalResurseStock(holder);
        }

        public override void AwakeModel()
        {
           
        }
    }
}
