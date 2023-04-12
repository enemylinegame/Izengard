

namespace ResurseSystem
{
    [System.Serializable]
    public class ResurseHolder : Holder<ResurseCraft>
    {
        
        public ResurseHolder(ResurseHolder holder)
        {
            _objectInHolder = holder.ObjectInHolder;
            _currentValue = holder.CurrentValue;
            _maxValue = holder.MaxValue;
        }
        public ResurseHolder(ResurseCraft res,float currentvalue,float maxvalue)
        {
            _objectInHolder = res;
            _currentValue = currentvalue;
            _maxValue = maxvalue;
        }
    }
}
