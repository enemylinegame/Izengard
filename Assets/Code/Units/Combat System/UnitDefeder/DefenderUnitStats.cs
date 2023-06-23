using System;

namespace CombatSystem
{
    [Serializable]
    public class DefenderUnitStats
    {
        public float AttackInterval;
        public float AttackRange;
        public float VisionRange;
        public float MovementSpeed;
        public int AttackDamage;
        public int MaxHealth;
        public int ThreatLevel;

        public DefenderUnitStats(float attackInterval, float attackRange, float visionRange, int attackDamage, int maxHealth)
        {
            AttackInterval = attackInterval;
            AttackRange = attackRange;
            VisionRange = visionRange;
            AttackDamage = attackDamage;
            MaxHealth = maxHealth;
        }
    }
}