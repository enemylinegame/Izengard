using System;

public sealed class TimeRemaining: ITimeRemaining
{

    private readonly Action _method;
    
    
    #region ITimeRemaining

    public bool IsRepeating { get; }

    public float Duration { get; private set; }
    
    public float TimeLeft { get; set; }

    public void Invoke()
    {
        _method?.Invoke();
    }
    
    #endregion


    public TimeRemaining(Action method, float duration, bool isRepeating = false)
    {
        _method = method;
        Duration = duration;
        TimeLeft = duration;
        IsRepeating = isRepeating;
    }

    public void ChangeDuration(float newDuration)
    {
        TimeLeft += (newDuration - Duration);
        Duration = newDuration;
    }

}

public sealed class TimeRemaining<T>: ITimeRemaining
{

    private readonly Action<T> _method;

    private T _data;
    

    #region ITimeRemaining

    public bool IsRepeating { get; }

    public float Duration { get; private set; }
    
    public float TimeLeft { get; set; }

    public void Invoke()
    {
        _method?.Invoke(_data);
    }

    #endregion


    public TimeRemaining(Action<T> method, T data, float duration, bool isRepeating = false)
    {
        _method = method;
        _data = data;
        Duration = duration;
        TimeLeft = duration;
        IsRepeating = isRepeating;
    }

    public void ChangeDuration(float newDuration)
    {
        TimeLeft += (newDuration - Duration);
        Duration = newDuration;
    }

}
