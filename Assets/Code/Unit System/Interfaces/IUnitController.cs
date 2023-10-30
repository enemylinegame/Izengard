using System;

namespace UnitSystem
{
    public interface IUnitController : 
        IOnController, 
        IOnUpdate, 
        IOnFixedUpdate
    {
        event Action<IUnit> OnUnitDone;

        void AddUnit(IUnit unit);

        void RemoveUnit(int unitId);
    }
}
