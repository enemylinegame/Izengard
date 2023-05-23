using System;
using System.Collections.Generic;
using Code.TileSystem;
using UnityEngine;
using UnityEngine.AI;


namespace CombatSystem
{
    public class DefenderUnit : IDisposable, IOnUpdate
    {
        private enum DefenderState
        {
            None        = 0,
            Going       = 1,
            Fight       = 2,
            Idle        = 3,
            InBarrack   = 4,
            GotoBarrack = 5
        }

        public event Action<DefenderUnit> DefenderUnitDead;
        public event Action<DefenderUnit> OnDestinationReached;

        public GameObject DefenderGameObject { get { return _defender; } }

        private Damageable _damageable;
        private IAction<Damageable> _attackAction;
        private List<Damageable> _listMeAttackedUnits = new List<Damageable>();
        private GameObject _defender;
        private DefenderUnitStats _unitStats;
        private NavMeshAgent _agent;
        private TileModel _tileModel;

        private Vector3 _defendPosition;
        private DefenderState _state;

        private float _tempTime = 0;
        private float _stopDistanceSqr;
        private bool _isReload = false;
        private bool _isPositionChanged;
        private bool _isActive;

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
                return (_state == DefenderState.GotoBarrack || _state == DefenderState.InBarrack);
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
            _state = DefenderState.Going;
            _isActive = true;
        }

        private void MeAttacked(List<Damageable> listMeAttackedUnits)
        {
            if (listMeAttackedUnits.Count != 0)
            {
                _state = DefenderState.Fight;
            }
            else
            {
                _state = DefenderState.Idle;
            }
            _listMeAttackedUnits = listMeAttackedUnits;
        }

        private void DefenderDead()
        {
            DefenderUnitDead?.Invoke(this);
        }

        public void Dispose()
        {
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
                    if (_state != DefenderState.GotoBarrack)
                    {
                        _state = DefenderState.Going;
                    }
                }
                else
                {
                    if (_state == DefenderState.Going)
                    {
                        _state = DefenderState.Idle;
                        OnDestinationReached?.Invoke(this);
                    }
                    else if (_state == DefenderState.GotoBarrack)
                    {
                        _state = DefenderState.InBarrack;
                        Deactivate();
                        OnDestinationReached?.Invoke(this);
                    }
                }
            }

            switch (_state)
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
            _state = DefenderState.GotoBarrack;
            GoToPosition(destination);
        }

        public void Activate()
        {
            if (!_isActive)
            {
                _isActive = true;
                _defender.SetActive(true);
                _state = DefenderState.Idle;
            }
        }

        private void Deactivate()
        {
            _isActive = false;
            _defender.SetActive(false);
        }
    }

}
