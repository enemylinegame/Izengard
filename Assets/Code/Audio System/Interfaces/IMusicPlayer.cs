namespace Audio_System
{
    public interface IMusicPlayer
    {
        int PlaySound(ISound clip);

        void StopSound(int audioCode);
   
        void PauseSound(int audioCode);

        void ResumeSound(int audioCode);
  
        bool IsSoundPlaying(int audioCode);
    }
}
