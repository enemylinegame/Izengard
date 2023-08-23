using System.Collections.Generic;


public static class TimersHolder
{
    private static List<ITimeRemaining> _timers = new();
    
    public static IReadOnlyList<ITimeRemaining> Timers => _timers;

    public static void AddTimer(ITimeRemaining timer)
    {
        if (timer != null)
        {
            if (!_timers.Contains(timer))
            {
                _timers.Add(timer);
            }
        }
    }

    public static void RemoveTimer(ITimeRemaining timer)
    {
        if (timer != null)
        {
            _timers.Remove(timer);
        }
    }

}
