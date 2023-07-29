using System;
using System.Collections;
using UnityEngine;
using Wave;



namespace CombatSystem
{
    public class AttackAction : MonoBehaviour, IAction<Damageable>
    {

        public event Action<Damageable> OnComplete;

        private readonly IEnemyAnimationController _animation;
        private readonly Enemy _unit;
        private Damageable _currentTarget;
        private float cooldownTime = 3.0f;
        private bool isCooldown;



        public AttackAction(IEnemyAnimationController animation, Enemy unit)
        {
            _animation = animation;
            _unit = unit;
        }

        public void StartAction(Damageable target)
        {
            _currentTarget = target;
            Cooldown();
            
            if (!isCooldown)
            {
                _animation.ActionMoment += OnActionMoment;
                _animation.AnimationComplete += OnAnimationComplete;
                
                _animation.PlayAnimation(AnimationType.Attack);
            }
           
  

        }
        
        public void ClearTarget()
        {
            _currentTarget = null;
        }

        private void OnActionMoment()
        {
            if (_currentTarget != null)
            {
                _currentTarget.MakeDamage(_unit.Stats.Attack, _unit.MyDamagable);
            }
            _animation.ActionMoment -= OnActionMoment;
            
        }

        private void OnAnimationComplete()
        {
            _animation.AnimationComplete -= OnAnimationComplete;
            OnComplete?.Invoke(_currentTarget);
        }
        private IEnumerator Cooldown()
        {
            
            isCooldown = true;
            
            yield return new WaitForSeconds(cooldownTime);
            
            isCooldown = false;
        }
    }
}