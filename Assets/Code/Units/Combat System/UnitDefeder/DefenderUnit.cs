using System;
using System.Collections.Generic;
using Code.TileSystem;
using CombatSystem.DefenderStates;
using CombatSystem.Interfaces;
using UnityEngine;
using UnityEngine.AI;


namespace CombatSystem
{
    public class DefenderUnit : IDisposable, IOnUpdate
    {
        public event Action<DefenderUnit> DefenderUnitDead;
        public event Action<DefenderState> OnStateChanged; 
        public event Action<float, float> OnHealthChanged; 

        public GameObject DefenderGameObject { get { return _defenderRootGO; } }

        private Damageable _myDamageable;
        private GameObject _defenderRootGO;
        private DefenderUnitStats _unitStats;
        private NavMeshAgent _agent;
        private TileModel _tileModel;
        private DefenderAnimation _animation;
        private DefenderTargetsHolder _targetsHolder;
        private DefenderTargetFinder _targetFinder;
        private DefenderTargetSelector _targetSelector;
        private IDamageable _currentTarget;
        private DefenderVisualSelect _visualSelect;

        private DefenderFight _fightState;
        private DefenderGoing _goingState;
        private DefenderGotoBarrack _gotoBarrackState;
        private DefenderIdle _idleState;
        private DefenderInBarrack _inBarrackState;
        private DefenderPursuit _pursuitState;
        private DefenderStateBase _currentStateExecuter;

        private Vector3 _defendPosition;
        private DefenderState _state;


        /// <summary>
        /// Going to barrack or inside barrack
        /// </summary>
        public bool IsInBarrack => (_state == DefenderState.GotoBarrack || _state == DefenderState.InBarrack);

        public TileModel Tile
        {
            get => _tileModel;
            set => _tileModel = value;
        }

        public IHealthHolder HealthHolder  => _myDamageable;

        public Vector3 DefendPosition => _defendPosition;

        public bool IsVisualSelection
        {
            set
            {
                if (value)
                {
                    _visualSelect.On();
                }
                else
                {
                    _visualSelect.Off();
                }
            }
        }
        
        public Sprite Icon { get; private set; }
        

        public DefenderUnit(GameObject defender, Vector3 defendPosition, DefenderSettings settings, 
            IBulletsController bulletsController)
        {
            _unitStats = settings.UnitStats;
            _defenderRootGO = defender;
            _defendPosition = defendPosition;
            Icon = settings.Icon;
            _myDamageable = defender.GetComponent<Damageable>();
            _myDamageable.OnHealthChanged += HealthChanged;
            _myDamageable.OnDeath += DefenderDead;
            _myDamageable.OnDamaged += OnDamaged;
            _myDamageable.Init(_unitStats.MaxHealth, 1);
            _agent = defender.GetComponent<NavMeshAgent>();
            _agent.speed = _unitStats.MovementSpeed;
            _animation = new DefenderAnimation(defender, this);
            _targetsHolder = new DefenderTargetsHolder();
            _targetFinder = new DefenderTargetFinder(_defenderRootGO, _unitStats.VisionRange, _targetsHolder, 
                _unitStats, () => _tileModel);
            _targetFinder.OnTargetsDetected += AddedTargetInRange;
            _targetSelector = new DefenderTargetSelector(_defenderRootGO, _targetsHolder);
            
            if (settings.Type == DefenderType.Range)
            {
                _fightState = new DefenderFightRange(this, SetState, _unitStats, _targetsHolder, _targetSelector, 
                    _myDamageable, _targetFinder, bulletsController);
            }
            else
            {
                _fightState = new DefenderFight(this, SetState, _unitStats, _targetsHolder, _targetSelector, 
                    _myDamageable, _targetFinder);
            }

            _goingState = new DefenderGoing(this, SetState, _unitStats, _agent);
            _gotoBarrackState = new DefenderGotoBarrack(this, SetState, _agent);
            _idleState = new DefenderIdle(this, SetState, _agent);
            _inBarrackState = new DefenderInBarrack(this, SetState);
            _pursuitState = new DefenderPursuit(this, SetState, _agent, _targetSelector, _targetsHolder,
                _targetFinder);
            _visualSelect = new DefenderVisualSelect(defender, settings.SelectVisualEffectPrefab);
            _visualSelect.Off();

            SetState(DefenderState.Going);
        }

