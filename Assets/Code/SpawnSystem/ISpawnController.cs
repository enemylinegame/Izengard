using System;
using UnitSystem;
using UnitSystem.Enum;

namespace SpawnSystem
{
    public interface ISpawnController : IOnController
    {
        event Action<IUnit> OnUnitSpawned;

        void SpawnUnit(UnitType unitType);
        void DespawnUnit(IUnit unit);
    }
}
