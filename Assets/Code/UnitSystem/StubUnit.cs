using Izengard.Abstraction.Interfaces;
using Izengard.UnitSystem.View;
using UnityEngine;

namespace Izengard.UnitSystem
{
    public class StubUnit : IUnit
    {
        public int Id => -1;

        public IUnitView View { get; private set; }

        public UnitModel Model { get; private set; }

        public INavigation<Vector3> Navigation { get; }

        public StubUnit(string logMessage)
        {
            View = new StubUnitView();
            Debug.LogWarning(logMessage);
        }

        public void Disable() { }

        public void Enable() { }
        public void TakeDamage(UnitDamage damageValue) { }

        public UnitDamage GetAttackDamage()
        {
            return new UnitDamage
            {
                BaseDamage = 0,
                FireDamage = 0,
                ColdDamage = 0
            };
        }

        public Vector3 GetPosition()
        {
            return Vector3.zero;
        }
        public void SetPosition(Vector3 pos) { }

        public Vector3 GetRotation()
        {
            return Vector3.zero;
        }

        public void SetRotation(Vector3 rot) { }
    }
}
