using UnityEngine;

namespace AudioSystem
{
    public interface IAudioSource
    {
        int SourceCode { get; }
        AudioSource AudioSource { get; }
    }
}
