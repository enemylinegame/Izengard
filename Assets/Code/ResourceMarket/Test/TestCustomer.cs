using UnityEngine;

namespace ResourceMarket
{
    public class TestCustomer : ICustomer
    {
        private int _gold;
        public int Gold 
        {
            get => _gold;
            private set
            {
                if(_gold != value)
                {
                    _gold = Mathf.Clamp(value, 0, int.MaxValue);
                }
            }
        }

        public TestCustomer(int _initialGold)
        {
            Gold = _initialGold;
        }

        public void AddGold(int amount)
        {
            _gold += amount;
        }

        public void RemoveGold(int amount)
        {
            _gold -= amount;
        }
    }
}
