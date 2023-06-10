using System.Collections.Generic;
using CombatSystem.Interfaces;

namespace CombatSystem
{
    public sealed class DefenderTargetsHolder
    {
        public IDamageable CurrentTarget;
        public List<IDamageable> TargetsInRange;
        public List<IDamageable> AttackingTargets;

        public DefenderTargetsHolder()
        {
            TargetsInRange = new List<IDamageable>();
            AttackingTargets = new List<IDamageable>();
        }

    }
}