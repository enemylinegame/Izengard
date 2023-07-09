using System;
using CombatSystem.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace CombatSystem.DefenderStates
{
    public class DefenderPursuit : DefenderStateBase
    {

        private NavMeshAgent _agent;
        private DefenderTargetSelector _targetSelector;
        private DefenderTargetsHolder _targetsHolder;
        private DefenderTargetFinder _targetFinder;
        
        public DefenderPursuit(DefenderUnit defenderUnit, Action<DefenderState> setStateDelegate, 
            NavMeshAgent agent, DefenderTargetSelector selector, DefenderTargetsHolder holder, 
            DefenderTargetFinder finder) : 
            base(defenderUnit, setStateDelegate)
        {
            _agent = agent;
            _targetSelector = selector;
            _targetsHolder = holder;
            _targetFinder = finder;
        }


        public override void OnUpdate()
        {
            var target = _targetsHolder.CurrentTarget;
            if (target == null || target.IsDead)
            {
                target = _targetSelector.SelectTarget();
            }

            if (target == null)
            {
                _agent.ResetPath();
                _setState(DefenderState.Going);
            }
            else if (target.IsDead)
            {
                _targetsHolder.CurrentTarget = null;
                _agent.ResetPath();
                _setState(DefenderState.Going);
            }
            else
            {
                if (_targetFinder.IsTargetInRange(target))
                {
                    _agent.ResetPath();
                    _setState(DefenderState.Fight);
                }
                else
                {
                    _agent.ResetPath();
                    _agent.SetDestination(target.Position);
                }
            }

        }

        public override void StartState()
        {
            _agent.ResetPath();
            if (_targetsHolder.CurrentTarget == null && _targetsHolder.AttackingTargets.Count > 0)
            {
                _targetsHolder.CurrentTarget = _targetsHolder.AttackingTargets[0];
            }
        }

        // public override void OnDamaged(IDamageable attacker)
        // {
        //     _targetSelector.SelectTarget();
        // }
        
        // public override void AddedTargetInRange(IDamageable target)
        // {
        //     _targetSelector.SelectTarget();
        // }

        public override void GoToPosition(Vector3 destination)
        {
            _agent.ResetPath();
            _setState(DefenderState.Going);
        }
    }
}