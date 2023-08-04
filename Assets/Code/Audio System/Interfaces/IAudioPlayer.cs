using UnityEngine;

namespace Audio_System
{
    public interface IAudioPlayer
    {
        int PlayAudioClip2D(AudioClip clip, float volumeValue = 1f, bool looped = false);

        int PlayAudioClip3D(AudioClip clip, Vector3 position, float maxSoundDistance, float volumeValue = 1f, bool looped = false);

        void StopClip(int audioCode);

        bool IsAudioClipPlaying(int audioCode);

        void SetAudioListenerToPosition(Vector3 position);
 
        void SetSourcePositionTo(int audioCode, Vector3 destinationPos);

    }
}
