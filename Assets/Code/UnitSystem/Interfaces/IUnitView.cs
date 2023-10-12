using Abstraction;
using UnityEngine;
using UnityEngine.AI;

namespace UnitSystem
{
    public interface IUnitView : IFightingObject, ITarget
    {
        Transform SelfTransform { get; }
        NavMeshAgent UnitNavigation { get; }

        Animator UnitAnimator { get; }

        void Init(int unitId);

        void Show();
        void Hide();
        void ChangeHealth(int hpValue);
        void ChangeSize(float sizeValue);
        void ChangeSpeed(float speedValue);
 
    }
}
