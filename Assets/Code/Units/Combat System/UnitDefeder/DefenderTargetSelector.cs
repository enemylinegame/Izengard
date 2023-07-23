﻿using System.Collections.Generic;
using CombatSystem.Interfaces;
using UnityEngine;


namespace CombatSystem
{
    public class DefenderTargetSelector
    {
        private readonly DefenderTargetsHolder _targetHolder;
        private readonly Transform _transform;

        
        public DefenderTargetSelector(GameObject defenderGameObject, DefenderTargetsHolder holder)
        {
            _transform = defenderGameObject.transform;
            _targetHolder = holder;
        }
        
        
        /// <summary>
        /// Select target. Set selected target
        /// to DefenderTargetsHolder.CurrentTarget.
        /// null target - no any target. 
        /// </summary>
        /// <returns> selected target </returns>
        public IDamageable SelectTarget()
        {
            IDamageable selectedTarget = null;

            List<IDamageable> targets = new List<IDamageable>();

            SelectMaxThreatTargets(targets, _targetHolder.TargetsInRange);
            SelectMaxThreatTargets(targets, _targetHolder.AttackingTargets);

            selectedTarget = SelectNearestTarget(targets);

            if (selectedTarget != null)
            {
                _targetHolder.CurrentTarget = selectedTarget;
            }
            else
            {
                IDamageable target = _targetHolder.CurrentTarget;
                if (target != null && target.IsDead)
                {
                    _targetHolder.CurrentTarget = null;
                }
                selectedTarget = _targetHolder.CurrentTarget;
            }

            return selectedTarget;
        }

        private void SelectMaxThreatTargets(List<IDamageable> toList, List<IDamageable> fromList)
        {
            int maxThreatLevel = 0;
            if (toList.Count > 0)
            {
                maxThreatLevel = toList[0].ThreatLevel;
            }
            
            for (int i = 0; i < fromList.Count; i++)
            {
                IDamageable current = fromList[i];
                if (!current.IsDead)
                {
                    int currentThreatLevel = current.ThreatLevel;
                    if (currentThreatLevel > maxThreatLevel)
                    {
                        toList.Clear();
                        toList.Add(current);
                        maxThreatLevel = currentThreatLevel;
                    }
                    else if (currentThreatLevel == maxThreatLevel)
                    {
                        toList.Add(current);
                    }
                }
            }
        }

        private IDamageable SelectNearestTarget(List<IDamageable> targets)
        {
            if (targets.Count == 0)
            {
                return null;
            }

            if (targets.Count == 1)
            {
                return targets[0];
            }

            IDamageable selectedTarget = targets[0];
            Vector3 myPosition = _transform.position;
            myPosition.y = 0.0f;
            float minSqrDistance = float.MaxValue;

            for (int i = 0; i < targets.Count; i++)
            {
                Vector3 targetPosition = targets[i].Position;
                targetPosition.y = 0.0f;
                float currentSqrDistance = (myPosition - targetPosition).sqrMagnitude;
                if ( currentSqrDistance < minSqrDistance )
                {
                    minSqrDistance = currentSqrDistance;
                    selectedTarget = targets[i];
                }
            }

            return selectedTarget;
        }

    }
}