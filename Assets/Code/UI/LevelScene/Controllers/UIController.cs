using System.Collections.Generic;
using Code.BuildingSystem;
using Code.BuldingsSystem.ScriptableObjects;
using Code.TileSystem;
using UnityEngine;
using UnityEngine.UI;
using Views.BuildBuildingsUI;
using CombatSystem.Views;


namespace Code.UI
{
    public class UIController
    {
        private RightUI _rightUI;
        private BottonUI _bottonUI;
        private CenterUI _centerUI;
        private TileView _view;
        private WarsView _warsView;

        public TileView View => _view;
        public BuildingsUIView BuildingsUIView => _bottonUI.BuildingMenu;
        public TileUIView TileUIView => _bottonUI.TileUIView;

        public ResourcesLayoutUIView ResourcesLayoutUIView => _bottonUI.ResourcesLayoutUIView;

        public WarsView WarsView => _warsView;

        public List<BuildingConfig> ButtonsBuy = new List<BuildingConfig>();
        public Dictionary<BuildingConfig, Button> ButtonsInMenu = new Dictionary<BuildingConfig, Button>();
        public Dictionary<GameObject, BuildingUIInfo> DestroyBuildingInfo = new Dictionary<GameObject, BuildingUIInfo>();
        /// <summary>
        /// Главный контроллер UI
        /// </summary>
        public UIController(RightUI rightUI, BottonUI bottonUI, CenterUI centerUI)
        {
            _rightUI = rightUI;
            _bottonUI = bottonUI;
            _centerUI = centerUI;

            _warsView = new WarsView(bottonUI.WarsUIView);
            
            IsWorkUI(UIType.All, false);
            
            _bottonUI.BuildingMenu.PrefabButtonClear.onClick.AddListener((() => IsWorkUI(UIType.Buy, true)));
            _centerUI.CloseBuildingsBuy.onClick.AddListener((() => IsWorkUI(UIType.Buy, false)));
        }
        /// <summary>
        /// Включение/отключение любой части UI
        /// </summary>
        /// <param name="type">Тип UI</param>
        /// <param name="isOn"> Вкл/Откл</param>
        public void IsWorkUI(UIType type, bool isOn)
        {
            switch (type)
            {
                case UIType.All:
                    _centerUI.BuildingBuy.SetActive(isOn);
                    _centerUI.TileByButtons.gameObject.SetActive(!isOn);
                    ClearButtonsUIBuy(isOn);
                    IsOnTileUI(isOn);
                    break;
                case UIType.Tile:
                    IsOnTileUI(isOn);
                    break;
                case UIType.Buy:
                    _centerUI.BuildingBuy.SetActive(isOn);
                    _centerUI.TileByButtons.gameObject.SetActive(!isOn);
                    break;
                case UIType.Сonfirmation:
                    break;
                case UIType.Unit:
                    break;
            }
        }
        /// <summary>
        /// Создание кнопок из конфига здания
        /// </summary>
        public void Init(List<BuildingConfig> models)
        {
            foreach (var building in models)
            {
                var button = GameObject.Instantiate(BuildingsUIView.BuyPrefabButton, _centerUI.BuildButtonsHolder);
                ButtonsInMenu.Add(building, button);
                CreateButtonUI(building, button);
            }
        }
        /// <summary>
        /// Создание новоого блока информации определеного здания и загрузка иго в UI
        /// </summary>
        /// <returns></returns>
        public BuildingUIInfo CreateBuildingInfo(BuildingConfig config, TileController controller, Building building)
        {
            var button = GameObject.Instantiate(BuildingsUIView.BuildingInfo, BuildingsUIView.ByBuildButtonsHolder);
            var view = button.GetComponent<BuildingUIInfo>();
            
            view.Icon.sprite = config.Icon;
            view.Type.text = config.BuildingType.ToString();
            view.Types = config.BuildingType;
            view.UnitsBusy.text = view.Units +"/5";
            DestroyBuildingInfo.Add(button, view);
            view.DestroyBuildingInfo.onClick.AddListener((() => controller.DestroyBuilding(controller.TileModel.FloodedBuildings
                , view, controller.TileModel)));
            view.PlusUnit.onClick.AddListener((() => view.Hiring(true, controller, building)));
            view.MinusUnit.onClick.AddListener((() => view.Hiring(false, controller, building)));
            IsWorkUI(UIType.Buy, false);
            return view;
        }
        /// <summary>
        /// Загрузка сохраненного блока информации определеного здания и загрузка иго в UI
        /// </summary>
        /// <returns></returns>
        public BuildingUIInfo LoadBuildingInfo(Building building, int Units, TileController controller)
        {
            var button = GameObject.Instantiate(BuildingsUIView.BuildingInfo, BuildingsUIView.ByBuildButtonsHolder);
            var view = button.GetComponent<BuildingUIInfo>();
            view.Icon.sprite = building.Icon.sprite;
            view.Type.text = building.BuildingTypes.ToString();
            view.Types = building.BuildingTypes;
            view.UnitsBusy.text = Units +"/5";
            view.Units = Units;
            DestroyBuildingInfo.Add(button, view);
            view.DestroyBuildingInfo.onClick.AddListener((() => controller.DestroyBuilding(controller.TileModel.FloodedBuildings
                , view, controller.TileModel)));
            view.PlusUnit.onClick.AddListener((() => view.Hiring(true, controller, building)));
            view.MinusUnit.onClick.AddListener((() => view.Hiring(false, controller, building)));
            return view;
        }
        /// <summary>
        /// удаление кнопок для постройки зданий для загрузки другого тайла
        /// </summary>
        public void Deinit()
        {
            foreach (var kvp in ButtonsInMenu)
            {
                GameObject.Destroy(kvp.Value.gameObject);
            }
            ButtonsInMenu.Clear();
            
        }
        /// <summary>
        /// Удаление блока информации построенного здания для загрузки другого тайла
        /// </summary>
        public void ClearButtonsUIBuy(bool isOn)
        {
            if (isOn == false)
            {
                foreach (var kvp in DestroyBuildingInfo)
                {
                    GameObject.Destroy(kvp.Key.gameObject);
                }
                DestroyBuildingInfo.Clear();
                ButtonsBuy.Clear();
            }
            
        }
        
        private void CreateButtonUI(BuildingConfig buildingConfig, Button button)
        {
            var view = button.GetComponent<BuildButtonView>();
            if (view) 
            {
                view.BuildingName.text = buildingConfig.BuildingType.ToString();
                foreach (var cost in buildingConfig.BuildingCost)
                {
                    view.CostForBuildingsUI.text += cost.ResourceType + ":" + cost.Cost + " ";
                }
                view.Description.text = buildingConfig.Description;
                view.Icon.sprite = buildingConfig.Icon;
            }
            else
            {
                Debug.LogError("Button field is empty");
            }
        }

        public void IsOnTileUI(bool isOpen)
        {
            foreach (var window in _bottonUI.BuildingMenu.Windows) window.gameObject.SetActive(isOpen);
            BuildingsUIView.CloseMenuButton.gameObject.SetActive(isOpen);
        }
    }

    public enum UIType
    {
        All,
        Buy,
        Сonfirmation,
        Tile,
        Unit,
    }
}