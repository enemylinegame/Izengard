using Abstraction;
using System;
using Tools;
using UnitSystem.Enum;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace UnitSystem.View
{
    public abstract class BaseUnitView : BaseGameObject, IUnitView
    {
        private string _id;
        protected string _name;
        private UnitType _type;

        protected Transform selfTransform;
        protected NavMeshAgent unitNavigation;
        protected Collider unitCollider;
        protected IUnitAnimationView unitAnimation;

        protected DamageFlash _damageEffect;

        public string Id => _id;
        public string Name => _name;
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

        public abstract void SetUnitName(string name);
        public abstract void ChangeHealth(int hpValue);
        public abstract void ChangeSize(float sizeValue);
        public abstract void ChangeSpeed(float speedValue);


        public void SetCollisionEnabled(bool isEnabled)
        {
            unitCollider.enabled = isEnabled;
        }

        private void Awake()
        {
            _id = GUID.Generate().ToString();

            _damageEffect = GetComponent<DamageFlash>();

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

            if (_damageEffect != null)
                _damageEffect.Flash();
        }
    }
}
