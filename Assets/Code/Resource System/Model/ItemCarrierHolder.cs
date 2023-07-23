using ResourceSystem;

namespace EquipmentSystem
{
    [System.Serializable]
    public class ItemСarrierHolder : Holder<ItemModel>
    {
         
        public ItemСarrierHolder(ItemСarrierHolder itholder)
        {
            _objectInHolder = itholder.ObjectInHolder;
            _currentValue = itholder.CurrentValue;
            _maxValue = itholder.MaxValue;
        }
        public ItemСarrierHolder(ItemModel item, float currentValue, float maxItemValue)
        {
            _objectInHolder = (ItemModel)item;
            _currentValue = currentValue;
            _maxValue = maxItemValue;
        }
    }
}

       
