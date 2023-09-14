    public interface ITimeRemaining
    {
        bool IsRepeating { get; } 
        float Duration { get; }
        float TimeLeft { get; }
        void Invoke();
        void ChangeRemainingTime(float time);
    }
    
