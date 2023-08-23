using UnityEngine;
using Wave;

namespace CombatSystem.UnitEnemy
{
    public class EnemyModel
    {
        private readonly EnemyType _type;
        private readonly EnemyStats _stats;
        private readonly GameObject _rootGameObject;
        private readonly Damageable _myDamagable;

        public EnemyType Type => _type;

        public EnemyStats Stats => _stats;

        public GameObject RootGameObject => _rootGameObject;

        public Damageable MyDamagable => _myDamagable;

        public EnemyModel(
            EnemySettings enemySettings, 
            GameObject rootGO, 
            Damageable damageable)
        {
            _type = enemySettings.Type;
            _stats = new EnemyStats
            {
                Health = enemySettings.Stats.Health,
                Attack = enemySettings.Stats.Attack,
                AttackSpeed = enemySettings.Stats.AttackSpeed,
                AttackRange = enemySettings.Stats.AttackRange,
                RunSpeed = enemySettings.Stats.RunSpeed,
                Cost = enemySettings.Stats.Cost,
                Gold = enemySettings.Stats.Gold
            };
            _rootGameObject = rootGO;
            _myDamagable = damageable;
        }
    }
}
