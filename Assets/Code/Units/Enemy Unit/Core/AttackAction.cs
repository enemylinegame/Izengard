using System;
using CombatSystem;
using UnityEngine;

namespace EnemyUnit.Core
{
    public class AttackAction : MonoBehaviour, IAction<Damageable>
    {
        public event Action<Damageable> OnComplete;

        private readonly IEnemyAnimationController _animation;
        private readonly EnemyModel _model;
        private readonly EnemyView _view;
        private Damageable _currentTarget;

        public AttackAction(
            EnemyModel model,
            EnemyView view,
            IEnemyAnimationController animation)
        {
            _model = model;
            _view = view;
            _animation = animation;
        }

        public void StartAction(Damageable target)
        {
            _currentTarget = target;
            
            _animation.ActionMoment += OnActionMoment;
            _animation.AnimationComplete += OnAnimationComplete;
            
            _animation.PlayAnimation(AnimationType.Attack);
        }
        
        public void ClearTarget()
        {
            _currentTarget = null;
        }

        private void OnActionMoment()
        {
            if (_currentTarget != null)
            {
                _currentTarget.MakeDamage(_model.Stats.Attack, _view);
            }
            _animation.ActionMoment -= OnActionMoment;
        }

        private void OnAnimationComplete()
        {
            _animation.AnimationComplete -= OnAnimationComplete;
            OnComplete?.Invoke(_currentTarget);
        }
    }
}