
    using System;
    
    public interface ITimeRemaining
    {
        Action Method { get; }
        bool IsRepeating { get; } float Time { get; }
        float CurrentTime { get; set; }
    }
    
