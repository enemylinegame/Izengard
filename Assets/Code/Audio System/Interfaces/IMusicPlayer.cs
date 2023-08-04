using UnityEngine;

namespace Audio_System
{
    public interface IMusicPlayer
    {
        int PlayClip(AudioClip clip, float volumeValue= 1f);

        void StopClip(int audioCode);
   
        void PauseClip(int audioCode);

        void ResumeClip(int audioCode);
  
        bool IsClipPlaying(int audioCode);
    }
}
