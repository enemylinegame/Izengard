using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    public class AudioController : IAudioController, IMusicPlayer, ISoundPlayer
    {
        private readonly Dictionary<int, AudioSource> _audioSources;
        private readonly Dictionary<int, AudioSourceData> _sourceMedia;

        private readonly AudioPresenter _presenter;

        private int soundCodeIndex;

        public AudioController()
        {
            _audioSources = new Dictionary<int, AudioSource>();
            _sourceMedia = new Dictionary<int, AudioSourceData>();

            var initAudioController = (IAudioController)this;
            initAudioController.SoundEnabled = true;
            initAudioController.MusicEnabled = true;

            soundCodeIndex = 0;
        }


        private bool _soundEnabled;
        private bool _musicEnabled;

        public bool SoundEnabled
        {
            get => _soundEnabled;
            set 
            {
                if(_soundEnabled != value)
                {
                    foreach (var key in _sourceMedia.Keys)
                    { 
                        _soundEnabled = value;

                        var sourceData = _sourceMedia[key];

                        if (!sourceData.IsMusic)
                        {
                            sourceData.Source.volume = _soundEnabled ? sourceData.Volume : 0;
                        }
                    }                 
                }
            }
        }

        public bool MusicEnabled
        {
            get => _musicEnabled;
            set
            {
                if (_musicEnabled != value)
                {
                    foreach (var key in _sourceMedia.Keys)
                    {
                        _musicEnabled = value;

                        var sourceData = _sourceMedia[key];
                        
                        if (sourceData.IsMusic)
                        {
                            sourceData.Source.volume = _musicEnabled ? sourceData.Volume : 0;
                        }
                    }                   
                }
            }
        }


        public void RegisterAudioSource(IAudioSource source)
        {
            AddSourceToCollection(source);
        }


        #region IMusicPlayer

        int IMusicPlayer.Play(IAudio sound)
        {
            ScanForEndedSources();

            soundCodeIndex++;

            var source = _audioSources[sound.AudioSourceCode];

            source.clip = sound.Clip;
            source.volume = sound.Volume;
            source.pitch = sound.Pitch;
            source.loop = sound.IsLoop;
            source.priority = 0;
            source.spatialBlend = 0;

            var mediaData = new AudioSourceData
            {
                Source = source,
                SoundCode = soundCodeIndex,
                Volume = sound.Volume,
                OnPause = false,
                Is3DSound = false,
                IsMusic = true,
                SourceRequestedPos = Vector3.one,
            };

            source.Play();

            _sourceMedia.Add(soundCodeIndex, mediaData);

            return soundCodeIndex;
        }

        void IMusicPlayer.Stop(int audioCode)
        {
            if (!_sourceMedia.ContainsKey(audioCode))
                return;

            var source = _sourceMedia[audioCode];
            _sourceMedia.Remove(audioCode);
            source.Source.Stop();
        }

        void IMusicPlayer.Pause(int audioCode)
        {
            if (!_sourceMedia.ContainsKey(audioCode))
                return;

            var source = _sourceMedia[audioCode];
            source.Source.Pause();
            source.OnPause = true;
        }


        void IMusicPlayer.Resume(int audioCode)
        {
            if (!_sourceMedia.ContainsKey(audioCode))
                return;

            var source = _sourceMedia[audioCode];

            if (source.OnPause == false)
                return;

            source.Source.UnPause();
            source.OnPause = false;
        }


        bool IMusicPlayer.IsAudioPlaying(int audioCode)
        {
            if (!_sourceMedia.ContainsKey(audioCode))
                return false;

            var source = _sourceMedia[audioCode];
            return source.Source.isPlaying;
        }

        #endregion

        #region IAudioPlayer

        int ISoundPlayer.PlayIn2D(IAudio sound)
        {
            ScanForEndedSources();

            soundCodeIndex++;

            var source = _audioSources[sound.AudioSourceCode];

            source.clip = sound.Clip;
            source.volume = sound.Volume;
            source.pitch = sound.Pitch;
            source.loop = sound.IsLoop;
            source.priority = 0;
            source.spatialBlend = 0;

            var mediaData = new AudioSourceData
            {
                Source = source,
                SoundCode = soundCodeIndex,
                Volume = sound.Volume,
                OnPause = false,
                Is3DSound = false,
                IsMusic = false,
                SourceRequestedPos = Vector3.one,
            };

            source.Play();

            _sourceMedia.Add(soundCodeIndex, mediaData);

            return soundCodeIndex;
        }

        int ISoundPlayer.PlayIn3D(IAudio sound, Vector3 position, float maxSoundDistance)
        {
            ScanForEndedSources();

            soundCodeIndex++;

            var source = _audioSources[sound.AudioSourceCode];

            source.clip = sound.Clip;
            source.volume = sound.Volume;
            source.pitch = sound.Pitch;
            source.loop = sound.IsLoop;

            source.spatialBlend = 1;

            source.minDistance = 0.4f;
            source.rolloffMode = AudioRolloffMode.Logarithmic;
            source.maxDistance = maxSoundDistance;

            var mediaData = new AudioSourceData
            {
                Source = source,
                SoundCode = soundCodeIndex,
                Volume = sound.Volume,
                OnPause = false,
                Is3DSound = true,
                IsMusic = false,
                SourceRequestedPos = position,
                CachedTransform = source.transform
            };

            source.Play();

            _sourceMedia.Add(soundCodeIndex, mediaData);

            return soundCodeIndex;
        }

        void ISoundPlayer.Stop(int audioCode)
        {
            if (!_sourceMedia.ContainsKey(audioCode))
                return;

            var source = _sourceMedia[audioCode];
            _sourceMedia.Remove(audioCode);
            source.Source.Stop();
        }

        bool ISoundPlayer.IsSoundPlaying(int audioCode)
        {
            if (!_sourceMedia.ContainsKey(audioCode))
                return false;

            var source = _sourceMedia[audioCode];
            return source.Source.isPlaying;
        }

        void ISoundPlayer.SetSourcePositionTo(int audioCode, Vector3 destinationPos)
        {
            throw new System.NotImplementedException();
        }

        void ISoundPlayer.SetAudioListenerToPosition(Vector3 position)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private void AddSourceToCollection(IAudioSource source)
        {
            if (_audioSources.ContainsKey(source.SourceCode))
                return;

            _audioSources[source.SourceCode] = source.AudioSource;
        }

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
            _audioSources.Clear();
            _sourceMedia.Clear();
        }


    }
}
