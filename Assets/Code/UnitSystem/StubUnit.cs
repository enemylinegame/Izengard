using Abstraction;
using UnitSystem.View;
using UnityEngine;

namespace UnitSystem
{
    public class StubUnit : IUnit
    {
        public int Id => -1;

        public IUnitView UnitView { get; private set; }

        public UnitStatsModel UnitStats { get; private set; }

        public INavigation<Vector3> Navigation { get; }

        public IUnitDefence UnitDefence { get; }

        public IUnitOffence UnitOffence { get; }

        public UnitStateModel UnitState { get; }

        public StubUnit(string logMessage)
        {
            UnitView = new StubUnitView();
            Debug.LogWarning(logMessage);
        }

        public void Disable() { }

        public void Enable() { }
        public void TakeDamage(IDamage damageValue) { }

        public IDamage GetAttackDamage()
        {
            return new DamageStructure
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
