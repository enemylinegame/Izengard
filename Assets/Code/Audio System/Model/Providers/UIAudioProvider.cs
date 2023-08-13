using System;

namespace Audio_System
{
    public interface IUIAudioProvider: IDisposable 
    {
        void PlayButtonClickCound();
    }

    public class UIAudioProvider : BaseAudioProvider, IUIAudioProvider
    {
        private readonly string _onClickSoundPath = "UI/OnClickSound";
        
        protected readonly ISoundPlayer _audioPlayer;

        private readonly IAudio _onClickSound;

        private int _currentPlayedSound;

        public UIAudioProvider(ISoundPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;

            _onClickSound = LoadSound(_onClickSoundPath);
        }

        public void PlayButtonClickCound() 
        {
            _currentPlayedSound = _audioPlayer.PlayIn2D(_onClickSound);
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            _audioPlayer.Stop(_currentPlayedSound);
        }
    }
}
