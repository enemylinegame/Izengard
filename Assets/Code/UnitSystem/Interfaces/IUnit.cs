using Izengard.Abstraction.Interfaces;
using Izengard.UnitSystem.View;
using UnityEngine;

namespace Izengard.UnitSystem
{
    public interface IUnit : 
        IDamageable<UnitDamage>, 
        IDamageDealer<UnitDamage>, 
        IPositioned<Vector3>, 
        IRotated<Vector3>
    {
        IUnitView View { get; }

        UnitModel Model { get; }

        int Index { get; }

        void Enable();

        void Disable();
    }
}
