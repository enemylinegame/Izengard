using System;
using System.Collections.Generic;
using Enums.BaseUnit;
using Models.BaseUnit;
using UnityEngine;
using UnityEngine.AI;
using Views.BaseUnit;

namespace Controllers.BaseUnit
{
    public class BaseUnitController: IOnController,IOnStart, IOnUpdate, IOnLateUpdate, IDisposable
    {
        #region Fields
        
        private BaseUnitModel _unitModel;
        private UnitMovement _unitMovementView;
        private UnitAnimation _unitAnimation;
        public IUnitHandler CurrentUnitHandler;
        public int MoveCounter;
        private float normalizedTime = 0.0f;

        #endregion

        #region Ctor

        public BaseUnitController(BaseUnitModel baseUnitModel, UnitMovement unitMovement, UnitAnimation unitAnimation)
        {
            _unitModel = baseUnitModel;
            _unitMovementView =  unitMovement;
            _unitAnimation = unitAnimation;
        }

        #endregion


        #region Interfaces

        public void OnStart()
        {
            _unitMovementView.OnStart();
            _unitMovementView.StoppedAtPosition += SetStateMachine;
        }

        public void OnUpdate(float deltaTime)
        {
            Check(deltaTime);
        }
        
        public void OnLateUpdate(float deltaTime)
        {
            
        }

        public void Dispose()
        {
            _unitMovementView.StoppedAtPosition -= SetStateMachine;
        }
        
        #endregion


        #region Methods

        public virtual void SetStateMachine(UnitStates unitStates) { }

        public virtual void SetDestination(Vector3 whereToGo) { }
        
        public virtual void SetEndTime(float time){}
        
        public virtual void SetUnitSequence(List<UnitStates> workerActionsList){}

        public virtual void Check(float deltaTime) { }
    
        public void NormalSpeed(NavMeshAgent agent,Vector3 endPos,float deltaTime)
        {
            if (agent.transform.position != endPos)
                agent.transform.position =
                    Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * deltaTime);
            else
                agent.CompleteOffMeshLink();
        }

        #endregion
    }
}