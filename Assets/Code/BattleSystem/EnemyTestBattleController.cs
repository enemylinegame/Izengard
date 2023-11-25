using Abstraction;
using BattleSystem.Buildings;
using BattleSystem.Buildings.Interfaces;
using BattleSystem.Models;
using System;
using System.Collections.Generic;
using Tools;
using UnitSystem;
using UnitSystem.Enum;
using UnityEngine;
using NoneTarget = Abstraction.NoneTarget;

namespace BattleSystem
{
    [Obsolete]
    public class EnemyTestBattleController : BaseBattleController
    {
        private enum AttackPhase
        {
            None = 0,
            Cast,
            Attack
        }

        private class AttackModel
        {
            public IUnit Attacker;
            public float TimingProgress;
            public AttackPhase Phase;
        }

        private class DeadUnit
        {
            public IUnit Unit;
            public float TimeLeft;

            public DeadUnit(IUnit unit)
            {
                Unit = unit;
                TimeLeft = DEAD_UNITS_DESTROY_DELAY;
            }
        }

        private const float DESTINATION_POSITION_ERROR_SQR = 0.3f * 0.3f;
        private const float DEAD_UNITS_DESTROY_DELAY = 10.0f;

        private List<AttackModel> _attackModels = new();
        private List<DeadUnit> _deadUnits = new();

        private readonly ObstacleController _obstacleController;

        private TimeRemaining _timer;

        public EnemyTestBattleController(
            TargetFinder targetFinder, 
            UnitsContainer unitsContainer,
            ObstacleController obstacleController) : base(targetFinder, unitsContainer)
        {
            _obstacleController = obstacleController;
            _obstacleController.OnObstalceRemoved += UpdateEnemyObstalce;

            _timer = new TimeRemaining(UpdateTargetPosition, 1.5f, true);
            TimersHolder.AddTimer(_timer);
        }

