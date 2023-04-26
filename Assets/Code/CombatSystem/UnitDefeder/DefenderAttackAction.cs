using System;
using UnityEngine;

namespace CombatSystem
{
    public class DefenderAttackAction : IAction<Damageable>
    {
        private DefenderUnitStats _stats;

        public DefenderAttackAction(DefenderUnitStats stats)
        {
            _stats = stats;
        }

        public event Action<Damageable> OnComplete;

        public void StartAction(Damageable target)
        {
            Debug.Log("DefenderAttackAction->StartAction:");
            target.MakeDamage(_stats._attackDamage);
            OnComplete?.Invoke(target);
        }

        
    }

}
