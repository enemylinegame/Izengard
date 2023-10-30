using System;

namespace Tools 
{
    public sealed class TimeRemainingController : IOnController, IOnUpdate, IDisposable
    {

        public void Dispose()
        {
            TimersHolder.Clear();
        }

        #region IExecute

        public void OnUpdate(float deltatime)
        {
            for (int i = TimersHolder.Timers.Count - 1; i >= 0; i--)
            {
                ITimeRemaining timer = TimersHolder.Timers[i];
                timer.ChangeRemainingTime(timer.TimeLeft - deltatime);
                if (timer.TimeLeft <= 0.0f)
                {
                    if (!timer.IsRepeating)
                    {
                        TimersHolder.RemoveTimer(timer);
                    }
                    else
                    {
                        timer.ChangeRemainingTime(timer.Duration);
                    }
                    timer.Invoke();
                }
            }
        }

        #endregion
    }
}
