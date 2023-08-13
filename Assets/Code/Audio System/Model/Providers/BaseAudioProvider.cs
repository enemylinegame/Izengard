using System;
using UnityEngine;

namespace Audio_System
{
    public abstract class BaseAudioProvider: IDisposable
    {
        private readonly string _soundRootResources = "AudioData/Sound";
        private readonly string _musicRootResourcesName = "AudioData/Music";

        public BaseAudioProvider() { }

        protected IAudio LoadSound(string soundPath)
        {
            var fullPath = $"{_soundRootResources}/{soundPath}";
            return GetAudio(fullPath);
        }
        
        protected IAudio LoadMusic(string musicPath)
        {
            var fullPath = $"{_musicRootResourcesName}/{musicPath}";
            return GetAudio(fullPath);
        }

        private IAudio GetAudio(string path) 
            => Resources.Load<AudioConfig>(path);

        #region IDisposable

        private bool _isDisposed;

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            OnDispose();
        }

        protected virtual void OnDispose() { }

        #endregion
    }
}
