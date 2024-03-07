namespace AudioSystem
{
    public class UIAudioProvider : BaseAudioProvider
    {
        private readonly string _onClickSoundPath = "UI/OnClickSound";
        
        protected readonly ISoundPlayer _audioPlayer;

        private readonly IAudio _onClickSound;

        private int _currentSoundId;

        public override int CurrentAudioId
        {
            get => _currentSoundId;
            protected set
            {
                if (_currentSoundId != value)
                {
                    _currentSoundId = value;
                }
            }
        }

        public UIAudioProvider(ISoundPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
              
            _onClickSound = LoadSound(_onClickSoundPath);
        }

        public override void StopCurrentAudio()
        {
            _audioPlayer.Stop(_currentSoundId);
        }

        public void PlayButtonClickCound() 
        {
            _currentSoundId = _audioPlayer.PlayIn2D(_onClickSound);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
