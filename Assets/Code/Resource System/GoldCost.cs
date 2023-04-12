using UnityEngine;

namespace ResourceSystem
{ 
    [System.Serializable]
    public class GoldCost
    {
        public ResurseCraft GoldObject => _goldObject;
        public float Cost => _cost;
        
        
        [SerializeField] private ResurseCraft _goldObject;
        [SerializeField] private float _cost;

        public GoldCost(GoldCost cost, float value)
        {
            _goldObject = cost.GoldObject;
            _cost = value;
        }
        public GoldCost(GoldCost cost)
        {
            _goldObject = cost.GoldObject;
            _cost = cost.Cost;
        }
        public void ChangeCost(float value)
        {
            _cost = value;
        }
    }
}
