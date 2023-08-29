using UnityEngine;
using Wave;

namespace EnemyUnit
{
    public class EnemyModel
    {
        private readonly EnemyType _type;
        private readonly EnemyStats _stats;
        private readonly Damageable _myDamagable;

        public EnemyType Type => _type;

        public EnemyStats Stats => _stats;

        public Damageable MyDamagable => _myDamagable;

        public EnemyModel(
            EnemyData data, 
            Damageable damageable)
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
            _myDamagable = damageable;
        }
    }
}
