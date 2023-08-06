using System.Collections.Generic;
using UnityEngine;

namespace Audio_System
{

    public class AudioController : IAudioController, IMusicPlayer, IAudioPlayer
    {
        private readonly AudioSource _musicAudioSource;

        private readonly Dictionary<int, AudioSourceData> _sourceMedia;

        public AudioController(AudioManager audioManager)
        {
            _musicAudioSource = audioManager.MusicAudioSource;
            _sourceMedia = new Dictionary<int, AudioSourceData>();
        }


        #region IAudioController

        bool IAudioController.SoundEnabled
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }

        bool IAudioController.MusicEnabled
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }

        bool IAudioController.VoiceEnabled
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }

        bool IAudioController.EffectsEnabled
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }

        float IAudioController.SoundVolume
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }

        float IAudioController.MusicVolume
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }

        float IAudioController.VoiceVolume
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }

        float IAudioController.EffectsVolume
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }


        #endregion

        #region IMusicPlayer

        int IMusicPlayer.PlaySound(ISound sound)
        {
            ScanForEndedSources();

            _musicAudioSource.clip = sound.Clip;

            var source = _musicAudioSource;
            source.clip = sound.Clip;
            source.volume = sound.Volume;
            source.pitch = sound.Pitch;
            source.loop = sound.IsLoop;
            source.priority = 0;
            source.spatialBlend = 0;

            var mediaData = new AudioSourceData
            {
                Source = source,
                SoundCode = sound.SoundCode,
                Volume = sound.Volume,
                OnPause = false,
                Is3DSound = false,
                IsMusic = true,
                SourceRequestedPos = Vector3.one,
            };

            source.Play();

            _sourceMedia.Add(mediaData.SoundCode, mediaData);

            return mediaData.SoundCode;
        }

        void IMusicPlayer.StopSound(int audioCode)
        {
            if (!_sourceMedia.ContainsKey(audioCode))
                return;

            var source = _sourceMedia[audioCode];
            _sourceMedia.Remove(audioCode);
            source.Source.Stop();
        }

        void IMusicPlayer.PauseSound(int audioCode)
        {
            if (!_sourceMedia.ContainsKey(audioCode))
                return;

            var source = _sourceMedia[audioCode];
            source.Source.Pause();
            source.OnPause = true;
        }


        void IMusicPlayer.ResumeSound(int audioCode)
        {
            if (!_sourceMedia.ContainsKey(audioCode))
                return;

            var source = _sourceMedia[audioCode];

            if (source.OnPause == false)
                return;
   
            source.Source.UnPause();
            source.OnPause = false;
        }


        bool IMusicPlayer.IsSoundPlaying(int audioCode)
        {
            if (!_sourceMedia.ContainsKey(audioCode))
                return false;

            var source = _sourceMedia[audioCode];
            return source.Source.isPlaying;
        }

        #endregion

        #region IAudioPlayer

        int IAudioPlayer.PlaySound2D(ISound sound)
        {
            ScanForEndedSources();

            return 0;
        }
 
        int IAudioPlayer.PlaySound3D(ISound sound, Vector3 position, float maxSoundDistance)
        {
            ScanForEndedSources();

            return 0;
        }

        void IAudioPlayer.StopSound(int audioCode)
        {
            if (!_sourceMedia.ContainsKey(audioCode))
                return;

            var source = _sourceMedia[audioCode];
            _sourceMedia.Remove(audioCode);
            source.Source.Stop();
        }

        bool IAudioPlayer.IsSoundPlaying(int audioCode)
        {
            if (!_sourceMedia.ContainsKey(audioCode))
                return false;

            var source = _sourceMedia[audioCode];
            return source.Source.isPlaying;
        }

        void IAudioPlayer.SetSourcePositionTo(int audioCode, Vector3 destinationPos)
        {
            throw new System.NotImplementedException();
        }

        void IAudioPlayer.SetAudioListenerToPosition(Vector3 position)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private void ScanForEndedSources()
        {
            var toDispose = new Dictionary<int, AudioSourceData>();

            foreach (var key in _sourceMedia.Keys)
            {
                var source = _sourceMedia[key];
                if (!source.OnPause && !source.Source.isPlaying)
                    toDispose.Add(key, source);
            }

            foreach (var key in toDispose.Keys)
            {
                var source = toDispose[key];
                source.Source.Stop();

                _sourceMedia.Remove(key);

                if (source.Is3DSound && !source.IsMusic)
                    DestroySource(source.Source.gameObject);
            }
        }

        private void DestroySource(Object source) 
            => Object.Destroy(source);

        public void Dispose()
        {
            _sourceMedia.Clear();
        }

    }
}
