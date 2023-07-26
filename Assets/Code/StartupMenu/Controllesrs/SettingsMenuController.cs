using System;
using UnityEngine;

namespace StartupMenu
{
    public class SettingsMenuController : IDisposable
    {
        private GameObject _settingsGO;
        private SettingsMenuView _view;

        public SettingsMenuController(GameObject prefab, Transform placeForUI)
        {
            CreateView(prefab, placeForUI);
        }

        private void CreateView(GameObject prefab, Transform placeForUI)
        {
            _settingsGO = UnityEngine.Object.Instantiate(prefab, placeForUI, false);
            _view = _settingsGO.GetComponent<SettingsMenuView>();
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(_settingsGO);
        }
    }
}
