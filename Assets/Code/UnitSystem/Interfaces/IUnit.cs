using Abstraction;
using Tools;
using UnitSystem.Enum;
using UnitSystem.Model;
using UnityEngine;

namespace UnitSystem
{
    public interface IUnit : 
        IDamageDealer,
        IMovable,
        IPositioned<Vector3>, 
        IRotated<Vector3>,
        IKillable<IUnit>,
        IHealthBarHandeled
    {
        IUnitView View { get; }
        UnitStatsModel Stats { get; }
        IUnitDefence Defence { get; }
        IUnitOffence Offence { get; }
        UnitTargetModel Target { get; }
        UnitStateModel State { get; }
        UnitPriorityModel Priority { get; }

        string Id { get; }

        string Name { get; }

        Vector3 StartPosition { get; }
        
        float TimeProgress { get; set; }

        void Enable();

        void Disable();

        void SetStartPosition(Vector3 spawnPosition);

        void ChangeState(UnitStateType state);
    }
}
