using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CombatSystem
{
    public class DefenderUnitStats
    {
        public float _attackSpeed;
        public int _attackDamage;

        public DefenderUnitStats(float attackSpeed, int attackDamage)
        {
            _attackSpeed = attackSpeed;
            _attackDamage = attackDamage;
        }
    }
    
    public enum DefenderState
    {
        None = 0,
        Going,
        Fight,
        Idle
    }

    public class DefenderUnit : IDisposable, IOnUpdate
    {
        public event Action<DefenderUnit> DefenderUnitDead;
        public event Action<DefenderUnit> OnDestinationReached;

        public GameObject DefenderGameObject { get { return _defender; } }

        private Damageable _damageable;
        private IAction<Damageable> _attackAction;
        private List<Damageable> _listMeAttackedUnits = new List<Damageable>();
        private GameObject _defender;
        private DefenderUnitStats _unitStats;
        private NavMeshAgent _agent;
        private TileDefendersSquad _squad;

        private Vector3 _defendPosition;
        private DefenderState _state = DefenderState.Going;

        private float _tempTime = 0;
        private float _stopDistanceSqr;
        private bool _isReload = false;
        private bool _isPositionChanged;
        private bool _isEnabled;


        public Vector3 Position
        {
            get
            {
                return _agent.nextPosition;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    _defender.SetActive(value);
                }
            }
        }

        public TileDefendersSquad Squad { get => _squad; set => _squad = value; }

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
            _isEnabled = true;

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
            Debug.Log($"DefenderUnit->MeAttacked: _listMeAttackedUnits.Count = {_listMeAttackedUnits.Count}; _state = {_state}");
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
            if (_isEnabled)
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
                    _state = DefenderState.Going;
                }
                else
                {
                    if (_state == DefenderState.Going)
                    {
                        _state = DefenderState.Idle;
                        OnDestinationReached?.Invoke(this);
                    }

                    _state = DefenderState.Idle;
                }
            }

            switch (_state)
            {
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
    }

}
