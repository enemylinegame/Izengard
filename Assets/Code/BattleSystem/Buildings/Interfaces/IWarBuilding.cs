using System;
using UnityEngine;
using Abstraction;
using UnitSystem;


namespace BattleSystem.Buildings.Interfaces
{
    public interface IWarBuilding : IDamageable
    {
        int Id { get; }
        
        IWarBuildingView View { get; }
        
        event Action<IWarBuilding> OnReachedZeroHealth;
        
        void Enable();

        void Disable();
    }
}