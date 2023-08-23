﻿    using System;
    using System.Collections.Generic;


    public sealed class TimeRemainingController: IOnController, IOnUpdate, IDisposable
    {
        
        public void Dispose()
        {
            //GetTimersList().Clear();
        }
        
        #region IExecute

        public void OnUpdate(float deltatime)
        {
            for (int i = TimersHolder.Timers.Count - 1; i >= 0; i--)
            {
                ITimeRemaining timer = TimersHolder.Timers[i];
                timer.TimeLeft -= deltatime;
                if (timer.TimeLeft <= 0.0f)
                {
                    if (!timer.IsRepeating)
                    {
                        TimersHolder.RemoveTimer(timer);
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