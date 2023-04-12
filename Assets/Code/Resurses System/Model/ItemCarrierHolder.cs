using ResurseSystem;

namespace EquipmentSystem
{
    [System.Serializable]
    public class ItemÑarrierHolder : Holder<ItemModel>
    {
         
        public ItemÑarrierHolder(ItemÑarrierHolder itholder)
        {
            _objectInHolder = itholder.ObjectInHolder;
            _currentValue = itholder.CurrentValue;
            _maxValue = itholder.MaxValue;
        }
        public ItemÑarrierHolder(ItemModel item, float currentValue, float maxItemValue)
        {
            _objectInHolder = (ItemModel)item;
            _currentValue = currentValue;
            _maxValue = maxItemValue;
        }
    }
}

       
