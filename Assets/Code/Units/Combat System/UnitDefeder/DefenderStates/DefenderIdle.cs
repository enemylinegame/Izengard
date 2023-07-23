using System;
using CombatSystem.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace CombatSystem.DefenderStates
{
    public class DefenderIdle : DefenderStateBase
    {

        private Transform _transform;
        private NavMeshAgent _agent; 
        private float _stopDistanceSqr;
        
        public DefenderIdle(DefenderUnit defenderUnit, Action<DefenderState> setStateDelegate, 
            NavMeshAgent agent) : 
            base(defenderUnit, setStateDelegate)
        {
            _transform = defenderUnit.DefenderGameObject.transform;
            _agent = agent;
            float stopDistance = _agent.stoppingDistance;
            _stopDistanceSqr = stopDistance * stopDistance + float.Epsilon;
        }

        public override void OnDamaged(IDamageable attacker)
        {
            _setState(DefenderState.Pursuit);
        }

        public override void AddedTargetInRange()
        {
            _setState(DefenderState.Pursuit);
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