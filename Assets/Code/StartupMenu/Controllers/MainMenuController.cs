using Audio_System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace StartupMenu
{
    public class MainMenuController : BaseController
    {
        private readonly string _viewPath = "UI/StartupMenuUI/MainMenuUI";
        private readonly StateModel _menuMonitor;
        private readonly IUIAudioProvider _uIAudioProvider;

        private MainMenuView _view;
    
        public MainMenuController(
            Transform placeForUI, 
            StateModel menuMonitor, 
            IUIAudioProvider uIAudioProvider)
        {
            _menuMonitor = menuMonitor;
            _uIAudioProvider = uIAudioProvider;
            
            _view = LoadView(placeForUI);
            _view.Init(StartGame, OpenSettings, Exit);
        }

        private MainMenuView LoadView(Transform placeForUI)
        {
            GameObject objectView = Object.Instantiate(LoadPrefab(_viewPath), placeForUI, false);
            AddGameObject(objectView);
            return objectView.GetComponent<MainMenuView>();
        }

        private void StartGame()
        {
            PlaySound();
            _menuMonitor.CurrentState = MenuState.Game;
        }

        private void OpenSettings()
        {
            PlaySound();
            _menuMonitor.CurrentState = MenuState.Settings;
        }

        private void Exit()
        {
            PlaySound();
            _menuMonitor.CurrentState = MenuState.Exit;
        }

        private void PlaySound() 
            => _uIAudioProvider.PlayButtonClickCound();

    }
}
