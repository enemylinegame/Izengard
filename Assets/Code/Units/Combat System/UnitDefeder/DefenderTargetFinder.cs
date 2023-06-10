﻿using System;
using System.Collections.Generic;
using CombatSystem.Interfaces;
using UnityEngine;


namespace CombatSystem
{
    public class DefenderTargetFinder: IOnUpdate, IDisposable
    {
        public event Action<IDamageable> OnNewTarget;
        public event Action<IDamageable> OnTargetLost;

        private const int FREQUNCY_REDUCTION_FRAMES = 5;

        private List<Collider> _targetsCollidersInRange;
        private DefenderTargetsHolder _targetsHolder;
        private SearchScope _searchScope;
        private Transform _transform;
        private LayerMask _targetMask = LayerMask.NameToLayer("Enemy");
        private float _range;

        private int _frequencyReductionCounter;


        public DefenderTargetFinder(GameObject defenderRoot, float range, DefenderTargetsHolder holder)
        {
            _targetsCollidersInRange = new List<Collider>();
            _searchScope = defenderRoot.GetComponentInChildren<SearchScope>();
            _searchScope.OnEnter += EnemyEnter;
            _searchScope.OnExit += EnemyLeave;
            _searchScope.SetRadius(range);
            _transform = defenderRoot.transform;
            _range = range;
            _targetsHolder = holder;
        }
        

        public void OnUpdate(float deltaTime)
        {
            // _frequencyReductionCounter++;
            // if (_frequencyReductionCounter >= FREQUNCY_REDUCTION_FRAMES)
            // {
            //     _frequencyReductionCounter = 0;
            //     DoSearch();
            // }
            int quantity = _targetsHolder.TargetsInRange.Count;
            for (int i = quantity - 1; i >= 0; i--)
            {
                IDamageable target = _targetsHolder.TargetsInRange[i];
                if (target.IsDead)
                {
                    Debug.Log("DefenderTargetFinder->OnUpdate: target.IsDead -");
                    _targetsHolder.TargetsInRange.RemoveAt(i);
                    OnTargetLost?.Invoke(target);
                }
            }
            
        }

        public void Dispose()
        {
            _targetsCollidersInRange.Clear();
            _targetsHolder.TargetsInRange.Clear();
        }

        private void EnemyEnter(GameObject enemy)
        {
            if (enemy.TryGetComponent(out IDamageable damageable))
            {
                if (!_targetsHolder.TargetsInRange.Contains(damageable))
                {
                    _targetsHolder.TargetsInRange.Add(damageable);
                    Debug.Log("DefenderTargetFinder->EnemyEnter: +");
                    OnNewTarget?.Invoke(damageable);
                }
            }
            Debug.Log($"DefenderTargetFinder->EnemyEnter: targets: {_targetsHolder.TargetsInRange.Count}");
        }

        private void EnemyLeave(GameObject enemy)
        {
            Debug.Log("DefenderTargetFinder->EnemyLeave: o ");
            if (enemy.TryGetComponent(out IDamageable damageable))
            {
                if (_targetsHolder.TargetsInRange.Remove(damageable))
                {
                    Debug.Log("DefenderTargetFinder->EnemyLeave: ----");
                    OnTargetLost?.Invoke(damageable);
                }
            }
            Debug.Log($"DefenderTargetFinder->EnemyLeave: targets: {_targetsHolder.TargetsInRange.Count}");
        }
        
        // private void DoSearch()
        // {
        //     var targets = Physics.SphereCastAll(_transform.position, _range,Vector3.forward, _targetMask);
        //
        //     List<Collider> currentColliders = new List<Collider>();
        //     for (int i = 0; i < targets.Length; i++)
        //     {
        //         currentColliders.Add(targets[i].collider);
        //     }
        //
        // }
        
    }
}