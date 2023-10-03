namespace UnitSystem
{
    public interface IUnitController : 
        IOnController, 
        IOnUpdate, 
        IOnFixedUpdate
    {
        void Enable();
        void Disable();

        void AddUnit(IUnit unit);

        void RemoveUnit(int unitId);
    }
}