        protected override void ExecuteOnUpdate(float deltaTime)
        {
            for (int i = 0; i < unitsContainer.EnemyUnits.Count; i++)
            {
                IUnit unit = unitsContainer.EnemyUnits[i];

                UpdateTargetExistence(unit);

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

            for (int i = _deadUnits.Count - 1; i >= 0; i--)
            {
                DeadUnit undead = _deadUnits[i];
                undead.TimeLeft -= deltaTime;
                if (undead.TimeLeft <= 0.0f)
                {
                    RemoveDeadUnit(undead);
                }
            }
        }


        private void UpdateTargetPosition()
        {
            for (int i = 0; i < unitsContainer.EnemyUnits.Count; i++)
            {
                IUnit unit = unitsContainer.EnemyUnits[i];

                IAttackTarget target = unit.Target.CurrentTarget;

                if (target is not NoneTarget)
                {
                    switch (unit.UnitState.CurrentState)
                    {
                        default:
                            break;
                        case UnitState.Move:
                            {
                                if (unit.Target.IsTargetChangePosition())
                                {
                                    MoveUnitToTarget(unit, unit.Target.CurrentTarget);
                                }
                                break;
                            }

                    }
                }
            }
        }

        private void UpdateEnemyObstalce(IObstacle obstacle)
        {

            for (int i = 0; i < unitsContainer.EnemyUnits.Count; i++)
            {
                IUnit unit = unitsContainer.EnemyUnits[i];

                if (unit.Target.CurrentTarget.Id == obstacle.Id)
                {
                    unit.Target.ResetTarget();
                }
            }
        }

        private void UpdateTargetExistence(IUnit unit)
        {
            IAttackTarget target = unit.Target.CurrentTarget;

            if (target is NoneTarget)
            {
                ChangeUnitState(unit, UnitState.Idle);
                return;
            }

            switch (unit.Priority.Current.Priority)
            {
                case UnitPriorityType.MainTower:
                    {
                        var mainTowerTarget = targetFinder.GetMainTower();
                        if (target.Id == mainTowerTarget.Id)
                        {
                            if (unit.Navigation.CheckForPathComplete() == false)
                            {
                                if (CheckBlockByObstacle(unit) == true)
                                {
                                    ChangeUnitState(unit, UnitState.Move);
                                }
                            }
                        }
                        break;
                    }
                case UnitPriorityType.ClosestFoe:
                case UnitPriorityType.FarthestFoe:
                case UnitPriorityType.SpecificFoe:
                    {
                        if (unitsContainer.DefenderUnits.Exists(def => def.Id == target.Id))
                        {
                            if (unit.Navigation.CheckForPathComplete() == false)
                            {
                                if (CheckBlockByObstacle(unit) == true)
                                {
                                    ChangeUnitState(unit, UnitState.Move);
                                }
                            }
                        }
                        break;
                    }
            }
        }


        private void RemoveDeadUnit(DeadUnit undead)
        {
            undead.Unit.Disable();
            _deadUnits.Remove(undead);
        }

        protected override void UnitIdleState(IUnit unit, float deltaTime)
        {
            IAttackTarget target = GetTarget(unit);

            if (target.Id >= 0)
            {
                unit.Target.SetTarget(target);

                ChangeUnitState(unit, UnitState.Move);
                MoveUnitToTarget(unit, target);
            }
            else
            {
                if (!CheckIsOnDestinationPosition(unit))
                {
                    ChangeUnitState(unit, UnitState.Move);
                    unit.Navigation.MoveTo(unit.StartPosition);
                }
            }

        }

        protected override void UnitMoveState(IUnit unit, float deltaTime)
        {
            if (unit.Target.CurrentTarget.Id >= 0)
            {
                Vector3 targetPos = unit.Target.CurrentTarget.Position;
                float distanceSqr = (unit.GetPosition() - targetPos).sqrMagnitude;
                if (distanceSqr <= unit.Offence.MaxRange * unit.Offence.MaxRange)
                {
                    StopUnit(unit);
                    ChangeUnitState(unit, UnitState.Attack);
                }
            }
            else
            {
                if (CheckIsOnDestinationPosition(unit))
                {
                    ChangeUnitState(unit, UnitState.Idle);
                    unit.Navigation.Stop();
                }
            }
        }

        protected override void UnitAttackState(IUnit unit, float deltaTime)
        {
            AttackModel attack = _attackModels.Find(model => model.Attacker == unit);
            if (attack == null)
            {
                attack = new AttackModel()
                {
                    Attacker = unit,
                    TimingProgress = deltaTime,
                    Phase = AttackPhase.Cast
                };
                _attackModels.Add(attack);
            }
            else
            {
                IAttackTarget target = unit.Target.CurrentTarget;

                if (target != null)
                {
                    if (IsAttackDistanceSuitable(unit, target.Position))
                    {
                        switch (attack.Phase)
                        {
                            case AttackPhase.None:
                                attack.TimingProgress = deltaTime;
                                attack.Phase = AttackPhase.Cast;
                                break;
                            case AttackPhase.Cast:
                                attack.TimingProgress += deltaTime;
                                if (attack.TimingProgress >= unit.Offence.CastingTime)
                                {
                                    var unitDamage = unit.Offence.GetDamage();

                                    target.TakeDamage(unitDamage); // damage main target

                                    var additionTargets = GetAdditionAttackTargets(unit);
                                    for (int i = 0; i < additionTargets.Count; i++)
                                    {
                                        additionTargets[i].TakeDamage(unitDamage);
                                    }

                                    attack.Phase = AttackPhase.None;
                                    attack.TimingProgress = 0.0f;

                                    StartAttackAnimation(unit);
                                    //StartTakeDamageAnimation(unitTarget);
                                }

                                break;
                            case AttackPhase.Attack:
                                throw new NotImplementedException();
                            //break;
                            default:
                                throw new ArgumentOutOfRangeException();
                                //break;
                        }
                    }
                    else
                    {
                        attack.Phase = AttackPhase.None;
                        ChangeUnitState(unit, UnitState.Move);
                    }

                }
                else
                {
                    unit.Target.ResetTarget();
                    ChangeUnitState(unit, UnitState.Idle);
                }

            }
        }


        protected override void UnitDeadState(IUnit unit, float deltaTime) { }

        private List<IAttackTarget> GetAdditionAttackTargets(IUnit unit)
        {
            var result = new List<IAttackTarget>();

            switch (unit.Offence.AbilityType)
            {
                default:
                    break;
                case UnitAbilityType.ClosedAOE:
                    {

                        var targets = FindTargetsInAOERange(unit, unit.Offence.MaxRange);
                        for (int i = 0; i < targets.Count; i++) 
                        {
                            var target = GetAttackTarget(targets[i]);
                            if (target is not NoneTarget)
                            {
                                result.Add(target);
                            }
                        }
                        break;
                    }
            }

            return result;

        }

        private List<ITarget> FindTargetsInAOERange(IUnit unit, float aoeRange, float angle = 120)
        {
            var result = new List<ITarget>();

            var unitPos = unit.GetPosition();
           
            var targetsInViewRadius = Physics.OverlapSphere(unitPos, aoeRange);

            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                if (targetsInViewRadius[i].gameObject.TryGetComponent<ITarget>(out var findedTarget))
                {
                    if (findedTarget.Id != unit.Target.CurrentTarget.Id)
                    {
                        var targetPos = findedTarget.Position;

                        Vector3 dirToTarget = (targetPos - unitPos).normalized;

                        if (Vector3.Angle(unit.View.SelfTransform.forward, dirToTarget) < angle / 2)
                        {
                            result.Add(findedTarget);
                        }
                    }
                }
            }

            return result;
        }

