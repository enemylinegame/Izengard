using System;

public class TimeRemaining: ITimeRemaining
{

    private readonly Action _method;
    
    
    #region ITimeRemaining

    public bool IsRepeating { get; }

    public float Duration { get; private set; }
    
    public float TimeLeft { get; private set; }

    public virtual void Invoke()
    {
        _method?.Invoke();
    }

    public void ChangeRemainingTime(float time)
    {
        TimeLeft = time;
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

public sealed class TimeRemaining<T>: TimeRemaining
{

    private readonly Action<T> _method;

    private readonly T _data;
    
    public override void Invoke()
    {
        _method?.Invoke(_data);
    }

    public TimeRemaining(Action<T> method, T data, float duration, bool isRepeating = false) 
    : base(null, duration, isRepeating)
    {
        _method = method;
        _data = data;
    }

}
