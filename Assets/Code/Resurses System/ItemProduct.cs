using EquipmentSystem;


namespace ResurseSystem
{ 
    [System.Serializable]
    public class ItemProduct : Product<ItemModel>
    {

        public ItemProduct(ItemModel obj, float produceValue, float produceTime, ResurseCost costInResurse) : base(obj, produceValue, produceTime, costInResurse)
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
