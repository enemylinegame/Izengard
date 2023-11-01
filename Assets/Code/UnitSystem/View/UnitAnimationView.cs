using UnityEngine;

namespace UnitSystem.View
{
    public class UnitAnimationView : MonoBehaviour, IUnitAnimationView
    {
        [SerializeField] private Animator _animator;

        private readonly int _attackTrigger = Animator.StringToHash("AttackTrigger");
        private readonly int _takeDamageTrigger = Animator.StringToHash("TakeDamage");
        private readonly int _isMovingFlag = Animator.StringToHash("IsMoving");
        private readonly int _unitDeadFlag = Animator.StringToHash("UnitDead");
        
        
        
        public virtual bool IsMoving 
        {
            set
            {
                _animator.SetBool(_isMovingFlag, value);
            }
        }
        
        public virtual void Reset()
        {
            _animator.SetBool(_isMovingFlag, false);
            _animator.SetBool(_unitDeadFlag, false);
        }

        public virtual void StartCast()
        {
            _animator.SetTrigger(_attackTrigger);
        }

        public virtual void StartAttack()
        {
            // Nothing to do, none suitable animation.
        }

        public virtual void StartDead()
        {
            _animator.SetBool(_unitDeadFlag, true);
        }

        public virtual void TakeDamage()
        {
            _animator.SetTrigger(_takeDamageTrigger);
        }
    }
}