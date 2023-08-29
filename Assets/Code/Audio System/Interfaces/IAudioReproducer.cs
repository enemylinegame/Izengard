using System;

namespace Audio_System
{
    public interface IAudioProvider : IDisposable
    {
        int CurrentAudioId { get; }

        void StopCurrentAudio();
    }
}
