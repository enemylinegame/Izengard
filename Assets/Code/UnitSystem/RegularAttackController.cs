using System;
using System.Collections.Generic;
using UnityEngine;
using UnitSystem.Enum;
using Abstraction;
using UnitSystem.Model;


namespace UnitSystem
{
    public class RegularAttackController : IRegularAttackController, IOnController, IOnUpdate
    {
        private class AttackTimingModel
        {
            public IAttacker Unit;
            public float Progress;
            public float CastTime;
            public float AttackTime;
            public AttackTimingState TimingState;
        }
        
        private readonly List<AttackTimingModel> _reloadDelays = new();
        
        
        public void OnUpdate(float deltaTime)
        {
            for (int index = 0; index < _reloadDelays.Count; index++)
            {
                AttackTimingModel model = _reloadDelays[index];

                switch (model.TimingState)
                {
                    case AttackTimingState.None:

                        if (CheckIsDistanceToTargetSuitable(model.Unit))
                        {
                            StartCast(model);
                        }
                        
                        break;
                    
                    case AttackTimingState.Cast:
                        model.Progress += Time.deltaTime;

                        if (model.Progress >= model.CastTime)
                        {
                            model.Progress = 0.0f;
                            StartAttack(model);
                        }
                        
                        break;
                        
                    case AttackTimingState.Attack:

                        model.Progress += Time.deltaTime;

                        if (model.Progress >= model.AttackTime)
                        {
                            model.Progress = 0.0f;
                            FinishAttack(model);
                        }
                        
                        break;
                }
            }
        }


        private void StartCast(AttackTimingModel timingModel)
        {
            timingModel.Progress = 0.0f;
            timingModel.TimingState = AttackTimingState.Cast;
            timingModel.Unit.StartCast();
        }

        private void StartAttack(AttackTimingModel timingModel)
        {
            timingModel.Unit.StartAutoAttack();
            timingModel.TimingState = AttackTimingState.Attack;
        }

        private bool CheckIsDistanceToTargetSuitable(IAttacker unit)
        {
            IAttackTarget target = unit.GetCurrentTarget();
            if (target == null) return false;
            
            float distanceSqr = (unit.GetPosition() - target.GetPosition()).sqrMagnitude;
            float minDistance = unit.GetMinAttackDistance();
            float maxDistance = unit.GetMaxAttackDistance();

            return (distanceSqr >= minDistance * minDistance) &&
                   (distanceSqr <= maxDistance * maxDistance);
        }

        private void FinishAttack(AttackTimingModel timingModel)
        {
            IAttackTarget target = timingModel.Unit.GetCurrentTarget();

            if (target != null)
            {
                IDamage damage = timingModel.Unit.GetDamagePower();
                target.TakeDamage(damage);
            }

            timingModel.TimingState = AttackTimingState.None;
        }



        #region IRegularAttackController

        public void AddUnit(IAttacker unit)
        {
            if (unit == null) return;

            AttackTimingModel delay = _reloadDelays.Find(u => u.Unit == unit);
            if (delay == null)
            {
                delay = new AttackTimingModel()
                {
                    Unit = unit,
                    Progress = 0.0f,
                    CastTime = unit.GetCastTime(),
                    AttackTime = unit.GetAttackTime(),
                    TimingState = AttackTimingState.Cast
                };
                
                _reloadDelays.Add(delay);

                unit.OnUnityDestroyed += RemoveUnit;
                unit.OnTargetChanged += UnitTargetChanged;
            }

        }

        public void RemoveUnit(IAttacker unit)
        {
            AttackTimingModel timingModel = _reloadDelays.Find(u => u.Unit == unit);
            if (timingModel != null)
            {
                RemoveDelayModel(timingModel);
            }
        }

        #endregion

        private void RemoveDelayModel(AttackTimingModel timingModel)
        {
            timingModel.Unit.OnUnityDestroyed -= RemoveUnit;
            timingModel.Unit.OnTargetChanged -= UnitTargetChanged;
            _reloadDelays.Remove(timingModel);
        }

        private void UnitTargetChanged(IAttacker unit)
        {
            AttackTimingModel timingModel = _reloadDelays.Find(u => u.Unit == unit);
            if (timingModel != null)
            {
                timingModel.TimingState = AttackTimingState.None;
                timingModel.Progress = 0.0f;
            }
        }

    }
}