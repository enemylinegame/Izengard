using System;
using UnitSystem;

namespace BattleSystem
{
    public class DefendersSpawnController
    {
        public event Action<IUnit> OnUnitSpawned;
    }
}