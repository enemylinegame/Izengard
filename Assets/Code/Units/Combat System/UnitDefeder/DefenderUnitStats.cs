namespace CombatSystem
{
    public class DefenderUnitStats
    {
        public float AttackInterval;
        public float AttackRange;
        public int AttackDamage;
        public int MaxHealth;

        public DefenderUnitStats(float attackInterval, float attackRange, int attackDamage, int maxHealth)
        {
            AttackInterval = attackInterval;
            AttackRange = attackRange;
            AttackDamage = attackDamage;
            MaxHealth = maxHealth;
        }
    }
}