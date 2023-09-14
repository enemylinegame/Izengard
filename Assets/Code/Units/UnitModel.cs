using Units.Data;

namespace Units 
{
    public class UnitModel : IUnit
    {
        private readonly IUnitData _data;
        private readonly UnitDefenceModel _defenceModel;

        private int _currentHealth;
        private int _currentArmor;

        public int CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                if(_currentHealth != value)
                {
                    if (_currentHealth > _data.HealthPoints)
                        _currentHealth = _data.HealthPoints;
                    else if (_currentHealth < 0)
                        _currentHealth = 0;
                    else
                    {
                        _currentHealth = value;
                    }
                }
            }
        }       
        public int CurrentArmor
        {
            get => _currentArmor;
            private set
            {
                if(_currentArmor != value)
                {
                    if(_currentArmor < 0)
                    {
                        _currentArmor = 0;
                    }
                    else
                    {
                        _currentArmor = value;
                    }
                }
            }
        }

        public UnitModel(IUnitData data) 
        {
            _data = data;

            _currentHealth = data.HealthPoints;
            _currentArmor = data.ArmorPoints;

            _defenceModel = new UnitDefenceModel(data.DefenceData);
        }


        public void IncreaseHealth(int amount)
        {
            CurrentHealth += amount;
        }

        public void DecreaseHealth(int amount)
        {
            var resultAmount = amount;

            resultAmount 
                = _defenceModel.GetCutDefenceDamage(resultAmount);

            if (CurrentArmor != 0)
            {
                resultAmount /= 2;
            }

            CurrentHealth -= resultAmount;
        }

        public void DecreaseArmor(int amount)
        {
            CurrentArmor -= amount;
        }
    }
}

  
