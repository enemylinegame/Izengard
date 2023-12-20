using System;
using UnitSystem;
using UnitSystem.Enum;

namespace SpawnSystem
{
    public interface ISpawnController : IOnController
    {
        event Action<IUnit> OnUnitSpawned;

        void SpawnUnit(IUnitData unitData);
        
        void SpawnUnit(UnitType type);

        void DespawnUnit(IUnit unit);
    }
}
