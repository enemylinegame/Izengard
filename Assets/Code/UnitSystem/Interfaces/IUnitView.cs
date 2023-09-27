using Izengard.Abstraction.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Izengard.UnitSystem
{
    public interface IUnitView : IFightingObject
    {
        Transform SelfTransform { get; }
        NavMeshAgent UnitNavigation { get; }

        Animator UnitAnimator { get; }

        void Show();
        void Hide();
        void ChangeHealth(int hpValue);
        void ChangeSize(float sizeValue);
        void ChangeSpeed(float speedValue);
 
    }
}
