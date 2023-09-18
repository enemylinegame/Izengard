using Izengard.Abstraction.Interfaces;
using UnityEngine;

namespace Izengard.UnitSystem
{
    public interface IUnit : 
        IDamageable<UnitDamage>, 
        IDamageDealer<UnitDamage>, 
        IPositioned<Vector3>, 
        IRotated<Vector3>
    {
        int Index { get; }

        void Enable();

        void Disable();
    }
}
