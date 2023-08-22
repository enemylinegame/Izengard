
    using System;
    using System.Collections.Generic;
    using Code.Time_Remaining;
    using UnityEngine;

    public sealed class TimeRemainingController: IOnController, IOnUpdate, IDisposable
    {


        public void Dispose()
        {
            TimersHolder.Timers.Clear();
        }
        
        
        #region IExecute

        public void OnUpdate(float deltatime)
        {
            List<ITimeRemaining> timers = TimersHolder.Timers;
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