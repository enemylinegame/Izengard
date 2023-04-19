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
        GoInPosition,
        Fight,
        Idle
    }

    public class DefenderUnit : IDisposable, IOnUpdate
    {
        public event Action<DefenderUnit> DefenderUnitDead;
        public GameObject DefenderGameObject { get { return _defender; } }

        private Damageable _damageable;
        private IAction<Damageable> _attackAction;
        private List<Damageable> _listMeAttackedUnits = new List<Damageable>();
        private GameObject _defender;
        private DefenderUnitStats _unitStats;
        private NavMeshAgent _agent;

        private Vector3 _defendPosition;
        private DefenderState _state = DefenderState.GoInPosition;

        private bool _isReload = false;
        private float _tempTime = 0;

       

        public DefenderUnit(GameObject defender, Vector3 defendPosition)
        {
            _unitStats = new DefenderUnitStats(1f,25);
            _defender = defender;
            _defendPosition = defendPosition;
            _damageable = defender.GetComponent<Damageable>();
            _agent = defender.GetComponent<NavMeshAgent>();
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
                if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
                {
                    _state = DefenderState.GoInPosition;
                }
                else
                {
                    _state = DefenderState.Idle;
                }
            }
            switch (_state)
            {
                case DefenderState.GoInPosition:
                    _agent.SetDestination(_defendPosition);
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
    }

}
