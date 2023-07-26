using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StartupMenu
{
    public class StartupMenuController
    {
        private readonly int _gameSceneIndex = 1;
        private readonly Transform _placeForUi;

        private readonly GameObject _mainMenuPrefab;
        private readonly GameObject _settingsPrefab;

        private readonly StateMonitor _startupSceneState;

        private MainMenuController _mainMenuController;
        private SettingsMenuController _settingsMenuContoller;

        public StartupMenuController(Transform placeForUi, GameObject mainMenuPrefab, GameObject settingsPrefab)
        {
            _placeForUi = placeForUi;
            _mainMenuPrefab = mainMenuPrefab;
            _settingsPrefab = settingsPrefab;

            _startupSceneState = new StateMonitor();

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
                        _mainMenuController = new MainMenuController(_mainMenuPrefab, _placeForUi, _startupSceneState);
                        break;
                    }
                case MenuState.Settings:
                    {
                        _settingsMenuContoller = new SettingsMenuController(_settingsPrefab, _placeForUi);
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

    }
}
