using System;
using System.Collections.Generic;
using CombatSystem.Interfaces;
using UnityEngine;


namespace CombatSystem
{
    public class DefenderTargetFinder: IOnUpdate
    {
        public event Action OnTargetsDetected;

        private const int FREQUNCY_REDUCTION_FRAMES = 5;

        private DefenderTargetsHolder _targetsHolder;
        private Transform _transform;

        private LayerMask _targetMask = LayerMask.GetMask("Enemy");
        private float _range;

        private int _frequencyReductionCounter;


        public DefenderTargetFinder(GameObject defenderRoot, float range, DefenderTargetsHolder holder)
        {
            _transform = defenderRoot.transform;
            _range = range;
            _targetsHolder = holder;
        }
        

        public void OnUpdate(float deltaTime)
        {
            _targetsHolder.TargetsInRange.Clear();
            _frequencyReductionCounter++;
            if (_frequencyReductionCounter >= FREQUNCY_REDUCTION_FRAMES)
            {
                _frequencyReductionCounter = 0;
                DoSearch();
            }
        }
        
        private void DoSearch()
        {
            var targets = Physics.SphereCastAll(_transform.position, _range, Vector3.forward,
                0.0f, _targetMask);
            if (targets.Length > 0)
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    if (targets[i].collider.gameObject.TryGetComponent(out IDamageable damageable))
                    {
                        _targetsHolder.TargetsInRange.Add(damageable);
                    }
                }
                OnTargetsDetected?.Invoke();
            }
        }

    }
}