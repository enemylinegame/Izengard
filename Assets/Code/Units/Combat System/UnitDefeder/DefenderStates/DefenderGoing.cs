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
        private Vector3 _destination;
        private float _stopDistanceSqr;
        private bool _avoidFight;

        public Vector3 Destination
        {
            get => _destination;
            set
            {
                _destination = value;
            }
        }

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
                _setState(DefenderState.Idle);
            }
        }

        public override void StartState()
        {
            _agent.ResetPath();
            _agent.SetDestination(_destination);
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