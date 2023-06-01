using System;
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
        private SearchScope _searchScope;
        private Transform _transform;
        private LayerMask _targetMask = LayerMask.NameToLayer("Enemy");
        private float _range;

        private int _frequencyReductionCounter;

        public List<IDamageable> Targets { get; private set; }

        public DefenderTargetFinder(GameObject defenderRoot, float range)
        {
            _targetsCollidersInRange = new List<Collider>();
            _searchScope = defenderRoot.GetComponentInChildren<SearchScope>();
            _searchScope.OnEnter += EnemyEnter;
            _searchScope.OnExit += EnemyLeave;
            _searchScope.SetRadius(range);
            _transform = defenderRoot.transform;
            _range = range;
            Targets = new List<IDamageable>();
        }
        

        public void OnUpdate(float deltaTime)
        {
            // _frequencyReductionCounter++;
            // if (_frequencyReductionCounter >= FREQUNCY_REDUCTION_FRAMES)
            // {
            //     _frequencyReductionCounter = 0;
            //     DoSearch();
            // }
        }

        public void Dispose()
        {
            _targetsCollidersInRange.Clear();
            Targets.Clear();
        }

        private void EnemyEnter(GameObject enemy)
        {
            if (enemy.TryGetComponent(out IDamageable damageable))
            {
                if (!Targets.Contains(damageable))
                {
                    Targets.Add(damageable);
                    OnNewTarget?.Invoke(damageable);
                }
            }
        }

        private void EnemyLeave(GameObject enemy)
        {
            if (enemy.TryGetComponent(out IDamageable damageable))
            {
                if (Targets.Remove(damageable))
                {
                    OnNewTarget?.Invoke(damageable);
                }
            }
        }
        
        private void DoSearch()
        {
            var targets = Physics.SphereCastAll(_transform.position, _range,Vector3.forward, _targetMask);

            List<Collider> currentColliders = new List<Collider>();
            for (int i = 0; i < targets.Length; i++)
            {
                currentColliders.Add(targets[i].collider);
            }

        }
        
    }
}