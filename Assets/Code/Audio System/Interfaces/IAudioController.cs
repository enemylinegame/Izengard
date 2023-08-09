using System;

namespace Audio_System
{
    public interface IAudioController : IDisposable
    {
        bool SoundEnabled { get; set; }
        bool MusicEnabled { get; set; }

        void RegisterSoundSource(ISoundSource source);
    }
}
