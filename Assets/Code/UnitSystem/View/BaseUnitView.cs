using Abstraction;
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
        
        [SerializeField] private int _id;
        
        public int Id => _id;

        public Vector3 Position => _selfTransform.position;

        public Transform SelfTransform => _selfTransform;
        public NavMeshAgent UnitNavigation => _unitNavigation;
        public IUnitAnimationView UnitAnimation => _unitAnimation;

        public event Action<IDamage> OnTakeDamage;

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
            _id = Math.Abs(gameObject.GetInstanceID());

            SetTransform();
            SetUnitNavigation();
            SetUnitAnimator();
            SetCollision();

            Hide();
        }

        protected abstract void SetTransform();
        protected abstract void SetUnitNavigation();
        protected abstract void SetUnitAnimator();
        protected abstract void SetCollision();

        private void FixedUpdate()
        {
            Debug.DrawRay(transform.position, transform.forward * 1.5f, Color.red, 0); 
        }

        public void TakeDamage(IDamage damage)
        {
            OnTakeDamage?.Invoke(damage);
        }
    }
}
