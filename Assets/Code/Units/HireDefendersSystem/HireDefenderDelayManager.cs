using System;
using System.Collections.Generic;
using Code.TileSystem;
using CombatSystem;
using UnityEngine;

namespace Code.Units.HireDefendersSystem
{
    public class HireDefenderDelayManager : IOnUpdate
    {

        public Action<TileView, DefenderSettings> OnHireDefendersDelayEnd;

        private Dictionary<TileView, List<HireDefenderDelay>> _queues;

        public HireDefenderDelayManager()
        {
            _queues = new Dictionary<TileView, List<HireDefenderDelay>>();
        }

        public void StartHireDefenderDelay(TileView tileView, DefenderSettings settings, float delay)
        {
            HireDefenderDelay newDelay = new HireDefenderDelay()
            {
                Settings = settings, 
                FinishTime = Time.time + delay 
            };

            if (!_queues.ContainsKey(tileView))
            {
                _queues.Add(tileView, new List<HireDefenderDelay>());
            }
            
            
            
        }

        public void OnUpdate(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            if (_queues.Count > 0)
            {
                foreach (KeyValuePair<TileView, List<HireDefenderDelay>> kvp in _queues)
                {
                    if (kvp.Value.Count > 0)
                    {
                        kvp.Value.Clear();
                    }
                }
                _queues.Clear();
            }
        }
    }
}