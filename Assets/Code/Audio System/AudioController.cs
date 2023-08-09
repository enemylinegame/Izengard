using System.Collections.Generic;
using UnityEngine;

namespace Audio_System
{
    public class AudioController : IAudioController, IMusicPlayer, IAudioPlayer
    {
        private readonly Dictionary<int, AudioSource> _audioSources;
        private readonly Dictionary<int, AudioSourceData> _sourceMedia;

        private readonly AudioPresenter _presenter;

        private int soundCodeIndex;

        public AudioController(AudioPresenter presenter)
        {
            _presenter = presenter;

            _audioSources = new Dictionary<int, AudioSource>();
            _sourceMedia = new Dictionary<int, AudioSourceData>();

            RegisterSoundSource(_presenter.GloabalMusicSource);
            RegisterSoundSource(_presenter.GloabalUISource);

            soundCodeIndex = 0;
        }

        #region IAudioController

        private bool _soundEnabled;
        private bool _musicEnabled;

        bool IAudioController.SoundEnabled
        {
            get => _soundEnabled;
            set 
            {
                if(_soundEnabled != value)
                {
                    foreach (var key in _sourceMedia.Keys)
                    {
                        var sourceData = _sourceMedia[key];

                        if (!sourceData.IsMusic)
                        {
                            sourceData.Source.volume = value ? sourceData.Volume : 0;
                        }
                    }

                    _soundEnabled = value;
                }
            }
        }

        bool IAudioController.MusicEnabled
        {
            get => _musicEnabled;
            set
            {
                if (_musicEnabled != value)
                {
                    foreach (var key in _sourceMedia.Keys)
                    {
                        var sourceData = _sourceMedia[key];

                        if (sourceData.IsMusic)
                        {
                            sourceData.Source.volume = value ? sourceData.Volume : 0;
                        }
                    }

                    _musicEnabled = value;
                }
            }
        }


        public void RegisterSoundSource(ISoundSource source)
        {
            AddSourceToCollection(source);
        }


        #endregion

        #region IMusicPlayer

        int IMusicPlayer.PlaySound(ISound sound)
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

        int IAudioPlayer.PlaySound3D(ISound sound, Vector3 position, float maxSoundDistance)
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

        private void AddSourceToCollection(ISoundSource source)
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
