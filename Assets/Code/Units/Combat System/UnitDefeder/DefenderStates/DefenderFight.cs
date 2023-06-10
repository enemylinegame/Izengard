using System;
using System.Collections.Generic;
using CombatSystem.Interfaces;
using UnityEngine;

namespace CombatSystem.DefenderStates
{
    public sealed class DefenderFight : DefenderStateBase
    {

        private DefenderUnitStats _stats;
        //private DefenderTargetsHolder _targetsHolder;
        private DefenderTargetSelector _targetSelector;
        private IDamageable _myDamagable;

        private float _reloadTime;
        private float _reloadTimeCounter;

        private bool _isAttackReady;
        
        public DefenderFight(DefenderUnit defenderUnit, Action<DefenderState> setStateDelegate, 
            DefenderUnitStats stats, DefenderTargetsHolder holder, DefenderTargetSelector selector, 
            IDamageable myDamagable) : 
            base(defenderUnit, setStateDelegate)
        {
            _stats = stats;
            _reloadTime = _stats.AttackInterval;
            //_targetsHolder = holder;
            _targetSelector = selector;
            _myDamagable = myDamagable;
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
            IDamageable target = _targetSelector.SelectTarget();
            if (_targetSelector.IsTargetInRange(target))
            {
                AttackTarget(target);
            }
            else
            {
                _setState(DefenderState.Pursuit);
            }
        }

        private void AttackTarget(IDamageable target)
        {
            if (_isAttackReady)
            {
                target.MakeDamage(_stats.AttackDamage, _myDamagable);
            }
        }

        public override void GoToPosition(Vector3 destination)
        {
            _setState(DefenderState.Going);
        }

        public override void GoToBarrack(Vector3 destination)
        {
            _setState(DefenderState.GotoBarrack);
        }
        
    }
}