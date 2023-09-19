using UnityEngine;
using UnityEngine.AI;

namespace Izengard.UnitSystem
{
    public interface IUnitView
    {
        Transform SelfTransform { get; }
        NavMeshAgent UnitNavigation { get; }

        Animator UnitAnimator { get; }

        void Show();
        void Hide();
        void ChangeHealth(int hpValue);
        void ChangeSize(float sizeValue);
        void ChangeArmor(int armorValue);
        void ChangeSpeed(float speedValue);
    }
}
