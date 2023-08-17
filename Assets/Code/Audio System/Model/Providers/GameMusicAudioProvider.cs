namespace Audio_System
{
    public class GameMusicAudioProvider : BaseAudioProvider
    {
        private readonly string mainMenuMusicPath = "Game/InMainMenuMusic";
        private readonly string inGameMusicPath = "Game/InGameMusic";

        private readonly IMusicPlayer _musicPlayer;

        private readonly IAudio _mainMenuMusic;
        private readonly IAudio _inGameMusic;

        private int _currentMusicId;

        public override int CurrentAudioId 
        {
            get => _currentMusicId;
            protected set 
            {
                if(_currentMusicId != value)
                {
                    _currentMusicId = value;
                }
            }
        }

        public GameMusicAudioProvider(IMusicPlayer musicPlayer)
        {
            _musicPlayer = musicPlayer;

            _mainMenuMusic = LoadMusic(mainMenuMusicPath);
            _inGameMusic = LoadMusic(inGameMusicPath);
        }

        public override void StopCurrentAudio()
        {
            _musicPlayer.Stop(_currentMusicId);
        }

        public void PlayInGameMusic()
        {
            _currentMusicId = _musicPlayer.Play(_mainMenuMusic);
        }

        public void PlayMainMenuMusic()
        {
            _currentMusicId = _musicPlayer.Play(_inGameMusic);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
