using UnityEngine;
using UnityEngine.AI;

namespace Izengard.UnitSystem.View
{
    public abstract class BaseUnitView : MonoBehaviour, IUnitView
    {
        protected Transform _selfTransform;
        protected NavMeshAgent _unitNavigation;
        protected Animator _unitAnimator;

        public Transform SelfTransform => _selfTransform;
        public NavMeshAgent UnitNavigation => _unitNavigation;
        public Animator UnitAnimator => _unitAnimator;

        public void Show() => 
            gameObject.SetActive(true);
        public void Hide() => 
            gameObject.SetActive(false);

        public abstract void ChangeHealth(int hpValue);
        public abstract void ChangeSize(float sizeValue);
        public abstract void ChangeSpeed(float speedValue);

        private void Awake()
        {
            OnSetTransform();
            OnSetUnitNavigation();
            OnSetUnitAnimator();

            Hide();
        }

        protected abstract void OnSetTransform();
        protected abstract void OnSetUnitNavigation();
        protected abstract void OnSetUnitAnimator();
    }
}
