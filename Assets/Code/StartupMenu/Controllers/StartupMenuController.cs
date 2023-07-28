using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace StartupMenu
{
    public class StartupMenuController : BaseController
    {
        private readonly int _gameSceneIndex = 1;

        private readonly Transform _placeForUi;
        private readonly AudioMixer _audioMixer;
        private readonly AudioSource _clickSource;

        private readonly StateModel _startupSceneState;

        private MainMenuController _mainMenuController;
        private SettingsMenuController _settingsMenuContoller;

        public StartupMenuController(Transform placeForUi, AudioMixer audioMixer, AudioSource clickAudioSource)
        {
            _placeForUi = placeForUi;
            _audioMixer = audioMixer;
            _clickSource = clickAudioSource;

            _startupSceneState = new StateModel();

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
                        _mainMenuController = new MainMenuController(_placeForUi, _startupSceneState, _clickSource);
                        break;
                    }
                case MenuState.Settings:
                    {
                        _settingsMenuContoller = new SettingsMenuController(_placeForUi, _audioMixer, _startupSceneState, _clickSource);
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


        public void Playgame()
        {
            SceneManager.LoadScene(_gameSceneIndex);
        }

        public void QuitGame()
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
        }

    }
}
