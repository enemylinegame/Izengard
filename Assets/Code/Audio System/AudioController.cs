
using UnityEngine;

namespace Audio_System
{
    public class AudioController : IAudioController, IMusicPlayer, IAudioPlayer
    {
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

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        #region IMusicPlayer

        bool IMusicPlayer.IsClipPlaying(int audioCode)
        {
            throw new System.NotImplementedException();
        }

        void IMusicPlayer.PauseClip(int audioCode)
        {
            throw new System.NotImplementedException();
        }

        int IMusicPlayer.PlayClip(AudioClip clip, float volumeValue)
        {
            throw new System.NotImplementedException();
        }

        void IMusicPlayer.ResumeClip(int audioCode)
        {
            throw new System.NotImplementedException();
        }

        void IMusicPlayer.StopClip(int audioCode)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IAudioPlayer

        bool IAudioPlayer.IsAudioClipPlaying(int audioCode)
        {
            throw new System.NotImplementedException();
        }

        int IAudioPlayer.PlayAudioClip2D(AudioClip clip, float volumeValue, bool looped)
        {
            throw new System.NotImplementedException();
        }

        int IAudioPlayer.PlayAudioClip3D(AudioClip clip, Vector3 position, float maxSoundDistance, float volumeValue, bool looped)
        {
            throw new System.NotImplementedException();
        }

        void IAudioPlayer.SetAudioListenerToPosition(Vector3 position)
        {
            throw new System.NotImplementedException();
        }

        void IAudioPlayer.SetSourcePositionTo(int audioCode, Vector3 destinationPos)
        {
            throw new System.NotImplementedException();
        }

        void IAudioPlayer.StopClip(int audioCode)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
