using UnityEngine;


namespace Wave
{
    public class Enemy
    {
        public readonly EnemyType Type;
        public readonly EnemyStats Stats;
        public readonly GameObject Prefab;


        public Enemy(EnemySettings enemySettings, GameObject prefab)
        {
            Type = enemySettings.Type;
            Stats = new EnemyStats
            {
                Health = enemySettings.Stats.Health,
                Attack = enemySettings.Stats.Attack,
                AttackSpeed = enemySettings.Stats.AttackSpeed,
                RunSpeed = enemySettings.Stats.RunSpeed,
                Cost = enemySettings.Stats.Cost,
                Gold = enemySettings.Stats.Gold
            };
            Prefab = prefab;
        }
    }
}