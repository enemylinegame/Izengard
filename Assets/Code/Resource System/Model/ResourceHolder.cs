

namespace ResourceSystem
{
    [System.Serializable]
    public class ResourceHolder : Holder<ResurseCraft>
    {
        
        public ResourceHolder(ResourceHolder holder)
        {
            _objectInHolder = holder.ObjectInHolder;
            _currentValue = holder.CurrentValue;
            _maxValue = holder.MaxValue;
        }
        public ResourceHolder(ResurseCraft res,float currentvalue,float maxvalue)
        {
            _objectInHolder = res;
            _currentValue = currentvalue;
            _maxValue = maxvalue;
        }
    }
}
