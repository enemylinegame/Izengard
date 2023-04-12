using System;
using System.Collections.Generic;
using Controllers.Worker;
using Enums.BaseUnit;
using UnityEngine;

namespace Controllers.BaseUnit
{
    public class UnitController: IOnController, IOnStart, IDisposable, IOnUpdate, IOnLateUpdate
    {
        

        #region Fields
        
        private List<BaseUnitController> _baseUnitControllers;
        public BaseUnitSpawner BaseUnitSpawner;

        #endregion


        #region UnityMethods

        public UnitController( )
        {
            
            _baseUnitControllers = new List<BaseUnitController>();
        }
        
        public void OnStart()
        {
            BaseUnitSpawner.unitWasSpawned += SetCommandLooking;
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (_baseUnitControllers.Count>0)
            {
                foreach (var baseUnit in _baseUnitControllers)
                {
                    baseUnit.OnUpdate(deltaTime);
                }
            }
        }

        public void OnLateUpdate(float deltaTime)
        {
            if (_baseUnitControllers.Count>0)
            {
                foreach (var baseUnit in _baseUnitControllers)
                {
                    baseUnit.OnLateUpdate(deltaTime);
                }
            }
        }
        public void Dispose()
        {
            BaseUnitSpawner.unitWasSpawned -= SetCommandLooking;
        }

        #endregion


        #region Methods

        private void SetCommandLooking(int id, List<Vector3> listPositions,List<float> listOfTimers)
        {
            List<UnitStates> workerActionsList = new List<UnitStates>();
            workerActionsList.Add(UnitStates.MOVING);
            SetEndPosition(id,listPositions[0]);
            workerActionsList.Add(UnitStates.ATTAKING); //work
            SetEndTime(id,listOfTimers[0]);
            workerActionsList.Add(UnitStates.MOVING);
            SetEndPosition(id,listPositions[1]);
            _baseUnitControllers[id].SetUnitSequence(workerActionsList);
        }

        public List<BaseUnitController> GetBaseUnitController()
        {
            return _baseUnitControllers;
        }

        private void SetEndPosition(int id, Vector3 endpos)
        {
            _baseUnitControllers[id].SetDestination(endpos);
        }

        private void SetEndTime(int id, float time)
        {
            _baseUnitControllers[id].SetEndTime(time);
        }
        #endregion

    }
}