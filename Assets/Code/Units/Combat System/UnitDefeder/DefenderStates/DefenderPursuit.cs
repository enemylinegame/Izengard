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
        
        public DefenderPursuit(DefenderUnit defenderUnit, Action<DefenderState> setStateDelegate, 
            NavMeshAgent agent, DefenderTargetSelector selector, DefenderTargetsHolder holder) : 
            base(defenderUnit, setStateDelegate)
        {
            _agent = agent;
            _targetSelector = selector;
            _targetsHolder = holder;
        }


        public override void OnUpdate()
        {
            var target = _targetsHolder.CurrentTarget;
            if (target == null || target.IsDead)
            {
                target = _targetSelector.SelectTarget();
            }

            if (target != null)
            {
                if (_targetSelector.IsTargetInRange(target))
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
            else
            {
                _agent.ResetPath();
                _setState(DefenderState.Going);
            }
        }

        public override void StartState()
        {
            _agent.ResetPath();
        }

        public override void OnDamaged(IDamageable attacker)
        {
            _targetSelector.SelectTarget();
        }

        public override void AddedTargetInRange(IDamageable target)
        {
            _targetSelector.SelectTarget();
        }

        public override void GoToPosition(Vector3 destination)
        {
            _agent.ResetPath();
            _setState(DefenderState.Going);
        }
    }
}