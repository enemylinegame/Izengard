using Abstraction;
using System;
using UnitSystem.Model;
using UnitSystem.View;
using UnityEngine;

namespace UnitSystem
{
    public class StubUnit : IUnit
    {
        public int Id => -1;

        public IUnitView View { get; private set; }

        public UnitStatsModel Stats { get; private set; }

        public UnitNavigationModel Navigation { get; }

        public IUnitDefence Defence { get; }

        public IUnitOffence Offence { get; }

        public UnitStateModel State { get; }
        public UnitTargetModel Target { get; }
        public UnitPriorityModel Priority { get; }

        public Vector3 StartPosition => Vector3.zero;

        public float TimeProgress { get; set; }
        
        public StubUnit(string logMessage)
        {
            View = new StubUnitView();
            Debug.LogWarning(logMessage);
        }

        public event Action<IUnit> OnReachedZeroHealth = default;
        public event Action<IDamageDealer, IDamageable> OnAttackProcessEnd = default;

        public void Disable() { }

        public void Enable() { }
        public bool IsAlive => true;
        public void TakeDamage(IDamage damageValue) { }

        public IDamage GetAttackDamage()
        {
            return new DamageStructure
            {
                BaseDamage = 0,
                FireDamage = 0,
                ColdDamage = 0
            };
        }

        public Vector3 GetPosition()
        {
            return Vector3.zero;
        }
        public void SetPosition(Vector3 pos) { }

        public Vector3 GetRotation()
        {
            return Vector3.zero;
        }

        public void SetRotation(Vector3 rot) { }

        public void SetStartPosition(Vector3 spawnPosition) { }

        public void StartAttack(IDamageable damageableTarget) { }
    }
}
