using System;
using UnityEngine;
using Wave;


namespace CombatSystem
{
    public class FindTargetAction : IAction<Damageable>, IOnUpdate//, IDisposable
    {
        private const byte SEARCH_FRAMES = 10;
        public event Action<Damageable> OnComplete;
      //  private readonly SearchScope _searchScope;
        private Damageable _currentTarget;
        private byte _frameCount;

        private Damageable _unitDamagable;
        private Damageable _privaryTarget;

        public FindTargetAction(Enemy unit, Damageable privaryTarget)
        {
         //   _searchScope = unit.Prefab.GetComponentInChildren<SearchScope>(true);
            _unitDamagable = unit.Prefab.gameObject.GetComponent<Damageable>();
            //_searchScope.OnTriger += OnSearchScopeEnter;
            _privaryTarget = privaryTarget;
        }

        public void StartAction(Damageable target)
        {
           // _searchScope.gameObject.SetActive(true);
            _frameCount = SEARCH_FRAMES;
            _currentTarget = target;
        }


        private void OnSearchScopeEnter()
        {
            //if (gameObject == null) return;
            //if (!gameObject.CompareTag("Player")) return;
            // current target MainTower
            //if (gameObject.TryGetComponent(out Damageable damageable))
            //{
            //    if (_currentTarget == _privaryTarget)
            //    {
            //        if (_currentTarget != damageable)
            //        {
            //            if (damageable.Attacked(_unitDamagable))
            //            {
            //                _searchScope.gameObject.SetActive(false);
            //                _currentTarget = damageable;
            //            }
            //        }
            //    }                
            // }
            if (CheckLayerBuildings())
            {
                
                var hits = Physics.SphereCastAll(_unitDamagable.transform.position, 0.4f, Vector3.forward, 0.01f, LayerMask.GetMask("Defenders"));
                if (hits.Length != 0)
                {
                    foreach (var hit in hits)
                    {
                        if (hit.transform.gameObject.TryGetComponent<Damageable>(out var damageable))
                        {
                            if (damageable.Attacked(_unitDamagable))
                            {
                                _currentTarget = damageable;
                                return;
                            }
                        }
                    }
                }
                hits = Physics.SphereCastAll(_unitDamagable.transform.position, 0.3f, Vector3.forward, 0.01f, LayerMask.GetMask("Buildings"));
                if (hits.Length != 0)
                {
                    foreach (var hit in hits)
                    {
                        if (hit.transform.gameObject.TryGetComponent<Damageable>(out var damageable))
                        {
                            if (damageable.Attacked(_unitDamagable))
                            {
                                _currentTarget = damageable;
                                return;
                            }
                        }
                    }

                    return;
                }
            }        
        }

        private bool CheckLayerBuildings()
        {
            if (_currentTarget != null)
            {
                if (_currentTarget.gameObject.layer == 10) return true;
            }
            return false;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_frameCount > 0)
            {
                _frameCount--;
                if (_frameCount <= 0)
                {
                    OnSearchScopeEnter();
                //    _searchScope.gameObject.SetActive(false);
                    OnComplete?.Invoke(_currentTarget);
                }
            }
        }

 
    }
}