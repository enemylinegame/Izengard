using Abstraction;

namespace UnitSystem
{
    public interface IRegularAttackController
    {
        void AddUnit(IAttacker unit);
        void RemoveUnit(IAttacker unit);
    }
}