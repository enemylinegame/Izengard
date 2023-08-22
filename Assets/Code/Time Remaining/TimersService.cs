

using Code.Time_Remaining;

public static class TimersService
{

    public static TimersHolder TamersContainer;

    public static bool AddTimer(TimeRemaining timer)
    {
        bool isAdded = false;

        if (TamersContainer != null && timer != null)
        {
            if (!TamersContainer.Timers.Contains(timer))
            {
                TamersContainer.Timers.Add(timer);
                isAdded = true;
            }
        }

        return isAdded;
    }

    public static bool RemoveTimer(TimeRemaining timer)
    {
        if (TamersContainer != null && timer != null)
        {
            return TamersContainer.Timers.Remove(timer);
        }

        return false;
    }
}
