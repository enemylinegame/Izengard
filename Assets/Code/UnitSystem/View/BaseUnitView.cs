using System;
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
        }

        protected abstract void OnSetTransform();
        protected abstract void OnSetUnitNavigation();
        protected abstract void OnSetUnitAnimator();

        #region IFightingObject

        public bool IsFighting { get; protected set; }

        public event Action OnPulledInFight;
        public event Action OnReleasedFromFight;

        public void PullIntoFight()
        {
            OnPulledInFight?.Invoke();
            IsFighting = true;
            Debug.Log($"{gameObject.name} pulled into fight");
        }

        public void ReleaseFromFight()
        {
            OnReleasedFromFight?.Invoke();
            IsFighting = false;
            Debug.Log($"{gameObject.name} Released from fight");
        }

        #endregion
    }
}
