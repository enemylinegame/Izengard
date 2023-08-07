using Audio_System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace StartupMenu
{
    public class MainMenuController : BaseController
    {
        private readonly string _viewPath = "UI/StartupMenuUI/MainMenuUI";
        private readonly StateModel _menuMonitor;
        private readonly IAudioPlayer _audioPlayer;

        private readonly ISound _clickSound;
        
        private MainMenuView _view;
    
        public MainMenuController(Transform placeForUI, StateModel menuMonitor, IAudioPlayer audioPlayer)
        {
            _menuMonitor = menuMonitor;
            _audioPlayer = audioPlayer;
            
            _view = LoadView(placeForUI);
            _view.Init(StartGame, OpenSettings, Exit);

            _clickSound = _view.ClickSound;
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
            => _audioPlayer.PlaySound2D(_clickSound);

    }
}
