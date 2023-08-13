namespace Audio_System
{
    public interface IMusicPlayer
    {
        int Play(IAudio clip);

        void Stop(int audioCode);
   
        void Pause(int audioCode);

        void Resume(int audioCode);
  
        bool IsAudioPlaying(int audioCode);
    }
}
