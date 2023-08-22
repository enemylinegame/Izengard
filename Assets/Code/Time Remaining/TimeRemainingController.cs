    using System;
    using System.Collections.Generic;


    public sealed class TimeRemainingController: IOnController, IOnUpdate, IDisposable
    {
        public Func<List<ITimeRemaining>> GetTimersList;

        public void Dispose()
        {
            GetTimersList().Clear();
        }
        
        
        #region IExecute

        public void OnUpdate(float deltatime)
        {
            List<ITimeRemaining> timers = GetTimersList();
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