using System;
using System.Collections.Generic;
using Code.TileSystem;
using CombatSystem.Interfaces;
using UnityEngine;


namespace CombatSystem
{
    public class DefenderTargetFinder: IOnUpdate
    {
        public event Action OnTargetsDetected;

        private const int FREQUNCY_REDUCTION_FRAMES = 5;

        private readonly DefenderUnitStats _stats;
        private DefenderTargetsHolder _targetsHolder;
        private Transform _transform;
        private Func<TileModel> _getTileDelegate;

        private LayerMask _targetMask = LayerMask.GetMask("Enemy");
        private float _range;

        private int _frequencyReductionCounter;


        public DefenderTargetFinder(GameObject defenderRoot, float range, DefenderTargetsHolder holder, 
            DefenderUnitStats stats, Func<TileModel> getTileDelegate)
        {
            _transform = defenderRoot.transform;
            _range = range;
            _targetsHolder = holder;
            _stats = stats;
            _getTileDelegate = getTileDelegate;
        }
        

        public void OnUpdate(float deltaTime)
        {
            _targetsHolder.TargetsInRange.Clear();
            _frequencyReductionCounter++;
            if (_frequencyReductionCounter >= FREQUNCY_REDUCTION_FRAMES)
            {
                _frequencyReductionCounter = 0;
                //DoSearchByCast();
                DoSearchFromTileData();
            }
        }
        
        private void DoSearchByCast()
        {
            var targets = Physics.SphereCastAll(_transform.position, _range, Vector3.forward,
                0.0f, _targetMask);
            bool isDetected = false;
            if (targets.Length > 0)
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    if (targets[i].collider.gameObject.TryGetComponent(out IDamageable damageable))
                    {
                        _targetsHolder.TargetsInRange.Add(damageable);
                        isDetected = true;
                    }
                }

                if (isDetected)
                {
                    OnTargetsDetected?.Invoke();
                }
            }
        }

        private void DoSearchFromTileData()
        {
            TileModel tile = _getTileDelegate();
            var list = tile.EnemiesInTile;
            float rangeSqr = _stats.VisionRange * _stats.VisionRange;
            bool isDetected = false;
            foreach (var target in list)
            {
                if ((target.Position - _transform.position).sqrMagnitude <= rangeSqr)
                {
                    _targetsHolder.TargetsInRange.Add(target);
                    isDetected = true;
                }
            }
            
            if (isDetected)
            {
                OnTargetsDetected?.Invoke();
            }

        }

        public bool IsTargetInRange(IDamageable target)
        {
            bool isInrange = false;
        
            if (target != null)
            {
                Vector3 myPosition = _transform.position;
                myPosition.y = 0.0f;
                Vector3 targetPosition = target.Position;
                targetPosition.y = 0.0f;
                
                isInrange = (targetPosition - myPosition).sqrMagnitude <= _stats.AttackRange * _stats.AttackRange;
            }
            
            return isInrange;
        }
        
    }
}