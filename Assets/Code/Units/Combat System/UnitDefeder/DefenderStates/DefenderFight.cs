using System;
using System.Collections.Generic;
using CombatSystem.Interfaces;
using UnityEngine;

namespace CombatSystem.DefenderStates
{
    public class DefenderFight : DefenderStateBase
    {

        public event Action OnStartAttack; 

        protected DefenderUnitStats _stats;
        private DefenderTargetsHolder _targetsHolder;
        private DefenderTargetSelector _targetSelector;
        private DefenderTargetFinder _targetFinder;
        protected IDamageable _myDamagable;

        private float _reloadTime;
        private float _reloadTimeCounter;

        private bool _isAttackReady;
        
        public DefenderFight(DefenderUnit defenderUnit, Action<DefenderState> setStateDelegate, 
            DefenderUnitStats stats, DefenderTargetsHolder holder, DefenderTargetSelector selector, 
            IDamageable myDamageable, DefenderTargetFinder finder) : 
            base(defenderUnit, setStateDelegate)
        {
            _stats = stats;
            _reloadTime = _stats.AttackInterval;
            _targetsHolder = holder;
            _targetSelector = selector;
            _myDamagable = myDamageable;
            _targetFinder = finder;
        }

        public void Reload()
        {
            if (!_isAttackReady)
            {
                _reloadTimeCounter += Time.deltaTime;
                if (_reloadTimeCounter >= _reloadTime)
                {
                    _reloadTimeCounter = 0.0f;
                    _isAttackReady = true;
                }
            }
        }

        public override void OnUpdate()
        {
            IDamageable target = _targetsHolder.CurrentTarget;
            if (target == null || target.IsDead)
            {
                target = _targetSelector.SelectTarget();
            }

            if (target != null)
            {
                if (_targetFinder.IsTargetInRange(target))
                {
                    if (_isAttackReady)
                    {
                        _isAttackReady = false;
                        AttackTarget(target);
                    }
                }
                else
                {
                    _setState(DefenderState.Pursuit);
                }
            }
            else
            {
                _setState(DefenderState.Going);
            }
        }

        protected virtual void AttackTarget(IDamageable target)
        {
            OnStartAttack?.Invoke();
            target.MakeDamage(_stats.AttackDamage, _myDamagable);
        }

        public override void GoToPosition(Vector3 destination)
        {
            _setState(DefenderState.Going);
        }

        public override void GoToBarrack(Vector3 destination)
        {
            _setState(DefenderState.GotoBarrack);
        }

        public override void StartState()
        {
            _targetSelector.SelectTarget();
        }
    }
}