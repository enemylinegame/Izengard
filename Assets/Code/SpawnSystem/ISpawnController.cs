using System;
using UnitSystem;
using UnitSystem.Enum;

namespace SpawnSystem
{
    public interface ISpawnController : IOnController, IOnUpdate
    {
        public event Action<IUnit> OnUnitSpawned;
        void SpawnUnit(UnitType unitType);
    }
}
