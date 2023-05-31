using System;
using UnityEngine;

namespace CombatSystem.Interfaces
{
    public interface IDamageable
    {

        Vector3 Position { get; }
        int ThreatLevel { get; }
        bool IsDead { get; }
        
        event Action DeathAction;
        event Action<IDamageable> OnDamaged; 

        void MakeDamage(int damage, IDamageable damageDealer);

    }
}