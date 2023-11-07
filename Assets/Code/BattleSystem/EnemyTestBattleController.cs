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

        protected List<IUnit> _enemyUnits = new();
        protected List<IUnit> _defenderUnits = new();
        private List<AttackModel> _attackModels = new();
        private List<DeadUnit> _deadUnits = new();

        private readonly ObstacleController _obstacleController;

        private TimeRemaining _timer;

        public EnemyTestBattleController(
            TargetFinder targetFinder, 
            ObstacleController obstacleController) : base(targetFinder)
        {
            _obstacleController = obstacleController;
            _obstacleController.OnObstalceRemoved += UpdateEnemyObstalce;

            _timer = new TimeRemaining(UpdateTargetPosition, 1.5f, true);
            TimersHolder.AddTimer(_timer);
        }

        private void UpdateTargetPosition()
        {
            for (int i = 0; i < _enemyUnits.Count; i++)
            {
                IUnit unit = _enemyUnits[i];

                IAttackTarget target = unit.Target.CurrentTarget;

                if(target is not NoneTarget)
                {
                    switch (unit.UnitState.CurrentState)
                    {
                        default:
                            break;
                        case UnitState.Move:
                        case UnitState.Approach:
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

            for (int i = 0; i < _enemyUnits.Count; i++)
            {
                IUnit unit = _enemyUnits[i];

                if(unit.Target.CurrentTarget.Id == obstacle.Id)
                {
                    unit.Target.ResetTarget();
                }
            }
        }


        public override void OnUpdate(float deltaTime)
        {

            for (int i = 0; i < _enemyUnits.Count; i++)
            {
                IUnit unit = _enemyUnits[i];

                ExecuteUnitUpdate(unit, deltaTime);
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

        public override void OnFixedUpdate(float fixedDeltaTime) { }


        protected void ExecuteUnitUpdate(IUnit unit, float deltaTime)
        {
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
                                    ChangeUnitState(unit, UnitState.Approach);
                                }
                            }
                        }
                        break;
                    }
                case UnitPriorityType.ClosestFoe:
                case UnitPriorityType.FarthestFoe:
                case UnitPriorityType.SpecificFoe:
                    {
                        if (_defenderUnits.Exists(def => def.Id == target.Id))
                        {
                            if (unit.Navigation.CheckForPathComplete() == false)
                            {
                                if (CheckBlockByObstacle(unit) == true)
                                {
                                    ChangeUnitState(unit, UnitState.Approach);
                                }
                            }
                        }
                        break;
                    }
            }
        }


        public override void AddUnit(IUnit unit)
        {
            if (unit == null) return;

            unit.OnReachedZeroHealth += UnitReachedZeroHealth;

            switch (unit.Stats.Faction)
            {
                default:
                    break;
                case UnitFactionType.Enemy:
                    {
                        unit.Navigation.Enable();
                        unit.UnitState.ChangeState(UnitState.Idle);

                        _enemyUnits.Add(unit);

                        break;
                    }
                case UnitFactionType.Defender:
                    {
                        _defenderUnits.Add(unit);
                        break;
                    }
            }
        }

        private void UnitReachedZeroHealth(IUnit unit)
        {
            unit.OnReachedZeroHealth -= UnitReachedZeroHealth;
           
            if (unit.Stats.Faction == UnitFactionType.Enemy)
            {                           
                ChangeUnitState(unit, UnitState.Die);
                
                unit.Target.ResetTarget();
                
                unit.Navigation.Stop();

                _enemyUnits.Remove(unit);

                var undead = new DeadUnit(unit);
                _deadUnits.Add(undead);
            }
            else if (unit.Stats.Faction == UnitFactionType.Defender)
            {
                var linkedEnemy = 
                    _enemyUnits.FindAll(e => e.Target.CurrentTarget == unit.View);

                foreach (var enemy in linkedEnemy)
                {
                    enemy.Target.ResetTarget();
                }

                _defenderUnits.Remove(unit);
            }
        }

        private void RemoveDeadUnit(DeadUnit undead)
        {
            undead.Unit.Disable();
            _deadUnits.Remove(undead);
        }

        protected void UnitIdleState(IUnit unit, float deltaTime)
        {        
            IAttackTarget target = GetTarget(unit);
            
            if (target.Id >= 0)
            {
                unit.Target.SetTarget(target);
                
                ChangeUnitState(unit, UnitState.Approach);
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

        protected void UnitMoveState(IUnit unit, float deltaTime)
        {
            IAttackTarget target = GetTarget(unit);
            if (target.Id >= 0)
            {
                unit.Target.SetTarget(target);
                ChangeUnitState(unit, UnitState.Approach);
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

        protected void UnitApproachState(IUnit unit, float deltaTime)
        {
            Vector3 targetPos = unit.Target.CurrentTarget.Position;
            float distanceSqr = (unit.GetPosition() - targetPos).sqrMagnitude;
            if (distanceSqr <= unit.Offence.MaxRange * unit.Offence.MaxRange)
            {
                StopUnit(unit);
                ChangeUnitState(unit, UnitState.Attack);
            }
        }

        protected void UnitAttackState(IUnit unit, float deltaTime)
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

                                    target.TakeDamage(unit.Offence.GetDamage());
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
                        ChangeUnitState(unit, UnitState.Approach);
                    }

                }
                else
                {
                    unit.Target.ResetTarget();
                    ChangeUnitState(unit, UnitState.Idle);
                }

            }
        }

        private bool IsAttackDistanceSuitable(IUnit attacker, Vector3 targetPosition)
        {
            Vector3 attackerPosition = attacker.GetPosition();
            float maxAttackDistance = attacker.Offence.MaxRange;

            return maxAttackDistance * maxAttackDistance >= (attackerPosition - targetPosition).sqrMagnitude;
        }

        protected void ChangeUnitState(IUnit unit, UnitState state)
        {
            unit.UnitState.ChangeState(state);
            IUnitAnimationView animView = unit.View.UnitAnimation;
            if (animView != null)
            {
                switch (state)
                {
                    case UnitState.None:
                        animView.Reset();
                        break;
                    case UnitState.Idle:
                        animView.IsMoving = false;
                        break;
                    case UnitState.Move:
                        animView.IsMoving = true;
                        break;
                    case UnitState.Approach:
                        animView.IsMoving = true;
                        break;
                    case UnitState.Search:
                        break;
                    case UnitState.Attack:
                        animView.IsMoving = false;
                        break;
                    case UnitState.Die:
                        animView.StartDead();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

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

            for (int i = 0; i < _defenderUnits.Count; i++)
            {
                IUnit foeUnit = _defenderUnits[i];

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
            
            if(existObstacle is not NoneTarget)
            {
                if(unit.Target.CurrentTarget.Id != existObstacle.Id)
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
