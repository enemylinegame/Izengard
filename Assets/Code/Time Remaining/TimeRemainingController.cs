
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class TimeRemainingController: IOnController, IOnUpdate
    {

        private readonly List<ITimeRemaining> _timeRemainings;
        

        public TimeRemainingController()
        {
            _timeRemainings = TimeRemainingExtensions.TimeRemainings;
        }


        public void Clear()
        {
            _timeRemainings.Clear();
        }
        
        
        #region IExecute

        public void OnUpdate(float deltatime)
        {
            var time = Time.deltaTime;
            for (var i = 0; i < _timeRemainings.Count; i++)
            {
                var obj = _timeRemainings[i];
                obj.CurrentTime -= time;
                if (obj.CurrentTime <= 0.0f)
                {
                    if (!obj.IsRepeating)
                    {
                        obj.RemoveTimeRemaining();
                    }
                    else
                    {
                        obj.CurrentTime = obj.Time;
                    }
                    obj?.Method?.Invoke();
                }
            }
        }
        
        #endregion
    }