        private IAttackTarget GetAttackTarget(ITarget target)
        {
            var mainTower = targetFinder.GetMainTower();
            if (mainTower.Id == target.Id)
                return mainTower;

            if (unitsContainer.DefenderUnits.Exists(def => def.Id == target.Id))
            {
                var defender = unitsContainer.DefenderUnits.Find(def => def.Id == target.Id);
                return new TargetModel(defender, target);
            }

            var obstacles = _obstacleController.ObstaclesCollection;
            if (obstacles.Exists(def => def.Id == target.Id))
            {
                var obstacle = obstacles.Find(def => def.Id == target.Id);
                return new TargetModel(obstacle, target);
            }

            return new NoneTarget();
        }

        private bool IsAttackDistanceSuitable(IUnit attacker, Vector3 targetPosition)
        {
            Vector3 attackerPosition = attacker.GetPosition();
            float maxAttackDistance = attacker.Offence.MaxRange;

            return maxAttackDistance * maxAttackDistance >= (attackerPosition - targetPosition).sqrMagnitude;
        }

        private void StartAttackAnimation(IUnit unit)
        {
            IUnitAnimationView animView = unit.View.UnitAnimation;
            if (animView != null)
            {
                animView.StartCast();
            }
        }

        private void StartTakeDamageAnimation(IUnit unit)
        {
            IUnitAnimationView animView = unit.View.UnitAnimation;
            if (animView != null)
            {
                animView.TakeDamage();
            }
        }

        #region Movement logic

        private void MoveUnitToTarget(IUnit unit, IAttackTarget target)
        {
            unit.Navigation.MoveTo(target.Position);
        }

        private void StopUnit(IUnit unit)
        {
            unit.Navigation.Stop();
        }

        private float GetDistanceToTarget(Vector3 unitPos, Vector3 targetPos) =>
            Vector3.Distance(unitPos, targetPos);


        private bool CheckIsOnDestinationPosition(IUnit unit)
        {
            Vector3 destination = unit.StartPosition;
            Vector3 position = unit.GetPosition();

            return (position - destination).sqrMagnitude <= DESTINATION_POSITION_ERROR_SQR;
        }

        #endregion

        #region Find target logic

        private IAttackTarget GetTarget(IUnit unit)
        {
            IAttackTarget result = new NoneTarget();

            while (unit.Priority.GetNext())
            {
                var currentPriority = unit.Priority.Current;

                switch (currentPriority.Priority)
                {
                    default:
                    case UnitPriorityType.MainTower:
                        {
                            result = targetFinder.GetMainTower();
                            break;
                        }

                    case UnitPriorityType.ClosestFoe:
                        {
                            result = GetClosestFoe(unit);
                            break;
                        }
                    case UnitPriorityType.SpecificFoe:
                        {
                            result = GetClosestFoe(unit, currentPriority.Type);
                            break;
                        }
                }

                if (result is not NoneTarget)
                {
                    break;
                }
            }

            unit.Priority.Reset();

            return result;
        }

        private IAttackTarget GetClosestFoe(IUnit unit, UnitType targetType = UnitType.None)
        {
            IAttackTarget target = new NoneTarget();

            Vector3 unitPos = unit.GetPosition();
            float minDist = float.MaxValue;

            for (int i = 0; i < unitsContainer.DefenderUnits.Count; i++)
            {
                IUnit foeUnit = unitsContainer.DefenderUnits[i];

                if (targetType != UnitType.None && foeUnit.Stats.Type != targetType)
                    continue;

                Vector3 defenderPos = foeUnit.GetPosition();

                float distance = Vector3.Distance(unitPos, defenderPos);
                if (distance < minDist)
                {
                    minDist = distance;
                    target = new TargetModel(foeUnit, foeUnit.View);
                }
            }

            return target;
        }

        #endregion

        private bool CheckBlockByObstacle(IUnit unit)
        {
            var existObstacle = GetObstacle(unit);

            if (existObstacle is not NoneTarget)
            {
                if (unit.Target.CurrentTarget.Id != existObstacle.Id)
                {
                    unit.Target.ResetTarget();
                    unit.Target.SetTarget(existObstacle);

                    return true;
                }
            }
            return false;
        }

        public IAttackTarget GetObstacle(IUnit unit)
        {
            if (_obstacleController.ObstaclesCollection.Count == 0)
                return new NoneTarget();

            IAttackTarget result = new NoneTarget();

            var unitPos = unit.GetPosition();
            var targetPos = unit.Target.CurrentTarget.Position;

            Vector3 direction = targetPos - unitPos;
            float distance = Vector3.Distance(unitPos, targetPos);

            var hitObjects = Physics.RaycastAll(unitPos, direction, distance);

            if (hitObjects.Length > 0)
            {
                var obstacles = _obstacleController.ObstaclesCollection;

                for (int i = 0; i < hitObjects.Length; i++)
                {
                    var hitGo = hitObjects[i].collider.gameObject;

                    if (hitGo.TryGetComponent<ITarget>(out var findedObstacle))
                    {
                        var obstacle = obstacles.Find(obst => obst.Id == findedObstacle.Id);

                        if (obstacle != null)
                        {
                            result = new TargetModel(obstacle, obstacle.View);
                            break;
                        }
                    }
                }
            }

            return result;
        }
    }
}
