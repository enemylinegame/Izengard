using System;
using UnityEngine;
using UnityEngine.AI;

namespace UnitSystem.View
{
    public abstract class BaseUnitView : MonoBehaviour, IUnitView
    {
        protected Transform _selfTransform;
        protected NavMeshAgent _unitNavigation;
        protected Animator _unitAnimator;

        public Transform SelfTransform => _selfTransform;
        public NavMeshAgent UnitNavigation => _unitNavigation;
        public Animator UnitAnimator => _unitAnimator;

        private int _unitId;

        public void Init(int unitId)
        {
            _unitId = unitId;
            Hide();
        }

        public void Show() => 
            gameObject.SetActive(true);
        public void Hide() => 
            gameObject.SetActive(false);

        public abstract void ChangeHealth(int hpValue);
        public abstract void ChangeSize(float sizeValue);
        public abstract void ChangeSpeed(float speedValue);

        private void Awake()
        {
            _unitId = -1;

            SetTransform();
            SetUnitNavigation();
            SetUnitAnimator();
        }

        protected abstract void SetTransform();
        protected abstract void SetUnitNavigation();
        protected abstract void SetUnitAnimator();

        #region ITarget

        public int Id => _unitId;

        public Vector3 Position => _selfTransform.position;

        #endregion

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
