using System;
using System.Collections.Generic;
using Enums.BaseUnit;
using UnityEngine;
using UnityEngine.AI;

namespace Views.BaseUnit
{
    public class UnitMovement : MonoBehaviour, IOnStart
    {
        #region Fields

        [SerializeField] private NavMeshAgent _navMeshAgent;
        [NonSerialized] public List<Vector3> PointWhereToGo;
        [NonSerialized] public int CountOfSequence;
        public Action<Vector2> EnterWorkZone = delegate {  };
        public Action<UnitStates> StoppedAtPosition;
        
        #endregion

        #region Properties

        public NavMeshAgent navMeshAgent => _navMeshAgent;

        #endregion

        #region Methods

        private void Start()
        {
            EnterWorkZone += SetPositionInZone;
        }

        private void OnDestroy()
        {
            EnterWorkZone -= SetPositionInZone;
        }
        
        public void OnStart()
        {
            CountOfSequence = 0;
            PointWhereToGo = new List<Vector3>();
        }
        
        public void SetThePointWhereToGo()
        {
            _navMeshAgent.SetDestination(PointWhereToGo[CountOfSequence]);
        }

        public void SetPositionInZone(Vector2 endPosition)
        {
            PointWhereToGo[CountOfSequence] = new Vector3(
                PointWhereToGo[CountOfSequence].x+endPosition.x,
                PointWhereToGo[CountOfSequence].y
                ,PointWhereToGo[CountOfSequence].z+endPosition.y);
            _navMeshAgent.SetDestination(PointWhereToGo[CountOfSequence]);
        }

        public void StopAgent()
        {
            _navMeshAgent.isStopped = true;
        }
        #endregion

     
    }
}