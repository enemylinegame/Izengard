using UnityEngine;

namespace Audio_System
{
    public class AudioReproducerComponent : MonoBehaviour, IAudioReproducer
    {
        [SerializeField] private SoundConfig _sound;
        [SerializeField] private SoundSource _soundSource;
     
        public ISound SoundData => _sound;
        public ISoundSource SoundSourceData => _soundSource;
    }

    [System.Serializable]
    public class SoundSource : ISoundSource
    {
        [field: SerializeField] public int SourceCode { get; private set; } = 1111;

        [field: SerializeField] public AudioSource AudioSource { get; private set; }
    }
}
