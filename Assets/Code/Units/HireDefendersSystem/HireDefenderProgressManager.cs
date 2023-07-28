using System;
using System.Collections.Generic;
using Code.TileSystem;
using CombatSystem;

namespace Code.Units.HireDefendersSystem
{
    public class HireDefenderProgressManager : IOnController, IOnUpdate
    {

        private Action<DefenderPreview, TileModel, DefenderSettings> _finishProgressListener;

        private Dictionary<TileModel, Queue<HireProgress>> _queues = new();
        
        
        public void StartDefenderHireProcess(DefenderPreview defenderPreview, TileModel tile, 
            DefenderSettings settings, float hireDuration)
        {
            if (!_queues.ContainsKey(tile))
            {
                _queues.Add(tile, new Queue<HireProgress>());
            }
            
            HireProgress newProgress = new HireProgress()
            {
                Defender = defenderPreview,
                Settings = settings,
                Tile = tile,
                Duration = hireDuration,
                TimePassed = 0.0f
            };
            _queues[tile].Enqueue(newProgress);
            defenderPreview.SetHireProgress(newProgress);
        }
        
        
        public void OnUpdate(float deltaTime)
        {
            foreach (KeyValuePair<TileModel, Queue<HireProgress>> kvp in _queues)
            {
                var queue = kvp.Value;
                if (queue.Count > 0)
                {
                    HireProgress progress = queue.Peek();
                    progress.TimePassed += deltaTime;
                    if (progress.TimePassed >= progress.Duration)
                    {
                        queue.Dequeue();
                        ProgressCompleted(progress, kvp.Key);
                    }
                }
                
            }
        }

        private void ProgressCompleted(HireProgress progress, TileModel tile)
        {
            progress.Defender.SetHireProgress(null);
            _finishProgressListener?.Invoke(progress.Defender, tile, progress.Settings);
        }

        public void AddFinishProgressListener(Action<DefenderPreview, TileModel, DefenderSettings> listener)
        {
            _finishProgressListener += listener;
        }

        public void Clear()
        {
            
        }
        
    }
}