using UnityEngine;

namespace Audio_System
{
    public interface ISound
    {
        int AudioSourceCode { get; }
        
        AudioClip Clip { get;}
        float Volume { get;}
        float Pitch { get;}
        bool IsLoop { get;}
    }
}
