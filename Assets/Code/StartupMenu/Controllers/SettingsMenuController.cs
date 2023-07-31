using UnityEngine;
using Object = UnityEngine.Object;

namespace StartupMenu
{
    public class SettingsMenuController : BaseController
    {
        private readonly string _viewPath = "UI/StartupMenuUI/OptionsMenuUI";

        private readonly GameSettingsManager _gameSettings;
        private readonly StateModel _menuMonitor;

        private SettingsMenuView _view;

        private bool _isSettingsChanged;

        public SettingsMenuController(
            Transform placeForUI, 
            GameSettingsManager gameSettings,
            StateModel menuMonitor,
            AudioSource clickSource)
        {
            _gameSettings = gameSettings;
            _menuMonitor = menuMonitor;
 
            _view = LoadView(placeForUI);

            _view.Init(
                ApplySettings, 
                BackToMenu,
                OnResolutionChange,
                _gameSettings.ResolutionList,
                _gameSettings.Model, 
                clickSource);

            _view.UpdateViewOptions(_gameSettings.Model);

            _isSettingsChanged = false;
        }
        
        private SettingsMenuView LoadView(Transform placeForUI)
        {
            GameObject objectView = Object.Instantiate(LoadPrefab(_viewPath), placeForUI, false);
            AddGameObject(objectView);
            return objectView.GetComponent<SettingsMenuView>();
        }

        private void ApplySettings()
        {
            _gameSettings.ApplyCurrentSettings();

            _isSettingsChanged = true;
        }

        private void BackToMenu()
        {
            if(_isSettingsChanged != true)
            {
                _gameSettings.RestoreDefaultSettings();
                _view.UpdateViewOptions(_gameSettings.Model);
            }

            _menuMonitor.CurrentState = MenuState.Start;
        }

        private void OnResolutionChange(int index)
        {
            var newResolution = _gameSettings.ResolutionList[index];
            _gameSettings.Model.ChangeResolution(newResolution.width, newResolution.height);
        }

    }
}
