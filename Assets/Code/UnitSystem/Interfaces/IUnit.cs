using Abstraction;
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

        UnitModel Model { get; }

        INavigation<Vector3> Navigation { get; }

        int Id { get; }

        void Enable();

        void Disable();
    }
}
