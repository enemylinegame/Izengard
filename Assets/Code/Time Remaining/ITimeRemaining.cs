    public interface ITimeRemaining
    {
        bool IsRepeating { get; } 
        float Duration { get; }
        float TimeLeft { get; set; }
        void Invoke();
    }
    
