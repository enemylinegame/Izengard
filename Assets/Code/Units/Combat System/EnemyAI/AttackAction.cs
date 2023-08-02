using System;
using System.Collections;
using UnityEngine;
using Wave;



namespace CombatSystem
{
    public class AttackAction : MonoBehaviour, IAction<Damageable>
    {

        public event Action<Damageable> OnComplete;

        //private readonly IEnemyAnimationController _animation;
        private readonly Enemy _unit;
        private Damageable _currentTarget;
        private float cooldownTime = 3.0f;
        private bool isCooldown;



        public AttackAction(Enemy unit)
        {

            _unit = unit;
        }

        public void StartAction(Damageable target)
        {
            _currentTarget = target;
            
            
            if (!isCooldown)
            {


                OnActionMoment();
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
            
            
        }

        private void OnAnimationComplete()
        {
            
            OnComplete?.Invoke(_currentTarget);
        }
       
    }
}