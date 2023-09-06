using UnityEngine;
using Wave;

namespace EnemyUnit
{
    public class EnemyModel
    {
        private readonly EnemyType _type;
        private readonly EnemyStats _stats;

        private int _currentHealth;

        public EnemyType Type => _type;
        public EnemyStats Stats => _stats;
        public int CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                if(_currentHealth != value)
                {
                    if (value > _stats.Health)
                        _currentHealth = _stats.Health;
                    else if (value < 0)
                        _currentHealth = 0;
                    else
                        _currentHealth = value;                   
                }
            }
        }

        public EnemyModel(EnemyData data)
        {
            _type = data.Type;
            _stats = new EnemyStats
            {
                Health = data.Stats.Health,
                Attack = data.Stats.Attack,
                AttackSpeed = data.Stats.AttackSpeed,
                AttackRange = data.Stats.AttackRange,
                RunSpeed = data.Stats.RunSpeed,
                Cost = data.Stats.Cost,
                Gold = data.Stats.Gold
            };
        }

        public void IncreaseHealth(int amount)
        {
            CurrentHealth += amount;
        }

        public void DecreaseHealth(int amount) 
        {
            CurrentHealth -= amount;
        }

    }
}
