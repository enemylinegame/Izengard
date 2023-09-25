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
        IUnitView View { get; }

        UnitModel Model { get; }

        INavigation<Vector3> Navigation { get; }

        int Id { get; }

        void Enable();

        void Disable();
    }
}
