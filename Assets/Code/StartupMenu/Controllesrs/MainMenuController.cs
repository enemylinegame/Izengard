using System;
using UnityEngine;

namespace StartupMenu
{
    public class MainMenuController : IDisposable
    {
        private readonly StateMonitor _menuMonitor;
        private GameObject _menuGO;
        private MainMenuView _view;
    
        public MainMenuController(GameObject prefab, Transform placeForUI, StateMonitor menuMonitor)
        {
            _menuMonitor = menuMonitor;

            CreateView(prefab, placeForUI);
        }

        private void CreateView(GameObject prefab, Transform placeForUI)
        {
            _menuGO = UnityEngine.Object.Instantiate(prefab, placeForUI, false);
            _view = _menuGO.GetComponent<MainMenuView>();
            _view.Init(StartGame, OpenSettings, Exit);
        }

        private void StartGame()
        {
            _menuMonitor.CurrentState = MenuState.Game;
        }

        private void OpenSettings()
        {
            _menuMonitor.CurrentState = MenuState.Settings;
        }

        private void Exit()
        {
            _menuMonitor.CurrentState = MenuState.Exit;
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(_menuGO);
        }
    }
}
