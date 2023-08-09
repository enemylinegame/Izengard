using System;
using System.Collections.Generic;
using Code.TileSystem;
using CombatSystem;
using UnityEngine;

namespace Code.Units.HireDefendersSystem
{
    public class HireDefenderProgressManager : IOnController, IOnUpdate, IOnDisable
    {

        private Action<DefenderPreview, TileModel, DefenderSettings> _finishProgressListener;

        private readonly Dictionary<TileModel, Queue<HireProgress>> _queues = new();

        private readonly Dictionary<DefenderPreview, HireProgress> _defenderProgressTable = new ();

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
                Tile = tile,
                Duration = hireDuration,
                TimePassed = 0.0f
            };
            _queues[tile].Enqueue(newProgress);
            defenderPreview.SetHireProgress(newProgress);
            _defenderProgressTable.Add(defenderPreview, newProgress);
        }
        
        
        public void OnUpdate(float deltaTime)
        {
            foreach (KeyValuePair<TileModel, Queue<HireProgress>> kvp in _queues)
            {
                var queue = kvp.Value;
                if (queue.Count > 0)
                {
                    HireProgress progress = queue.Peek();
                    if (progress.Defender != null)
                    {
                        progress.TimePassed += deltaTime;
                        if (progress.TimePassed >= progress.Duration)
                        {
                            queue.Dequeue();
                            _defenderProgressTable.Remove(progress.Defender);
                            ProgressCompleted(progress, kvp.Key);
                        }
                    }
                    else
                    {
                        queue.Dequeue();
                    }

                }
                
            }
        }

        private void ProgressCompleted(HireProgress progress, TileModel tile)
        {
            progress.Defender.SetHireProgress(null);
            _finishProgressListener?.Invoke(progress.Defender, tile, progress.Defender.Settings);
        }

        public void AddFinishProgressListener(Action<DefenderPreview, TileModel, DefenderSettings> listener)
        {
            _finishProgressListener += listener;
        }

        public void StopDefenderHiringProcess(DefenderPreview defenderPreview)
        {
            if (_defenderProgressTable.TryGetValue(defenderPreview, out HireProgress progress))
            {
                progress.Defender = null;
                _defenderProgressTable.Remove(defenderPreview);
            }
        }

        private void Clear()
        {
            if (_queues.Count > 0)
            {
                foreach (var tileQueue in _queues)
                {
                    var que = tileQueue.Value;
                    while (que.Count > 0)
                    {
                        var hireProgress = que.Dequeue();
                        hireProgress.Defender.SetHireProgress(null);
                        hireProgress.Defender = null;
                        hireProgress.Tile = null;
                    }
                }
                
                _queues.Clear();
            }
        }

        public void OnDisableItself()
        {
            Clear();
        }
    }
}