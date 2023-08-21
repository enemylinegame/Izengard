
using System;
using System.Collections.Generic;

public class TimersHolder
{

    public List<ITimeRemaining> Timers = new();
    
    private static TimersHolder Instance { get; set; }

    public TimersHolder()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new Exception("Can not create instance of the TimersHolder, already exist.");
        }
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


