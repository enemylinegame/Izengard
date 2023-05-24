using System;
using System.Collections.Generic;
using Code.TileSystem;
using UnityEngine;
using UnityEngine.AI;


namespace CombatSystem
{
    public class DefenderUnit : IDisposable, IOnUpdate
    {
        public event Action<DefenderUnit> DefenderUnitDead;
        public event Action<DefenderUnit> OnDestinationReached;
        public event Action<DefenderState> OnStateChanged; 

        public GameObject DefenderGameObject { get { return _defender; } }

        private Damageable _damageable;
        private IAction<Damageable> _attackAction;
        private List<Damageable> _listMeAttackedUnits = new List<Damageable>();
        private GameObject _defender;
        private DefenderUnitStats _unitStats;
        private NavMeshAgent _agent;
        private TileModel _tileModel;
        private DefenderAnimation _animation;

        private Vector3 _defendPosition;
        private DefenderState _state;

        private float _tempTime = 0;
        private float _stopDistanceSqr;
        private bool _isReload = false;
        private bool _isPositionChanged;
        private bool _isActive;


        public DefenderState State
        {
            get => _state;
            private set
            {
                if (_state != value)
                {
                    _state = value;
                    OnStateChanged?.Invoke(_state);
                }
            }
        }

        public Vector3 Position
        {
            get
            {
                return _agent.nextPosition;
            }
        }

        /// <summary>
        /// Going to barrack or inside barrack
        /// </summary>
        public bool IsInBarrack
        {
            get
            {
                return (State == DefenderState.GotoBarrack || State == DefenderState.InBarrack);
            }
        }

        public TileModel Tile
        {
            get => _tileModel;
            set => _tileModel = value;
        }

        public DefenderUnit(GameObject defender, Vector3 defendPosition)
        {
            _unitStats = new DefenderUnitStats(1f,25);
            _defender = defender;
            _defendPosition = defendPosition;
            _isPositionChanged = true;
            _damageable = defender.GetComponent<Damageable>();
            _agent = defender.GetComponent<NavMeshAgent>();
            _stopDistanceSqr = _agent.stoppingDistance * _agent.stoppingDistance;
            _damageable.DeathAction += DefenderDead;
            _damageable.MeAttackedChenged += MeAttacked;
            _damageable.Init(100, 1);
            _attackAction = new DefenderAttackAction(_unitStats);
            _animation = new DefenderAnimation(defender, this);
            State = DefenderState.Going;
            _isActive = true;
        }

        private void MeAttacked(List<Damageable> listMeAttackedUnits)
        {
            if (listMeAttackedUnits.Count != 0)
            {
                State = DefenderState.Fight;
            }
            else
            {
                State = DefenderState.Idle;
            }
            _listMeAttackedUnits = listMeAttackedUnits;
        }

        private void DefenderDead()
        {
            _animation.Disable();
            DefenderUnitDead?.Invoke(this);
        }

        public void Dispose()
        {
            _animation.Disable();
            _damageable.DeathAction -= DefenderDead;
            _damageable.MeAttackedChenged -= MeAttacked;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_isActive)
            {
                DefenderLogic();
            }
            Reload();
        }

        private void Reload()
        {
            if (_isReload)
            {
                if (_tempTime < _unitStats._attackSpeed)
                {
                    _tempTime += Time.deltaTime;
                }
                else
                {
                    _tempTime = 0;
                    _isReload = false;
                }
            }
        }

        private void DefenderLogic()
        {
            if (_listMeAttackedUnits.Count == 0)
            {
                Vector3 currentPosition = _agent.nextPosition;
                currentPosition.y = 0.0f;

                if (_isPositionChanged || (_agent.remainingDistance > _agent.stoppingDistance))
                {
                    if (State != DefenderState.GotoBarrack)
                    {
                        State = DefenderState.Going;
                    }
                }
                else
                {
                    if (State == DefenderState.Going)
                    {
                        State = DefenderState.Idle;
                        OnDestinationReached?.Invoke(this);
                    }
                    else if (State == DefenderState.GotoBarrack)
                    {
                        State = DefenderState.InBarrack;
                        Deactivate();
                        OnDestinationReached?.Invoke(this);
                    }
                }
            }

            switch (State)
            {
                case DefenderState.GotoBarrack:
                case DefenderState.Going:
                    if (_isPositionChanged)
                    {
                        _agent.ResetPath();
                        _agent.SetDestination(_defendPosition);
                        _isPositionChanged = false;
                    }
                    break;
                case DefenderState.Idle:
                    break;
                case DefenderState.Fight:
                    if (_listMeAttackedUnits.Count != 0 && !_isReload)
                    {
                        for (int i = 0; i < _listMeAttackedUnits.Count; i++)
                        {
                            _isReload = true;
                            _attackAction.StartAction(_listMeAttackedUnits[i]);
                        }
                    }
                    break;
            }
        }

        public void GoToPosition(Vector3 newPosition)
        {
            newPosition.y = _defendPosition.y;
            _defendPosition = newPosition;
            _isPositionChanged = true;
        }

        public void GoToBarrack(Vector3 destination)
        {
            State = DefenderState.GotoBarrack;
            GoToPosition(destination);
        }

        public void Activate()
        {
            if (!_isActive)
            {
                _isActive = true;
                _defender.SetActive(true);
                State = DefenderState.Idle;
            }
        }

        private void Deactivate()
        {
            _isActive = false;
            _defender.SetActive(false);
        }
    }

}
