using System;
using System.Collections.Generic;
using Abstraction;
using UnityEngine;
using UnitSystem;
using UnitSystem.Enum;
using UnitSystem.Model;

namespace BattleSystem
{
    public class DefenderBattleController : BaseBattleController
    {        
        private class UnitData
        {
            public readonly UnitAttackerModel AttackerModel;
            public readonly IUnit Unit;
            public Vector3 DefendPosition;
            public bool HaveDefendPosition;

            public UnitData(IUnit unit)
            {
                Unit = unit;
                AttackerModel = new UnitAttackerModel(unit);
            }
        }

        private const float MAX_DEFEND_POSITION_ERROR_SQR = 0.1f * 0.1f;

        private List<UnitData> _defenders = new();


        public DefenderBattleController(TargetFinder targetFinder, UnitsContainer unitsContainer) 
            : base(targetFinder, unitsContainer)
        {
        }


        protected override void ExecuteOnUpdate(float deltaTime)
        {
            for (int i = 0; i < _defenders.Count; i++)
            {
                UnitData currentUnit = _defenders[i];
                UnitStateType state = currentUnit.Unit.State.Current;
                switch (state)
                {
                    case UnitStateType.None:
                        break;
                    case UnitStateType.Idle:
                        ExecuteIdleState(currentUnit, deltaTime);
                        break;
                    case UnitStateType.Move:
                        ExecuteMoveState(currentUnit, deltaTime);
                        break;
                    case UnitStateType.Search:
                        break;
                    case UnitStateType.Attack:
                        ExecuteAttackState(currentUnit, deltaTime);
                        break;
                    case UnitStateType.Die:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        protected override void UnitIdleState(IUnit unit, float deltaTime) { }

        protected override void UnitMoveState(IUnit unit, float deltaTime) { }

        protected override void UnitAttackState(IUnit unit, float deltaTime) { }

        protected override void UnitDeadState(IUnit unit, float deltaTime) { }


        private void ExecuteIdleState(UnitData unit, float deltaTime) { }

        private void ExecuteMoveState(UnitData unitData, float deltaTime)
        {
            IAttackTarget target = unitData.Unit.Target.CurrentTarget;
            IUnit defender = unitData.Unit;
            if (target is not NoneTarget)
            {
                Vector3 currentTargetPosition = target.Position;
                float maxAttackRange = defender.Offence.MaxRange;
                
                if ( (defender.GetPosition() - currentTargetPosition).sqrMagnitude <= maxAttackRange * maxAttackRange )
                {
                    defender.Navigation.Stop();
                    ChangeState(unitData, UnitStateType.Attack);
                }
                else
                {
                    defender.Navigation.MoveTo(currentTargetPosition);
                }

            }
            else
            {
                if (unitData.HaveDefendPosition)
                {
                    if ((defender.GetPosition() - unitData.DefendPosition).sqrMagnitude <=
                        MAX_DEFEND_POSITION_ERROR_SQR)
                    {
                        ChangeState(unitData, UnitStateType.Idle);
                    }
                }
                else
                {
                    ChangeState(unitData, UnitStateType.Idle);
                }
            }
        }
        
        private void ExecuteAttackState(UnitData unit, float deltaTime)
        {

        }


        public void SetDefendPosition(IUnit unit, Vector3 position)
        {
            UnitData data = _defenders.Find(unitData => unitData.Unit == unit);
            if (data != null)
            {
                data.DefendPosition = position;
                data.HaveDefendPosition = true;
            }
        }

        private void ChangeState(UnitData unit, UnitStateType newState)
        {
            if (unit.Unit.State.Current != newState)
            {
                Debug.Log("DefenderBattleController->ChangeState: " + unit.Unit.View.SelfTransform.gameObject.name +  
                    " newState = " + newState.ToString() );
                unit.Unit.State.ChangeState(newState);
                if (newState == UnitStateType.Move && 
                    unit.Unit.Target.CurrentTarget is NoneTarget && 
                    unit.HaveDefendPosition) 
                {
                    unit.Unit.Navigation.MoveTo(unit.DefendPosition);
                }
            }
        }

        private void UnitReachedZeroHealth(IUnit unit)
        {
            RemoveUnit(unit);
        }

        public void RemoveUnit(IUnit unit)
        {
            unit.OnReachedZeroHealth -= UnitReachedZeroHealth;
            if (unit.Stats.Faction == UnitFactionType.Defender)
            {
                UnitData unitData = _defenders.Find(u => u.Unit == unit);
                if (unitData != null)
                {
                    //unit.Navigation.Stop();
                    unit.Target.ResetTarget();
                    unit.Disable();
                    _defenders.Remove(unitData);
                    //Debug.Log("DefenderBattleController->RemoveUnit: " + unit.View.SelfTransform.gameObject.name);
                }
            }
            else if (unit.Stats.Faction == UnitFactionType.Enemy)
            {
                _defenders.ForEach(defenderUnitData =>
                {
                    if ( defenderUnitData.Unit.Target.CurrentTarget.Id  == unit.Id)
                    {
                        defenderUnitData.Unit.Target.ResetTarget();
                    }
                });
                
            }
        }

    }
}
