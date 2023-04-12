using System;
using UnityEngine;
using Wave;


namespace CombatSystem
{
    public class FindTargetAction : IAction<Damageable>, IOnUpdate, IDisposable
    {
        private const byte SEARCH_FRAMES = 10;
        public event Action<Damageable> OnComplete;
        private readonly SearchScope _searchScope;
        private Damageable _currentTarget;
        private byte _frameCount;

        private Damageable _unitDamagable;


        public FindTargetAction(Enemy unit)
        {
            _searchScope = unit.Prefab.GetComponentInChildren<SearchScope>(true);
            _unitDamagable = unit.Prefab.gameObject.GetComponent<Damageable>();
            _searchScope.OnTriger += OnSearchScopeEnter;
        }

        public void StartAction(Damageable target)
        {
            _searchScope.gameObject.SetActive(true);
            _frameCount = SEARCH_FRAMES;
            _currentTarget = target;
        }


        private void OnSearchScopeEnter(GameObject gameObject)
        {
            if (gameObject == null) return;
            if (!gameObject.CompareTag("Player")) return;
            if (gameObject.TryGetComponent(out Damageable damageable))
            {
                if (_currentTarget != damageable)
                {
                    if (damageable.Attacked(_unitDamagable))
                    {
                        _searchScope.gameObject.SetActive(false);
                        _currentTarget = damageable;
                    }
                }
            }
        }

        public void OnUpdate(float deltaTime)
        {
            if (_frameCount > 0)
            {
                _frameCount--;
                if (_frameCount <= 0)
                {
                    _searchScope.gameObject.SetActive(false);
                    OnComplete?.Invoke(_currentTarget);
                }
            }
        }

        public void Dispose()
        {
            _searchScope.OnTriger -= OnSearchScopeEnter;
        }
    }
}