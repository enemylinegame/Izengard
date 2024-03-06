using System;

namespace AudioSystem
{
    public interface IAudioProvider : IDisposable
    {
        int CurrentAudioId { get; }

        void StopCurrentAudio();
    }
}
