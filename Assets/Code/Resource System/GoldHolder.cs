using UnityEngine;

namespace ResourceSystem
{ 
    [System.Serializable]
    public class GoldHolder  
    {
        public ResurseCraft GoldObject => _goldObject;
        public float CurrentValue => _currentValue;

        [SerializeField] private ResurseCraft _goldObject;
        [SerializeField] private float _currentValue;

        public void AddGold(GoldCost cost)
        {
            if (cost.GoldObject==GoldObject)
            {
                _currentValue += cost.Cost;
            }
            else
            {
                Debug.Log("Что-то пошло не так либо в цене, либо в хранилище золота!");
            }
        }
        public void GetGold(GoldCost cost)
        {
            if (cost.GoldObject == GoldObject)
            {
                if (cost.Cost <= CurrentValue)
                {
                    _currentValue -= cost.Cost;
                }
                else Debug.Log($"Нужно больше золота!Не хватает: {cost.Cost-CurrentValue}");
            }
            else
            {
                Debug.Log("Что-то пошло не так либо в цене, либо в хранилище золота!");
            }
        }
        public void SetCurrentGold(float value)
        {
            _currentValue = value;
        }

    }
}
