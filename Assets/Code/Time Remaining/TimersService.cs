

using Code.Time_Remaining;

public static class TimersService
{

    public static bool AddTimer(TimeRemaining timer)
    {
        bool isAdded = false;

        if (timer != null)
        {
            if (!TimersHolder.Timers.Contains(timer))
            {
                TimersHolder.Timers.Add(timer);
                isAdded = true;
            }
        }

        return isAdded;
    }

    public static bool RemoveTimer(TimeRemaining timer)
    {
        if (timer != null)
        {
            return TimersHolder.Timers.Remove(timer);
        }

        return false;
    }
}
