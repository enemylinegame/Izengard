using Code.Game;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace StartupMenu
{
    public class StartupMenuController : BaseController
    {

        private readonly Transform _placeForUi;
        private readonly AudioMixer _audioMixer;
        private readonly AudioSource _clickSource;

        private readonly StateModel _startupSceneState;

        private readonly ISettingsData _baseSettingsData;

        private readonly GameSettingsManager _settingsManager;
        private readonly GameStateManager _gameStateManager;

        private MainMenuController _mainMenuController;
        private SettingsMenuController _settingsMenuContoller;

        public StartupMenuController(
            GameStateManager gameStateManager,
            Transform placeForUi,
            ISettingsData baseSettingsData,
            AudioMixer audioMixer, 
            AudioSource clickAudioSource)
        {
            _gameStateManager = gameStateManager;
            _placeForUi = placeForUi;
            _baseSettingsData = baseSettingsData;
            _audioMixer = audioMixer;
            _clickSource = clickAudioSource;
            

            _startupSceneState = new StateModel();

            _settingsManager 
                = new GameSettingsManager(_audioMixer, _baseSettingsData);

            _startupSceneState.OnStateChange += OnChangeGameState;

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
                            = new MainMenuController(_placeForUi, _startupSceneState, _clickSource);
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
                                _clickSource);
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
        }

    }
}
