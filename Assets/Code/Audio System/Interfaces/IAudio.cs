using UnityEngine;

namespace Audio_System
{
    public interface IAudio
    {
        int AudioSourceCode { get; }
        
        AudioClip Clip { get;}
        float Volume { get;}
        float Pitch { get;}
        bool IsLoop { get;}
    }
}
