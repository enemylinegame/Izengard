using UnitSystem;
using UnitSystem.Enum;

namespace SpawnSystem
{
    public interface ISpawnController : IOnController
    {
        void SpawnUnit(UnitType unitType);
        void DespawnUnit(IUnit unit);
    }
}
