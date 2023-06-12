using System;
using System.Collections.Generic;
using UnityEngine;

using Wave;


namespace CombatSystem
{
    public class FindTargetAction : IAction<Damageable>, IOnUpdate//, IDisposable
    {
        private const float RAY_CAST_MAX_DISTANCE = 0.01f;
        private const int BUILDING_LAYER_NUMBER = 10;
        private const int SEARCH_FRAMES = 10;
        private const int SERACH_REZULTS_SIZE = 16;
        
        public event Action<Damageable> OnComplete;
        
        private Damageable _currentTarget;
        private readonly Damageable _unitDamageable;
        private readonly Damageable _primaryTarget;
        private RaycastHit[] _searchResults;

        private readonly Vector3 _rayCastDirection = Vector3.forward;
        private readonly LayerMask _mask = LayerMask.GetMask("Defenders", "Buildings");

        private float _searchRadius = 1.4f;
        private EnemyType _type;

        
        private int _maxAttackToDefender = 1;
        private int _maxAttackToBuilding = 5;
        private int _frameCount;
        
        
        public FindTargetAction(Enemy unit, Damageable privaryTarget)
        {
            _unitDamageable = unit.RootGameObject.GetComponent<Damageable>();
            _primaryTarget = privaryTarget;
            _type = unit.Type;
            _searchResults = new RaycastHit[SERACH_REZULTS_SIZE];
        }

        public void StartAction(Damageable target)
        {
            _frameCount = SEARCH_FRAMES;
            _currentTarget = target;
        }

        public void ClearTarget()
        {
            _currentTarget = null;
        }

        private void OnSearchScopeEnter()
        {
            if (CheckCurrentTargetIsBuildingOrNull())
            {
                _currentTarget = _primaryTarget;

                int hitsQuantity = Physics.SphereCastNonAlloc(_unitDamageable.transform.position, _searchRadius,
                    _rayCastDirection, _searchResults, RAY_CAST_MAX_DISTANCE, _mask);
                
                List<Damageable> buildings = new List<Damageable>();
                List<Damageable> units = new List<Damageable>();

                for (int i = 0; i < hitsQuantity; i++)
                {
                    GameObject current = _searchResults[i].transform.gameObject;
                    if (current.layer == BUILDING_LAYER_NUMBER)
                    {
                        if (current.TryGetComponent(out Damageable target))
                        {
                            if (target.NumberOfAttackers < _maxAttackToBuilding && !target.IsDead)
                            {
                                buildings.Add(target);
                            }
                        }
                    }
                    else
                    {
                        if (current.TryGetComponent(out Damageable target))
                        {
                            if (target.NumberOfAttackers < _maxAttackToDefender && !target.IsDead)
                            {
                                units.Add(target);
                            }
                        }
                    }
                }

                if (units.Count > 0)
                {
                    _currentTarget = SelectNearestTarget(units);
                }
                else if (buildings.Count > 0)
                {
                    _currentTarget = SelectNearestTarget(buildings);
                }

                if (_currentTarget != _primaryTarget)
                {
                    _currentTarget.Attacked(_unitDamageable);
                }
                   
            }
        }

        private bool CheckCurrentTargetIsBuildingOrNull()
        {
            if (_currentTarget == null) return true;
            return _currentTarget.gameObject.layer == BUILDING_LAYER_NUMBER;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_frameCount > 0)
            {
                _frameCount--;
                if (_frameCount <= 0)
                {
                    OnSearchScopeEnter();
                    OnComplete?.Invoke(_currentTarget);
                }
            }
        }

        private Damageable SelectNearestTarget(List<Damageable> units)
        {
            Vector3 myPosition = _unitDamageable.transform.position;
            float minSqrDistance = (units[0].Position - myPosition).sqrMagnitude;
            Damageable selectedTarget = units[0];

            for (int i = 1; i < units.Count; i++)
            {
                float sqrDistance = (units[i].transform.position - myPosition).sqrMagnitude;
                if ( sqrDistance  < minSqrDistance )
                {
                    minSqrDistance = sqrDistance;
                    selectedTarget = units[i];
                }
            }

            return selectedTarget;
        }
 
    }
}