        private void DefenderDead()
        {
            _animation.Disable();
            DefenderUnitDead?.Invoke(this);
        }

        public void Dispose()
        {
            _animation.Disable();
            _myDamageable.OnDeath -= DefenderDead;
            ClearTargets();
            Debug.Log("DefenderUnit->Dispose:");
        }

        public void OnUpdate(float deltaTime)
        {
            ClearAttackingTargets();
            _currentStateExecuter.OnUpdate();
            _targetFinder.OnUpdate(deltaTime);
            _fightState.Reload();
            DebugDrawLineToTarget(_targetsHolder.CurrentTarget);
        }

        public void GoToPosition(Vector3 newPosition)
        {
            ClearTargets();
            _defendPosition = newPosition;
            
            _currentStateExecuter.GoToPosition(_defendPosition);
        }

        public void GoToBarrack(Vector3 destination)
        {
            ClearTargets();
            _defendPosition = destination;
            _gotoBarrackState.BarrackPosition = _defendPosition;
            _currentStateExecuter.GoToBarrack(destination);
        }

        public void ExitFromBarrack()
        {
            _currentStateExecuter.ExitFromBarrack();
        }

        private void HealthChanged(float maxHealth, float currentHealth)
        {
            OnHealthChanged?.Invoke(maxHealth,currentHealth);
        }

        private void OnDamaged(IDamageable attacker)
        {
            if (attacker != null)
            {
                if (!_targetsHolder.AttackingTargets.Contains(attacker))
                {
                    _targetsHolder.AttackingTargets.Add(attacker);

                }

                attacker.OnDeath += EnemyDead;
                //Debug.Log($"DefenderUnit::OnDamaged: {_state} ");
                _currentStateExecuter.OnDamaged(attacker);
            }
        }

        private void AddedTargetInRange()
        {
            _currentStateExecuter.AddedTargetInRange();
        }

        private void SetState(DefenderState newState)
        {
            if (_state == newState) return;
            switch (newState)
            {
                case DefenderState.Fight:
                    _currentStateExecuter = _fightState;
                    break;
                case DefenderState.Going:
                    _currentStateExecuter = _goingState;
                    break;
                case DefenderState.Idle:
                    _currentStateExecuter = _idleState;
                    break;
                case DefenderState.Pursuit:
                    _currentStateExecuter = _pursuitState;
                    break;
                case DefenderState.GotoBarrack:
                    _currentStateExecuter = _gotoBarrackState;
                    break;
                case DefenderState.InBarrack:
                    _currentStateExecuter = _inBarrackState;
                    break;
            }

            DefenderState oldState = _state;
            _state = newState;
            //Debug.Log($"DefenderUnit->SetState: {oldState} -> {_state}");
            _currentStateExecuter.StartState();
            OnStateChanged?.Invoke(_state);
        }

        private void EnemyDead()
        {
            for (int i = _targetsHolder.AttackingTargets.Count - 1; i >= 0; i--)
            {
                if (_targetsHolder.AttackingTargets[i].IsDead)
                {
                    _targetsHolder.AttackingTargets[i].OnDeath -= EnemyDead;
                    _targetsHolder.AttackingTargets.RemoveAt(i);
                }
            }
            // _attakersTargets.RemoveAll(target =>
            // {
            //     if (target.IsDead) target.DeathAction -= EnemyDead;
            //     return target.IsDead;
            // });
        }

        private void ClearTargets()
        {
            _targetsHolder.CurrentTarget = null;
            ClearAttackingTargets();
        }
        
        private void ClearAttackingTargets()
        {
            _targetsHolder.AttackingTargets.ForEach(target => target.OnDeath -= EnemyDead);
            _targetsHolder.AttackingTargets.Clear();
        }

        public void DestroyItself()
        {
            GameObject.Destroy(_defenderRootGO);
        }

        public void Dismiss()
        {
            if (!_myDamageable.IsDead)
            {
                _myDamageable.MakeDamage(_unitStats.MaxHealth, null);
            }

            GameObject.Destroy(_defenderRootGO);
        }

        private void DebugDrawLineToTarget(IDamageable target)
        {
#if UNITY_EDITOR
            if (target != null)
            {
                Vector3 start = _myDamageable.transform.position;
                start.y += 0.25f;
                Vector3 end = target.Position;
                end.y += 0.25f;
                Debug.DrawLine(start, end, Color.blue);
            }
#endif
        }
    }

}
