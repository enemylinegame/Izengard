namespace ResourceSystem.SupportClases
{
    public class ResourceHolder
    {
        private ResourceType _resourceType;
        private int _currentAmount;
        private int _maxAmount;

        public ResourceType ResourceType => _resourceType;

        public int CurrentAmount => _currentAmount;

        public int MaxAmount => _maxAmount;

        public ResourceHolder(ResourceType resourceType, int maxAmount, int currentAmount = 0)
        {
            _resourceType = resourceType;
            _maxAmount = maxAmount;
            _currentAmount = currentAmount;
        }
        
        public bool CheckAmount(int value)
        {
           return _currentAmount <= value;
        } 

        public void AddResource(int value)
        {
            _currentAmount += value;
            if (_currentAmount > _maxAmount)
            {
                _currentAmount = _maxAmount;
            }
        }
        public void RemoveResource(int value)
        {
            _currentAmount -= value;
        }
    }
}