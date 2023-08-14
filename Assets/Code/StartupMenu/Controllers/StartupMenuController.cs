using Audio_System;
using Code.Game;
using UnityEngine;
using UnityEngine.Audio;

namespace StartupMenu
{
    public class StartupMenuController : BaseController
    {

        private readonly Transform _placeForUi;
        private readonly ISettingsData _baseSettingsData;
        private readonly AudioMixer _audioMixer;

        private readonly StateModel _startupSceneState;

        private readonly GameSettingsManager _settingsManager;
        
        private readonly AudioController _audioController;
        private readonly GameStateManager _gameStateManager;

        private readonly IGameMusicAudioProvider _gameMusicAudioProvider;
        private readonly IUIAudioProvider _uIAudioProvider;

        private MainMenuController _mainMenuController;
        private SettingsMenuController _settingsMenuContoller;

        public StartupMenuController(
            GameStateManager gameStateManager,
            Transform placeForUi,
            ISettingsData baseSettingsData,
            AudioMixer audioMixer, 
            AudioPresenter audioPresenter)
        {
            _gameStateManager = gameStateManager;
            _placeForUi = placeForUi;
            _baseSettingsData = baseSettingsData;
            _audioMixer = audioMixer;

            _startupSceneState = new StateModel();

            _settingsManager 
                = new GameSettingsManager(_audioMixer, _baseSettingsData);

            _startupSceneState.OnStateChange += OnChangeGameState;

            _audioController = new AudioController(audioPresenter);

            _gameMusicAudioProvider = new GameMusicAudioProvider(_audioController);
            _uIAudioProvider = new UIAudioProvider(_audioController);

            _gameMusicAudioProvider.PlayMainMenuMusic();

            _startupSceneState.CurrentState = MenuState.Start;
        }


        private void OnChangeGameState(MenuState state)
        {
            DisposeControllers();

            switch (state)
            {
                case MenuState.Start:
                    {
                        _mainMenuController 
                            = new MainMenuController(
                                _placeForUi, 
                                _startupSceneState,
                                _uIAudioProvider);
                        break;
                    }
                case MenuState.Settings:
                    {
                        _settingsMenuContoller 
                            = new SettingsMenuController(
                                _placeForUi, 
                                _settingsManager,
                                _baseSettingsData,
                                _startupSceneState,
                                _uIAudioProvider);
                        break;
                    }
                case MenuState.Game:
                    {
                        Playgame();
                        break;
                    }
                case MenuState.Exit:
                    {
                        QuitGame();
                        break;
                    }
            }
        }

        private void DisposeControllers()
        {
            _mainMenuController?.Dispose();
            _settingsMenuContoller?.Dispose();
        }


        private void Playgame()
        {
            _gameStateManager.SwitchToGame();
        }

        private void QuitGame()
        {
#if UNITY_EDITOR
            Debug.Log("EXIT!");
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        protected override void OnDispose()
        {
            DisposeControllers();
            
            _startupSceneState.OnStateChange -= OnChangeGameState;
         
            _settingsManager?.Dispose();

            _uIAudioProvider?.Dispose();
            _gameMusicAudioProvider?.Dispose();

            _audioController?.Dispose();
        }

    }
}
