using Wave;
using UnityEngine.AI;
using System;

namespace CombatSystem
{
    public class EnemyAI : IEnemyAI
    {
        private readonly Damageable _primaryTarget;
        private Damageable _currentTarget;
        private readonly IAction<Damageable> _findTarget;
        private readonly IAction<Damageable> _planRoute;
        private readonly IAction<Damageable> _checkAttackDistance;
        private readonly IAction<Damageable> _attack;
        private IAction<Damageable> _nextAction;
        private IOnUpdate _onUpdate;
        private readonly float _attackDistance;


        public bool IsActionComplete { get; private set; }


        public EnemyAI(Enemy unit, Damageable primaryTarget, IEnemyAnimationController animationController,
            IBulletsController bulletsController)
        {
            var navmesh = unit.Prefab.GetComponent<NavMeshAgent>();
            _primaryTarget = primaryTarget;
            _findTarget = new FindTargetAction(unit);
            _onUpdate = _findTarget as IOnUpdate;
            _planRoute = new PlanRouteAction(navmesh);
            if (unit.Type == EnemyType.Archer)
            {
                _attack = new RangedAttackAction(bulletsController, unit);
            }
            else
            {
                _attack = new AttackAction(animationController, unit);
            }
            _checkAttackDistance = new CheckAttackDistance(unit, navmesh, unit.Stats.AttackRange);

            _findTarget.OnComplete += OnFindTargetComplete;
            _planRoute.OnComplete += OnPlaneRouteComplete;
            _attack.OnComplete += OnAttackComplete;
            _checkAttackDistance.OnComplete += OnCheckAttackDistanceComplete;

            StopAction();
        }

        public void StartAction()
        {
            IsActionComplete = false;
            _nextAction.StartAction(_currentTarget);
        }

        public void StopAction()
        {
            _nextAction = _findTarget;
            _onUpdate = _findTarget as IOnUpdate;
            _planRoute.StartAction(_primaryTarget);
        }

        private void OnFindTargetComplete(Damageable target)
        {
            if (target == null || target.IsDamagableDead) _currentTarget = _primaryTarget;
            else _currentTarget = target;
            _nextAction = _planRoute;
            IsActionComplete = true;
        }

        private void OnPlaneRouteComplete(Damageable target)
        {
            if (target == null || target.IsDamagableDead)
            {
                _nextAction = _findTarget;
                _onUpdate = _findTarget as IOnUpdate;
            }
            else _nextAction = _checkAttackDistance;
            IsActionComplete = true;
        }

        private void OnAttackComplete(Damageable target)
        {
            if (target == null || target.IsDamagableDead)
            {
                _currentTarget = null;
                _nextAction = _findTarget;
                _onUpdate = _findTarget as IOnUpdate;
            }
            else _nextAction = _checkAttackDistance;
            IsActionComplete = true;
        }

        private void OnCheckAttackDistanceComplete(Damageable target)
        {
            if (target != null && !target.IsDamagableDead)
            {
                _nextAction = _attack;
                if (_attack is IOnUpdate update) _onUpdate = update;
            }
            else
            {
                _nextAction = _findTarget;
                _onUpdate = _findTarget as IOnUpdate;
            }
            IsActionComplete = true;
        }

        public void Dispose()
        {
            _findTarget.OnComplete -= OnFindTargetComplete;
            _planRoute.OnComplete -= OnPlaneRouteComplete;
            _attack.OnComplete -= OnAttackComplete;
            _checkAttackDistance.OnComplete -= OnCheckAttackDistanceComplete;
            if (_findTarget is IDisposable disposable) disposable.Dispose();
        }

        public void OnUpdate(float deltaTime)
        {
            _onUpdate?.OnUpdate(deltaTime);
        }
    }
}