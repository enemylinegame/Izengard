
    using System.Collections.Generic;
    using Code.Time_Remaining;
    using UnityEngine;

    public sealed class TimeRemainingController: IOnController, IOnUpdate
    {

        private TimersHolder _timersHolder;
        
        public TimeRemainingController(TimersHolder timersHolder)
        {
            _timersHolder = timersHolder;
        }


        public void Clear()
        {
            _timersHolder.Timers.Clear();
        }
        
        
        #region IExecute

        public void OnUpdate(float deltatime)
        {
            var time = Time.deltaTime;
            var timers = _timersHolder.Timers;
            for (var i = 0; i < timers.Count; i++)
            {
                var timer = timers[i];
                timer.TimeLeft -= time;
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
                    timer?.Method?.Invoke();
                }
            }
        }
        
        #endregion
    }