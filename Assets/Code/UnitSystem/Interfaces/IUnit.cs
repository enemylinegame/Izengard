using Abstraction;
using System;
using UnitSystem.Model;
using UnityEngine;

namespace UnitSystem
{
    public interface IUnit : 
        IDamageable, 
        IDamageDealer, 
        IPositioned<Vector3>, 
        IRotated<Vector3>
    {
        IUnitView View { get; }
        UnitStatsModel Stats { get; }
        IUnitDefence Defence { get; }
        IUnitOffence Offence { get; }
        UnitNavigationModel Navigation { get; }
        UnitTargetModel Target { get; }
        UnitStateModel UnitState { get; }
        UnitPriorityModel Priority { get; }

        event Action<IUnit> OnReachedZeroHealth;

        int Id { get; }

        Vector3 StartPosition { get; }
        
        public float TimeProgress { get; set; }

        void Enable();

        void Disable();

        void SetStartPosition(Vector3 spawnPosition);
    }
}
