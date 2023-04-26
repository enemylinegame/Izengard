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

        private Vector3 _defendPosition;
        private DefenderState _state = DefenderState.Going;

        private float _tempTime = 0;
        private float _stopDistanceSqr;
        private bool _isReload = false;
        private bool _isPositionChanged;


        public Vector3 Position
        {
            get
            {
                return _agent.nextPosition;
            }
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
            DefenderLogic();
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

                //if ( !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
                if ( _isPositionChanged || (_agent.remainingDistance > _agent.stoppingDistance))
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
                //Debug.Log("DefenderUnit->DefenderLogic: _state = " + _state.ToString());
            }
            else
            {
                Debug.Log("DefenderUnit->DefenderLogic: _listMeAttackedUnits.Count != 0; _state = " + _state.ToString());
            }

            switch (_state)
            {
                case DefenderState.Going:
                    if (_isPositionChanged)
                    {
                        _agent.SetDestination(_defendPosition);
                        _isPositionChanged = false;
                        Debug.Log($"DefenderUnit->DefenderLogic: _defendPosition = {_defendPosition}; " +
                            $"_nextPosition = {_agent.nextPosition}");
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
                            Debug.Log("DefenderUnit->DefenderLogic: _attackAction.StartAction(...)");
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
