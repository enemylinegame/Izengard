using EquipmentSystem;


namespace ResourceSystem
{ 
    [System.Serializable]
    public class ItemProduct : Product<ItemModel>
    {

        public ItemProduct(ItemModel obj, float produceValue, float produceTime, ResourceCost costInResource) : base(obj, produceValue, produceTime, costInResource)
        {

        }
        public ItemProduct(ItemModel obj, float produceValue, float produceTime) : base(obj, produceValue, produceTime)
        {

        }
        public ItemProduct(ItemProduct product) : base(product)
        {

        }
    }
}
