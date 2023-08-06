using UnityEngine;

namespace Audio_System
{
    public interface IAudioPlayer
    {
        int PlaySound2D(ISound sound);

        int PlaySound3D(ISound sound, Vector3 position, float maxSoundDistance);

        void StopSound(int audioCode);

        bool IsSoundPlaying(int audioCode);

        void SetAudioListenerToPosition(Vector3 position);
 
        void SetSourcePositionTo(int audioCode, Vector3 destinationPos);

    }
}
