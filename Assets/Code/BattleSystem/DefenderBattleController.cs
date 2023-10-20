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
        
        private class TargetData : IAttackTarget
        {
            public IUnit Unit;

            public void TakeDamage(IDamage damage)
            {
                Unit.TakeDamage(damage);
            }

            public Vector3 GetPosition()
            {
                return Unit.View.SelfTransform.position;
            }
        }

        private const float MAX_DEFEND_POSITION_ERROR_SQR = 0.1f * 0.1f;

        private readonly IRegularAttackController _regularAttackController; 
            
        private List<UnitData> _defenders = new();
        private List<TargetData> _enemies = new();


        public DefenderBattleController(TargetFinder targetFinder, IRegularAttackController regularAttackController) 
            : base(targetFinder)
        {
            _regularAttackController = regularAttackController;
        }

        public override void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < _defenders.Count; i++)
            {
                UnitData currentUnit = _defenders[i];
                UnitState state = currentUnit.Unit.UnitState.CurrentState;
                switch (state)
                {
                    case UnitState.None:
                        break;
                    case UnitState.Idle:
                        ExecuteIdleState(currentUnit, deltaTime);
                        break;
                    case UnitState.Move:
                        ExecuteMoveState(currentUnit, deltaTime);
                        break;
                    case UnitState.Search:
                        break;
                    case UnitState.Attack:
                        ExecuteAttackState(currentUnit, deltaTime);
                        break;
                    case UnitState.Die:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            
        }

        public override void AddUnit(IUnit unit)
        {
            if (unit != null)
            {
                if (unit.Stats.Faction == UnitFactionType.Defender)
                {
                    if (!_defenders.Exists(unitData => unitData.Unit == unit))
                    {
                        _defenders.Add( new UnitData(unit));
                        unit.Enable();
                        unit.Navigation.Enable();
                        unit.OnReachedZeroHealth += UnitReachedZeroHealth;
                        Debug.Log("DefenderBattleController->AddUnit: " + unit.View.SelfTransform.gameObject.name + 
                                  " UnitState = " + unit.UnitState.CurrentState.ToString());
                    }
                }
                else if (unit.Stats.Faction == UnitFactionType.Enemy)
                {
                    if (!_enemies.Exists(data => data.Unit == unit))
                    {
                        _enemies.Add( new TargetData() {Unit = unit} );
                        unit.OnReachedZeroHealth += UnitReachedZeroHealth;
                    }
                }

            }
        }

        private void ExecuteIdleState(UnitData unit, float deltaTime)
        {
            ITarget foundTarget = targetFinder.GetClosestUnit(unit.Unit);
            if (foundTarget is not NoneTarget)
            {
                //IUnit target = GetUnitByView(foundTarget);
 
                if (foundTarget.Id > 0)
                {
                    unit.Unit.Target.SetTarget(foundTarget);

                    TargetData target = _enemies.Find(u => u.Unit.Id == foundTarget.Id);
                    unit.AttackerModel.SetTarget(target);
                    ChangeState(unit, UnitState.Move);
                }
                
            }
        }

        private void ExecuteMoveState(UnitData unitData, float deltaTime)
        {
            ITarget target = unitData.Unit.Target.CurrentTarget;
            IUnit defender = unitData.Unit;
            if (target is not NoneTarget)
            {
                Vector3 currentTargetPosition = target.Position;
                float maxAttackRange = defender.Offence.MaxRange;
                
                if ( (defender.GetPosition() - currentTargetPosition).sqrMagnitude <= maxAttackRange * maxAttackRange )
                {
                    defender.Navigation.Stop();
                    ChangeState(unitData, UnitState.Attack);
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
                        ChangeState(unitData, UnitState.Idle);
                    }
                }
                else
                {
                    ChangeState(unitData, UnitState.Idle);
                }
            }
        }
        
        private void ExecuteAttackState(UnitData unit, float deltaTime)
        {
            _regularAttackController.AddUnit(unit.AttackerModel);
        }

        // private IUnit GetUnitByView(BaseUnitView targetView)
        // {
        //     IUnit target = null;
        //     IUnitView view = targetView;
        //
        //     if (targetView != null)
        //     {
        //         for (int i = 0; i < _enemies.Count; i++)
        //         {
        //             IUnitView current = _enemies[i].View;
        //             if ( current == view)
        //             {
        //                 target = _enemies[i];
        //                 break;
        //             }
        //         }
        //     }
        //
        //     return target;
        // }

        public void SetDefendPosition(IUnit unit, Vector3 position)
        {
            UnitData data = _defenders.Find(unitData => unitData.Unit == unit);
            if (data != null)
            {
                data.DefendPosition = position;
                data.HaveDefendPosition = true;
            }
        }

        private void ChangeState(UnitData unit, UnitState newState)
        {
            if (unit.Unit.UnitState.CurrentState != newState)
            {
                Debug.Log("DefenderBattleController->ChangeState: " + unit.Unit.View.SelfTransform.gameObject.name +  
                    " newState = " + newState.ToString() );
                unit.Unit.UnitState.ChangeState(newState);
                if (newState == UnitState.Move && 
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
                    _regularAttackController.RemoveUnit(unitData.AttackerModel);
                    //unit.Navigation.Stop();
                    unit.Target.ResetTarget();
                    unit.Disable();
                    _defenders.Remove(unitData);
                    Debug.Log("DefenderBattleController->RemoveUnit: " + unit.View.SelfTransform.gameObject.name);
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
                
                int index = _enemies.FindIndex(data=> data.Unit == unit);
                _enemies.RemoveAt(index);
            }
        }
        
    }
}
