
using System;
using System.Collections.Generic;

public class TimersHolder
{

    public List<ITimeRemaining> Timers;
    
    private static TimersHolder Instance { get; set; }

    public TimersHolder()
    {
        if (Instance == null)
        {
            Timers = new();
        }
        else
        {
            Timers = Instance.Timers;
        }
        Instance = this;
    }

    public static bool AddTimer(TimeRemaining timer )
    {
        bool isAdded = false;

        if (Instance != null && timer != null)
        {
            if (!Instance.Timers.Contains(timer))
            {
                Instance.Timers.Add(timer);
                isAdded = true;
            }
        }
        
        return isAdded;
    }

    public static bool RemoveTimer(TimeRemaining timer)
    {
        if (Instance != null && timer != null)
        {
            return Instance.Timers.Remove(timer);
        }
        return false;
    }
    
}


