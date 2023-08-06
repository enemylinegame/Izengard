using UnityEngine;

namespace Audio_System
{
    public interface ISound
    {
        int SoundCode { get; }
        
        AudioClip Clip { get;}
        float Volume { get;}
        float Pitch { get;}
        bool IsLoop { get;}
    }
}
