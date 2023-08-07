using UnityEngine;

namespace Audio_System
{
    public class AudioPresenter : MonoBehaviour
    {
        [SerializeField] private SoundSource _gloabalMusicSource;
        [SerializeField] private SoundSource _gloabalUISource;

        public ISoundSource GloabalMusicSource => _gloabalMusicSource;
        public ISoundSource GloabalUISource => _gloabalUISource;
    }
}
