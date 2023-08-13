using UnityEngine;

namespace Audio_System
{
    public interface IAudioSource
    {
        int SourceCode { get; }
        AudioSource AudioSource { get; }
    }
}
