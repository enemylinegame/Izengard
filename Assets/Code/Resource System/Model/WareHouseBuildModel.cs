using ResourceSystem;
using EquipmentSystem;
using UnityEngine;

namespace BuildingSystem
{
    [System.Serializable]    
    public class WareHouseBuildModel : BuildingModel
    {
        public System.Action<ResourceHolder> AddReusurseAction;
        public System.Action<ItemСarrierHolder> AddItemAction;

        public ResurseStock WareHouseStock=>_warehouseStock;

        [SerializeField] private ResurseStock _warehouseStock;
        [SerializeField] private GlobalResorceStock globalResorceStock;

        public WareHouseBuildModel (WareHouseBuildModel baseBuilding):base(baseBuilding)
        {
            _warehouseStock = new ResurseStock(baseBuilding.WareHouseStock);

        }

        public void AddInStock(ResourceHolder holder)
        {
            //AddReusurseAction?.Invoke(holder);
            globalResorceStock.AddResurseToGlobalResurseStock(holder);
        }
        public void AddInStock(ItemСarrierHolder holder)
        {
            //AddItemAction?.Invoke(holder);
            globalResorceStock.AddItemToGlobalResurseStock(holder);
        }

        public override void AwakeModel()
        {
           
        }
    }
}
