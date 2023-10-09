using Abstraction;
using System;
using System.Collections.Generic;
using UnitSystem.Data;
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

        public INavigation<Vector3> Navigation { get; }

        public IUnitDefence Defence { get; }

        public IUnitOffence Offence { get; }

        public UnitStateModel UnitState { get; }
        public UnitTargetModel Target { get; }
        public UnitPriorityModel Priority { get; }

        public Vector3 SpawnPosition => Vector3.zero;

        public StubUnit(string logMessage)
        {
            View = new StubUnitView();
            Debug.LogWarning(logMessage);
        }

        public event Action<IUnit> OnReachedZeroHealth;

        public void Disable() { }

        public void Enable() { }
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

        public void SetSpawnPosition(Vector3 spawnPosition) { }
    }
}
