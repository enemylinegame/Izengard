using System.Collections.Generic;
using UnitSystem;
using UnitSystem.Enum;
using UnitSystem.View;
using UnityEngine;

namespace BattleSystem
{
    public class EnemyBattleController : BaseBattleController
    {
        private List<IUnit> _enemyUnitCollection;       
        private List<IUnit> _defenderUnitCollection;

        public EnemyBattleController(TargetFinder targetFinder) : base(targetFinder)
        {
            _enemyUnitCollection = new List<IUnit>();
            _defenderUnitCollection = new List<IUnit>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach(var unit in _enemyUnitCollection)
            {
                switch (unit.UnitState.CurrentState)
                {
                    case UnitState.Idle:
                        {
                            if(unit.Stats.Role == UnitRoleType.Hunter)
                            {
                                unit.CurrentTarget = targetFinder.GetMainTowerPosition();
                            }
                            else if (unit.Stats.Role == UnitRoleType.Militiaman)
                            {
                                var target = GetClosestDefender(unit);
                                if(target is StubUnitView)
                                {
                                    unit.CurrentTarget = targetFinder.GetMainTowerPosition();
                                }
                                else
                                {
                                    unit.CurrentTarget = target.SelfTransform.position;
                                }
                            }
                            
                            MoveUnitToTarget(unit, unit.CurrentTarget);
                            unit.UnitState.ChangeState(UnitState.Move);
                            
                            break;
                        }
                    case UnitState.Move:
                        {
                            if (CheckStopDistance(unit, unit.CurrentTarget) == true)
                            {
                                StopUnit(unit);
                            }
                            break;
                        }
                }
            }
        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            foreach (var unit in _enemyUnitCollection)
            {

            }
        }


        public override void AddUnit(IUnit unit) 
        {
            switch (unit.Stats.Faction)
            {
                default:
                    break;
                case UnitFactionType.Enemy:
                    {
                        _enemyUnitCollection.Add(unit);
                        InitUnitLogic(unit);
                        break;
                    }
                case UnitFactionType.Defender:
                    {
                        _defenderUnitCollection.Add(unit);
                        break;
                    }
            }
        }

        private void InitUnitLogic(IUnit unit)
        {
            unit.Navigation.Enable();
            unit.OnReachedZeroHealth += UnitReachedZerohealth;

            unit.UnitState.ChangeState(UnitState.Idle);
        }

        private IUnitView GetClosestDefender(IUnit unit)
        {
            return targetFinder.GetClosestUnit(unit);
        }

        private void MoveUnitToTarget(IUnit unit, Vector3 target)
        {
            unit.Navigation.MoveTo(target);
        }

        private void StopUnit(IUnit unit)
        {
            unit.Navigation.Stop();
        }

        private bool CheckStopDistance(IUnit unit, Vector3 currentTarget)
        {
            var distance = Vector3.Distance(unit.GetPosition(), currentTarget);
            if (distance <= unit.Offence.MaxRange)
            {
                return true;
            }
            return false;
        }

        private void UnitReachedZerohealth(IUnit unit)
        {
            Debug.Log($"Enemy[{unit.Id}]_{unit.Stats.Role} - dead");
        }
    }
}
