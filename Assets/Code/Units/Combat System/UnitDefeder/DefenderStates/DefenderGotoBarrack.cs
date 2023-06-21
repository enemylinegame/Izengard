using System;
using UnityEngine;
using UnityEngine.AI;

namespace CombatSystem.DefenderStates
{
    public class DefenderGotoBarrack : DefenderStateBase
    {

        private NavMeshAgent _agent;
        private Transform _transform;
        private Vector3 _barrackPosition;
        private float _stopDistanceSqr;
        
        public Vector3 BarrackPosition
        {
            set => _barrackPosition = value;
        }
        
        public DefenderGotoBarrack(DefenderUnit defenderUnit, Action<DefenderState> setStateDelegate, 
            NavMeshAgent agent) : 
            base(defenderUnit, setStateDelegate)
        {
            _agent = agent;
            _transform = defenderUnit.DefenderGameObject.transform;
            float stopDistance = _agent.stoppingDistance;
            _stopDistanceSqr = stopDistance * stopDistance + float.Epsilon;
        }

        public override void StartState()
        {
            _agent.ResetPath();
            _agent.SetDestination(_barrackPosition);
        }

        public override void OnUpdate()
        {
            Vector3 position = _transform.position;
            position.y = _agent.destination.y;
            if (( position - _agent.destination).sqrMagnitude <= _stopDistanceSqr)
            {
                _agent.ResetPath();
                _setState(DefenderState.InBarrack);
            }
        }

        public override void GoToPosition(Vector3 destination)
        {
            _agent.ResetPath();
            _setState(DefenderState.Going);
        }

        public override void ExitFromBarrack()
        {
            _agent.ResetPath();
            _setState(DefenderState.Idle);
        }
    }
}