using UnityEngine;

namespace Audio_System
{
    [CreateAssetMenu(fileName = nameof(SoundConfig), 
        menuName = "Audio Data/" + nameof(SoundConfig))]
    public class SoundConfig : ScriptableObject, ISound
    {
        [SerializeField] private int _soundCode = 1234;
        
        [Space(10)]
        [SerializeField] private AudioClip _clip;
        [Range(0f, 1f)]
        [SerializeField] private float _volume = 1;
        [Range(0.5f, 2f)]
        [SerializeField] private float _pitch = 1;
        [SerializeField] private bool _isLoop;

        public int SoundCode => _soundCode;
        public AudioClip Clip => _clip;
        public float Volume => _volume;
        public float Pitch => _pitch;
        public bool IsLoop => _isLoop;
    }
}
