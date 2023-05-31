using Wave;
using UnityEngine.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class EnemyAI : IEnemyAI
    {
        private List<IAction<Damageable>> _actionList;
        private readonly Damageable _primaryTarget;
        private Damageable _currentTarget;
        private readonly IAction<Damageable> _findTarget;
        private readonly IAction<Damageable> _planRoute;
        private readonly IAction<Damageable> _checkAttackDistance;
        private readonly IAction<Damageable> _attack;
        private IAction<Damageable> _nextAction;
        private IOnUpdate _onUpdate;
        private readonly float _attackDistance;

        private Transform _enemyTransform;

        public bool IsActionComplete { get; private set; }


        public EnemyAI(Enemy unit, Damageable primaryTarget, IEnemyAnimationController animationController,
            IBulletsController bulletsController)
        {
            _actionList = new List<IAction<Damageable>>();
            var navmesh = unit.RootGameObject.GetComponent<NavMeshAgent>();
            _primaryTarget = primaryTarget;
            _findTarget = new FindTargetAction(unit,primaryTarget);
            _actionList.Add(_findTarget);
            _onUpdate = _findTarget as IOnUpdate;
            _planRoute = new PlanRouteAction(navmesh);
            _actionList.Add(_planRoute);
            if (unit.Type == EnemyType.Archer)
            {
                _attack = new RangedAttackAction(bulletsController, unit);
            }
            else
            {
                _attack = new AttackAction(animationController, unit);
            }
            _actionList.Add(_attack);
            _checkAttackDistance = new CheckAttackDistance(unit, navmesh, unit.Stats.AttackRange);
            _actionList.Add(_checkAttackDistance);

            _findTarget.OnComplete += OnFindTargetComplete;
            _planRoute.OnComplete += OnPlaneRouteComplete;
            _attack.OnComplete += OnAttackComplete;
            _checkAttackDistance.OnComplete += OnCheckAttackDistanceComplete;

            _enemyTransform = unit.RootGameObject.transform;
            
            StopAction();
        }

        public void StartAction()
        {
            IsActionComplete = false;
            _nextAction.StartAction(_currentTarget);
        }

        public void StopAction()
        {
            ClearTarget();
            _nextAction = _findTarget;
            _onUpdate = _findTarget as IOnUpdate;
            _planRoute.StartAction(_primaryTarget);
        }

        private void OnFindTargetComplete(Damageable target)
        {
            if (target == null)
            {
                _currentTarget = _primaryTarget;
            }
            else
            {
                _currentTarget.DeathAction -= OnTargetDestroyed;
                if (target.IsDamagableDead)
                {

                    _currentTarget = _primaryTarget;
                }
                else
                {
                    _currentTarget = target;
                    _currentTarget.DeathAction += OnTargetDestroyed;
                }
            }
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
                if (_currentTarget != null)
                {
                    OnTargetDestroyed();
                }
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
            if (_currentTarget != null)
            {
                _currentTarget.DeathAction -= OnTargetDestroyed;
            }
        }

        public void OnUpdate(float deltaTime)
        {
            DrawLineToTarget();
            _onUpdate?.OnUpdate(deltaTime);
        }

        private void OnTargetDestroyed()
        {
            Debug.Log("EnemyAI->OnTargetDestroyed:");
            if (_currentTarget)
            {
                _currentTarget.DeathAction -= OnTargetDestroyed;
            }
            else
            {
                Debug.Log("EnemyAI->OnTargetDestroyed: _currentTarget == null");
            }

            ClearTarget();
        }

        public void ClearTarget()
        {
            _currentTarget = null;
            _actionList.ForEach(action => action.ClearTarget());
        }

        private void DrawLineToTarget()
        {
#if UNITY_EDITOR
            if (_currentTarget)
            {
                Vector3 start = _enemyTransform.position;
                start.y += 0.30f;
                Vector3 end = _currentTarget.transform.position;
                end.y += 0.30f;
                Debug.DrawLine(start, end, Color.red);
            }
#endif
        }
        
    }
}