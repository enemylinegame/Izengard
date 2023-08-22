
    using System;
    using System.Collections.Generic;
    using Code.Time_Remaining;
    using UnityEngine;

    public sealed class TimeRemainingController: IOnController, IOnUpdate, IDisposable
    {

        private TimersHolder _timersHolder;
        
        public TimeRemainingController(TimersHolder timersHolder)
        {
            _timersHolder = timersHolder;
        }


        public void Dispose()
        {
            _timersHolder.Timers.Clear();
        }
        
        
        #region IExecute

        public void OnUpdate(float deltatime)
        {
            List<ITimeRemaining> timers = _timersHolder.Timers;
            for (int i = 0; i < timers.Count; i++)
            {
                ITimeRemaining timer = timers[i];
                timer.TimeLeft -= deltatime;
                if (timer.TimeLeft <= 0.0f)
                {
                    if (!timer.IsRepeating)
                    {
                        timers.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        timer.TimeLeft = timer.Duration;
                    }
                    timer.Method?.Invoke();
                }
            }
        }
        
        #endregion
    }