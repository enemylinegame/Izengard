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
        INavigation<Vector3> Navigation { get; }
        UnitStateModel UnitState { get; }
        UnitTargetModel Target { get; }
        
        event Action<IUnit> OnReachedZeroHealth;

        int Id { get; }

        Vector3 SpawnPosition { get; }

        void Enable();

        void Disable();

        void SetSpawnPosition(Vector3 spawnPosition);
    }
}
