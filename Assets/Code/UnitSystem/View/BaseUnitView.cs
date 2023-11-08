using System;
using UnityEngine;
using UnityEngine.AI;

namespace UnitSystem.View
{
    public abstract class BaseUnitView : MonoBehaviour, IUnitView
    {
        protected Transform _selfTransform;
        protected NavMeshAgent _unitNavigation;
        protected Collider _unitCollider;
        protected IUnitAnimationView _unitAnimation;

        public Transform SelfTransform => _selfTransform;
        public NavMeshAgent UnitNavigation => _unitNavigation;
        public IUnitAnimationView UnitAnimation => _unitAnimation;

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

        public void SetCollisionEnabled(bool isEnabled)
        {
            _unitCollider.enabled = isEnabled;
        }
        
        private void Awake()
        {
            _unitId = -1;

            SetTransform();
            SetUnitNavigation();
            SetUnitAnimator();
            SetCollision();
        }

        protected abstract void SetTransform();
        protected abstract void SetUnitNavigation();
        protected abstract void SetUnitAnimator();
        protected abstract void SetCollision();

        private void FixedUpdate()
        {
            Debug.DrawRay(transform.position, transform.forward * 1, Color.red, 0); 
        }

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
