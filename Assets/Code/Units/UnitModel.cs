using Izengard.Units.Data;

namespace Izengard.Units 
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
            CurrentHealth -= amount;
        }

        public void DecreaseArmor(int amount)
        {
            CurrentArmor -= amount;
        }

        public void TakeDamage(IUnitDamageData damageValue)
        {
            var damageAmount
                = _defenceModel.GetAfterDefDamage(damageValue);

            if (CurrentArmor != 0)
            {
                damageAmount /= 2;
                CurrentArmor--;
            }

            DecreaseHealth(damageAmount);
        }
    }
}

  
