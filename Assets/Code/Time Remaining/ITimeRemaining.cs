
    using System;
    
    public interface ITimeRemaining
    {
        Action Method { get; }
        bool IsRepeating { get; } 
        float Duration { get; }
        float TimeLeft { get; set; }
    }
    
