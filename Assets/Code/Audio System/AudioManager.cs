using UnityEngine;
using UnityEngine.Audio;

namespace Audio_System
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioSource _musicAudioSource;
        [SerializeField] private SoundList _gameSounds;

        public AudioMixer AudioMixer => _audioMixer;
        public AudioSource MusicAudioSource => _musicAudioSource;
        public SoundList GameSounds => _gameSounds;
    }
}
