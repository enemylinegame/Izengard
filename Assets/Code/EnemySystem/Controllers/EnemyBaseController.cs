using BattleSystem;
using System;
using System.Collections.Generic;
using UnitSystem;
using UnityEngine;

namespace EnemySystem.Controllers
{
    public abstract class EnemyBaseController : IUnitController, IDisposable
    {
        protected readonly TargetFinder targetFinder;

        protected List<IUnit> unitCollection;
        protected bool isEnable;

        public EnemyBaseController(TargetFinder targetFinder)
        {
            this.targetFinder = targetFinder;

            unitCollection = new List<IUnit>();
            isEnable = true;
        }
        protected bool CheckStopDistance(IUnit unit, Vector3 currentTarget)
        {
            var distance = Vector3.Distance(unit.GetPosition(), currentTarget);
            if (distance <= unit.UnitOffence.MinRange)
            {
                return true;
            }
            return false;
        }

        #region IUnitController

        public abstract event Action<IUnit> OnUnitDone;
        public void AddUnit(IUnit unit)
        {
            InitUnitLogic(unit);
            unitCollection.Add(unit);
        }
        protected abstract void InitUnitLogic(IUnit unit);

        public void RemoveUnit(int unitId)
        {
            var unit = unitCollection.Find(u => u.Id == unitId);
            DeinitUnitLogic(unit);
            unitCollection.Remove(unit);
        }
        protected abstract void DeinitUnitLogic(IUnit unit);
        public void OnUpdate(float deltaTime)
        {
            if (isEnable == false)
                return;

            OnUnitUpdate(deltaTime);
        }

        protected abstract void OnUnitUpdate(float deltaTime);

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            if (isEnable == false)
                return;

            OnUnitFixedUpdate(fixedDeltaTime);
        }

        protected abstract void OnUnitFixedUpdate(float deltaTime);

        #endregion

        #region IDisposable

        public void Dispose()
        {
            OnDispose();
        }

        protected virtual void OnDispose()
        {
            foreach (var unit in unitCollection)
            {
                DeinitUnitLogic(unit);
            }

            unitCollection.Clear();
            isEnable = false;
        }

        #endregion
    }
}
