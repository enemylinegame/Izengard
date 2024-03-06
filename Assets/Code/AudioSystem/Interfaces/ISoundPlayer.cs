using UnityEngine;

namespace AudioSystem
{
    public interface ISoundPlayer
    {
        int PlayIn2D(IAudio sound);

        int PlayIn3D(IAudio sound, Vector3 position, float maxSoundDistance);

        void Stop(int audioCode);

        bool IsSoundPlaying(int audioCode);

        void SetAudioListenerToPosition(Vector3 position);
 
        void SetSourcePositionTo(int audioCode, Vector3 destinationPos);

    }
}
