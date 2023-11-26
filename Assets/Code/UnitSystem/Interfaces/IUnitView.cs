using Abstraction;
using UnityEngine;
using UnityEngine.AI;

namespace UnitSystem
{
    public interface IUnitView : IAttackTarget
    {
        Transform SelfTransform { get; }
        NavMeshAgent UnitNavigation { get; }

        IUnitAnimationView UnitAnimation { get; }

        void Show();
        void Hide();
        void ChangeHealth(int hpValue);
        void ChangeSize(float sizeValue);
        void ChangeSpeed(float speedValue);
        void SetCollisionEnabled(bool isEnabled);
    }
}
