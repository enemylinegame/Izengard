﻿using Abstraction;
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
        IKillable<IUnit>
    {
        IUnitView View { get; }
        UnitStatsModel Stats { get; }
        IUnitDefence Defence { get; }
        IUnitOffence Offence { get; }
        UnitTargetModel Target { get; }
        UnitStateModel State { get; }
        UnitPriorityModel Priority { get; }

        int Id { get; }

        Vector3 StartPosition { get; }
        
        float TimeProgress { get; set; }

        bool IsInFight { get; set; }

        void Enable();

        void Disable();

        void SetStartPosition(Vector3 spawnPosition);

        void ChangeState(UnitStateType state);
    }
}
