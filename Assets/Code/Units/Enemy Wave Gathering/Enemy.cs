using UnityEngine;


namespace Wave
{
    public class Enemy
    {
        public readonly EnemyType Type;
        public readonly EnemyStats Stats;
        public readonly GameObject RootGameObject;
        public readonly Damageable MyDamagable;
        
        public Enemy(EnemySettings enemySettings, GameObject rootGO, Damageable damageable)
        {
            Type = enemySettings.Type;
            Stats = new EnemyStats
            {
                Health = enemySettings.Stats.Health,
                Attack = enemySettings.Stats.Attack,
                AttackSpeed = enemySettings.Stats.AttackSpeed,
                AttackRange = enemySettings.Stats.AttackRange,
                RunSpeed = enemySettings.Stats.RunSpeed,
                Cost = enemySettings.Stats.Cost,
                Gold = enemySettings.Stats.Gold
            };
            RootGameObject = rootGO;
            MyDamagable = damageable;
            }
    }
}