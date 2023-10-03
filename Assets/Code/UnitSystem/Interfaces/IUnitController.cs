using System;

namespace UnitSystem
{
    public interface IUnitController : 
        IOnController, 
        IOnUpdate, 
        IOnFixedUpdate
    {
        event Action<IUnit> OnUnitDone;

        void Enable();
        void Disable();

        void AddUnit(IUnit unit);

        void RemoveUnit(int unitId);
    }
}
