using System.Collections.Generic;


public static class TimersHolder
{
    private static List<ITimeRemaining> _timers = new();
    
    public static bool AddTimer(TimeRemaining timer)
    {
        bool isAdded = false;

        if (timer != null)
        {
            if (!_timers.Contains(timer))
            {
                _timers.Add(timer);
                isAdded = true;
            }
        }

        return isAdded;
    }

    public static bool RemoveTimer(TimeRemaining timer)
    {
        if (timer != null)
        {
            return _timers.Remove(timer);
        }

        return false;
    }

    public static void SetTimeRemainingController(TimeRemainingController controller)
    {
        controller.GetTimersList += GetTimersList;
    }

    private static List<ITimeRemaining> GetTimersList()
    {
        return _timers;
    }
    
}
