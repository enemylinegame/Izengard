using UnityEngine;
using Object = UnityEngine.Object;

namespace StartupMenu
{
    public class SettingsMenuController : BaseController
    {
        private readonly string _viewPath = "UI/StartupMenuUI/OptionsMenuUI";
        private readonly StateModel _menuMonitor;

        private readonly SettingsMenuModel _model;

        private SettingsMenuView _view;

        public SettingsMenuController(Transform placeForUI, StateModel menuMonitor)
        {
            _menuMonitor = menuMonitor;

            _model = new SettingsMenuModel();

            CreateView(placeForUI);
        }
        private void CreateView(Transform placeForUI)
        {
            _view = LoadView(placeForUI);
            _view.Init(BackToMenu, _model);
        }

        private SettingsMenuView LoadView(Transform placeForUI)
        {
            GameObject objectView = Object.Instantiate(LoadPrefab(_viewPath), placeForUI, false);
            AddGameObject(objectView);
            return objectView.GetComponent<SettingsMenuView>();
        }

        private void BackToMenu()
        {
            _menuMonitor.CurrentState = MenuState.Start;
        }
    }
}
