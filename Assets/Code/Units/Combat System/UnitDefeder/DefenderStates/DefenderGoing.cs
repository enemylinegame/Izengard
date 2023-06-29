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
        private bool _isDestinationChanged;

        
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
            if (_isDestinationChanged)
            {
                ChangeDestination();
            }
            // Vector3 position = _transform.position;
            // position.y = _agent.destination.y;
            // if (( position - _agent.destination).sqrMagnitude <= _stopDistanceSqr)
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                _setState(DefenderState.Idle);
            }
        }

        public override void StartState()
        {
            _isDestinationChanged = true;
        }

        public override void OnDamaged(IDamageable attacker)
        {
            if (!_avoidFight)
            {
                _agent.ResetPath();
                _setState(DefenderState.Pursuit);
            }
        }

        public override void GoToBarrack(Vector3 destination)
        {
            _agent.ResetPath();
            _setState(DefenderState.GotoBarrack);
        }

        public override void GoToPosition(Vector3 destination)
        {
             _isDestinationChanged = true;
        }

        private void ChangeDestination()
        {
             _agent.ResetPath();
             _agent.SetDestination(_defenderUnit.DefendPosition);
             _isDestinationChanged = false;
        }
    }
}