using UnityEngine;

namespace Audio_System
{
    public interface ISoundSource
    {
        int SourceCode { get; }
        AudioSource AudioSource { get; }
    }
}
