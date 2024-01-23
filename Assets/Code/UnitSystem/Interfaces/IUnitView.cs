using Abstraction;
using UnitSystem.Enum;
using UnityEngine;
using UnityEngine.AI;

namespace UnitSystem
{
    public interface IUnitView : IAttackTarget
    {
        UnitType Type { get; }

        Transform SelfTransform { get; }
        NavMeshAgent UnitNavigation { get; }

        IUnitAnimationView UnitAnimation { get; }

        void Init(UnitType type);

        void Show();
        void Hide();
        void ChangeHealth(int hpValue);
        void ChangeSize(float sizeValue);
        void ChangeSpeed(float speedValue);
        void SetCollisionEnabled(bool isEnabled);
        void SetUnitName(string name);
    }
}
