using Abstraction;
using System;
using UnitSystem.Enum;
using UnityEngine;
using UnityEngine.AI;

namespace UnitSystem.View
{
    public abstract class BaseUnitView : MonoBehaviour, IUnitView
    {
        private int _id;
        private UnitType _type;

        protected Transform selfTransform;
        protected NavMeshAgent unitNavigation;
        protected Collider unitCollider;
        protected IUnitAnimationView unitAnimation;

        public int Id => _id;
        public UnitType Type => _type;

        public Vector3 Position => selfTransform.position;
        public Transform SelfTransform => selfTransform;
        public NavMeshAgent UnitNavigation => unitNavigation;
        public IUnitAnimationView UnitAnimation => unitAnimation;
 
        public event Action<IDamage> OnTakeDamage;

        public virtual void Init(UnitType type)
        {
            _type = type;
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
            unitCollider.enabled = isEnabled;
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
