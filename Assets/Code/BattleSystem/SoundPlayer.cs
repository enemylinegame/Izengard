using System;
using UnityEngine;

namespace Code.Game
{
    public class SoundPlayer
    {
        private AudioSource _clickAudioSource;

        public SoundPlayer( AudioSource clickAudioSource)
        {
            _clickAudioSource = clickAudioSource;
        }

        public void PlayClickSound()
        {
            _clickAudioSource.Play();
        }

        public void PlaySound(int soundID)
        {
            throw new NotImplementedException();
        }
        
    }
}