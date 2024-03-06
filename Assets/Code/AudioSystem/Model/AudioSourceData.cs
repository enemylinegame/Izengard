using UnityEngine;

namespace AudioSystem
{
    internal class AudioSourceData
    {
        public AudioSource Source;
        public int SoundCode;
        public float Volume;
        public bool Is3DSound;
        public bool OnPause;
        public bool IsMusic;
        public Vector3 SourceRequestedPos;
        public Transform CachedTransform;
    }
}
