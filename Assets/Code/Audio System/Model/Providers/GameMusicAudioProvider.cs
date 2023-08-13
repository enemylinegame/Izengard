using System;

namespace Audio_System
{
    public interface IGameMusicAudioProvider : IDisposable
    {
        void PlayMainMenuMusic();
        void PlayInGameMusic();
    }

    public class GameMusicAudioProvider : BaseAudioProvider, IGameMusicAudioProvider
    {
        private readonly string mainMenuMusicPath = "Game/InMainMenuMusic";
        private readonly string inGameMusicPath = "Game/InGameMusic";

        private readonly IMusicPlayer _musicPlayer;

        private readonly IAudio _mainMenuMusic;
        private readonly IAudio _inGameMusic;

        private int _currentPlayedMusic;

        public GameMusicAudioProvider(IMusicPlayer musicPlayer)
        {
            _musicPlayer = musicPlayer;

            _mainMenuMusic = LoadMusic(mainMenuMusicPath);
            _inGameMusic = LoadMusic(inGameMusicPath);
        }

        public void PlayInGameMusic()
        {
            _currentPlayedMusic = _musicPlayer.Play(_mainMenuMusic);
        }

        public void PlayMainMenuMusic()
        {
            _currentPlayedMusic = _musicPlayer.Play(_inGameMusic);
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            _musicPlayer.Stop(_currentPlayedMusic);
        }
    }
}
