﻿using System;

public sealed class TimeRemaining: ITimeRemaining
{
    
    #region ITimeRemaining
        
    public Action Method { get; }
    public bool IsRepeating { get; }

    public float Duration { get; private set; }
    
    public float TimeLeft { get; set; }
        
    #endregion


    public TimeRemaining(Action method, float duration, bool isRepeating = false)
    {
        Method = method;
        Duration = duration;
        TimeLeft = duration;
        IsRepeating = isRepeating;
    }

    public void ChangeDuration(float newDuration)
    {
        Duration = newDuration;
    }

}
