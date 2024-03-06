using UnityEngine;

namespace AudioSystem
{
    public class AudioPresenter : MonoBehaviour
    {
        [SerializeField] private bool _soundEnabled = true;
        [SerializeField] private bool _musicEnabled = true;

        [SerializeField] private AudioSourceComponent _gloabalMusicSource;
        [SerializeField] private AudioSourceComponent _gloabalUISource;
        public bool SoundEnabled => _soundEnabled;
        public bool MusicEnabled => _musicEnabled;

        public IAudioSource GloabalMusicSource => _gloabalMusicSource;
        public IAudioSource GloabalUISource => _gloabalUISource; 
    }
}
