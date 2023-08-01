using UnityEngine;
using Object = UnityEngine.Object;

namespace StartupMenu
{
    public class MainMenuController : BaseController
    {
        private readonly string _viewPath = "UI/StartupMenuUI/MainMenuUI";
        private readonly StateModel _menuMonitor;
      
        private MainMenuView _view;
    
        public MainMenuController(Transform placeForUI, StateModel menuMonitor, AudioSource clickSource)
        {
            _menuMonitor = menuMonitor;

            _view = LoadView(placeForUI);
            _view.Init(StartGame, OpenSettings, Exit, clickSource);
        }

        private MainMenuView LoadView(Transform placeForUI)
        {
            GameObject objectView = Object.Instantiate(LoadPrefab(_viewPath), placeForUI, false);
            AddGameObject(objectView);
            return objectView.GetComponent<MainMenuView>();
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

    }
}
