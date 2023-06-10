using System;
using CombatSystem.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace CombatSystem.DefenderStates
{
    public sealed class DefenderGoing : DefenderStateBase
    {
        private DefenderUnitStats _stats;
        private NavMeshAgent _agent;
        private Transform _transform;
        private Rigidbody _rigidbody;
        private float _stopDistanceSqr;
        private bool _avoidFight;

        public bool AvoidFight
        {
            get => _avoidFight; 
            set => _avoidFight = value;
        }
        
        public DefenderGoing(DefenderUnit defenderUnit, Action<DefenderState> setStateDelegate, 
            DefenderUnitStats stats, NavMeshAgent agent) : 
            base(defenderUnit, setStateDelegate)
        {
            _stats = stats;
            _agent = agent;
            _transform = defenderUnit.DefenderGameObject.transform;
            _rigidbody = _transform.GetComponent<Rigidbody>();
            float stopDistance = _agent.stoppingDistance;
            _stopDistanceSqr = stopDistance * stopDistance + float.Epsilon;
        }

        public override void OnUpdate()
        {
            Vector3 position = _transform.position;
            position.y = _agent.destination.y;
            if (( position - _agent.destination).sqrMagnitude <= _stopDistanceSqr)
            {
                _agent.ResetPath();
                //_rigidbody.velocity = Vector3.zero;
                _setState(DefenderState.Idle);
            }
            // else
            // {
            //     _agent.ResetPath();
            //     _agent.SetDestination(_defenderUnit.DefendPosition);
            // }
        }

        public override void StartState()
        {
            _agent.ResetPath();
            _agent.SetDestination(_defenderUnit.DefendPosition);
        }

        public override void OnDamaged(IDamageable attacker)
        {
            if (!_avoidFight)
            {
                _agent.ResetPath();
                _setState(DefenderState.Fight);
            }
        }

        public override void GoToBarrack(Vector3 destination)
        {
            _agent.ResetPath();
            _setState(DefenderState.GotoBarrack);
        }
        
        
    }
}