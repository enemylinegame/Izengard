using BuildingSystem;
using System;
using Views.BuildBuildingsUI;


namespace Controllers.BuildBuildingsUI
{
    public class BuildBuildingsUIController : IDisposable, IOnController
    {
        private readonly BuildBuildingsUIView _view;
        

        public BuildBuildingsUIController(BuildBuildingsUIView view, GlobalBuildingsModels buildingsModels, BuildGenerator buildGenerator)
        {
            _view = view;
            var buildBuildings = new BuildBuildings(buildingsModels, buildGenerator);
            view.Init(buildingsModels);

            foreach (var kvp in _view.ButtonsInMenu)
            {
                kvp.Value.onClick.AddListener(() => buildBuildings.BuildBuilding(kvp.Key));
                kvp.Value.onClick.AddListener(OnCloseMenuButton);
            }

            _view.OpenMenuButton.onClick.AddListener(OnOpenMenuButton);
            _view.CloseMenuButton.onClick.AddListener(OnCloseMenuButton);

            OpenMenu(false);
        }

        private void OnOpenMenuButton() => OpenMenu(true);

        private void OnCloseMenuButton() => OpenMenu(false);

        private void OpenMenu(bool isOpen)
        {
            _view.OpenMenuButton.gameObject.SetActive(!isOpen);
            _view.BuildButtonsHolder.gameObject.SetActive(isOpen);
            _view.CloseMenuButton.gameObject.SetActive(isOpen);
        }

        public void Dispose()
        {
            foreach (var kvp in _view.ButtonsInMenu)
                kvp.Value.onClick.RemoveAllListeners();
            _view.OpenMenuButton.onClick.RemoveAllListeners();
            _view.CloseMenuButton.onClick.RemoveAllListeners();
            _view.Deinit();
        }
    }
}