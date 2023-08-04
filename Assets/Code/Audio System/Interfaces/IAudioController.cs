using System;

namespace Audio_System
{
    public interface IAudioController : IDisposable
    {
        bool SoundEnabled { get; set; }
        bool MusicEnabled { get; set; }
        bool VoiceEnabled { get; set; }
        bool EffectsEnabled { get; set; }

        float SoundVolume { get; set; }
        float MusicVolume { get; set; }
        float VoiceVolume { get; set; }
        float EffectsVolume { get; set; }
    }
}
