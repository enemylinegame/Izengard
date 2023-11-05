using Abstraction;
using BattleSystem.Buildings;
using BattleSystem.Models;
using System.Collections.Generic;
using UnitSystem;
using UnitSystem.Enum;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace BattleSystem
{
    public class EnemyTestBattleController : FifthBattleController
    {
        private readonly ObstacleController _obstacleController;

        public EnemyTestBattleController(
            TargetFinder targetFinder, 
            ObstacleController obstacleController) : base(targetFinder)
        {
            _obstacleController = obstacleController;
        }


        protected override void ExecuteUnitUpdate(IUnit unit, float deltaTime)
        {
            UpdateTargetExistence(unit);

            CheckForObstacle(unit);

            switch (unit.UnitState.CurrentState)
            {
                default:
                    break;

                case UnitState.Idle:
                    {
                        UnitIdleState(unit, deltaTime);
                        break;
                    }
                case UnitState.Move:
                    {
                        UnitMoveState(unit, deltaTime);
                        break;
                    }
                case UnitState.Approach:
                    {
                        UnitApproachState(unit, deltaTime);
                        break;
                    }
                case UnitState.Search:
                    {
                        break;
                    }
                case UnitState.Attack:
                    {
                        UnitAttackState(unit, deltaTime);
                        break;
                    }
                case UnitState.Die:
                    {
                        break;
                    }
            }
        }

        protected override void UpdateTargetExistence(IUnit unit)
        {
            switch (unit.Priority.Current.Priority)
            {
                case UnitPriorityType.MainTower:
                    {
                        var mainTowerTarget = targetFinder.GetMainTower();
                        if(unit.Target.CurrentTarget.Id != mainTowerTarget.Id)
{
                            unit.Target.SetTarget(mainTowerTarget);
                        }
                        break;
                    }
                case UnitPriorityType.ClosestFoe:
                case UnitPriorityType.FarthestFoe:
                case UnitPriorityType.SpecificFoe:
                    {
                        List<IUnit> list = (unit.Stats.Faction == UnitFactionType.Enemy) ? _defenderUnits : _enemyUnits;

                        IAttackTarget target = unit.Target.CurrentTarget;
                        if (!list.Exists(def => def.Id == target.Id))
                        {
                            unit.Target.ResetTarget();

                            ChangeUnitState(unit, UnitState.Idle);
                        }
                        break;
                    }
            }
        }

        private void CheckForObstacle(IUnit unit)
        {
            var obstacles = _obstacleController.ObstaclesCollection;

            if(obstacles.Count > 0)
            {
                var unitPos = unit.GetPosition();
                var targetPos = unit.Target.CurrentTarget.Position;

                Vector3 direction = targetPos - unitPos;

                for (int i =0; i < obstacles.Count; i++)
                {
                    var obstacle = obstacles[i];
                    var obstaclePos = obstacle.View.Position;

                    float dotProduct = Vector3.Dot(obstaclePos - unitPos, direction);
                    float sqrDistance = Vector3.SqrMagnitude(obstaclePos - unitPos);

                    if (dotProduct > 0f && dotProduct * dotProduct >= sqrDistance)
                    {
                        var target = new TargetModel(obstacle, obstacle.View);
                        unit.Target.SetTarget(target);
                    }
                }
            }
        }
    }
}
