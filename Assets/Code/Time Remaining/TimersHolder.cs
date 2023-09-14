using System.Collections.Generic;


public static class TimersHolder
{
    private static List<ITimeRemaining> _timers = new();
    private static List<ITimeRemaining> _pausedTimers = new();

    public static IReadOnlyList<ITimeRemaining> Timers => _timers;

    public static void AddTimer(ITimeRemaining timer)
    {
        if (timer != null)
        {
            if (!_timers.Contains(timer) && !_pausedTimers.Contains(timer) )
            {
                _timers.Add(timer);
            }
        }
    }

    public static void RemoveTimer(ITimeRemaining timer)
    {
        if (timer != null)
        {
            if (!_timers.Remove(timer))
            {
                _pausedTimers.Remove(timer);
            }
        }
    }

    public static void PauseTimer(ITimeRemaining timer)
    {
        int index = _timers.IndexOf(timer);
        if (index >= 0)
        {
            _timers.RemoveAt(index);
            _pausedTimers.Add(timer);
        }
    }

    public static void ResumeTimer(ITimeRemaining timer)
    {
        int index = _pausedTimers.IndexOf(timer);
        if (index >= 0)
        {
            _pausedTimers.RemoveAt(index);
            _timers.Add(timer);
        }
    }

    public static void Clear()
    {
        _timers.Clear();
        _pausedTimers.Clear();
    }
    
}
