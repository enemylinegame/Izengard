using System;
using System.Collections.Generic;
using Code.BuildingSystem;
using Code.BuldingsSystem.ScriptableObjects;
using Code.TileSystem;
using Code.UI;
using ResourceSystem;
using ResourceSystem.SupportClases;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Views.BuildBuildingsUI;
using Object = UnityEngine.Object;

namespace Code.TileSystem
{
    public class TileController : IDisposable, IOnController, IOnUpdate
    {
        private TileList _list;
        private TileUIView _uiView;
        private TileView _tileView;
        private TileModel _tileModel;
        private BaseCenterText _centerText;
        private BuildGenerator _generator;
        private GlobalStock _stock;
        private List<BuildingConfig> _buildingConfigs;
        private BuildingController _buildingController;
        private UIController _uiController;
        private WorkerAssigmentsController _workerAssigmentsController;
        private TileResourceUIController _tileResourceController;
        private TileResouceUIFactory _tileResouceUIFactory;
        private ResourcesLayoutUIView _resourcesLayoutUIView;
        private int _currentlvl;
        private int _eightQuantity;
        private int _units;

        public TileConfig Config;
        public TileList List => _list;
        public BaseCenterText CenterText => _centerText;
        public int CurrentLVL => _currentlvl;
        public WorkerAssigmentsController WorkerAssigmentsController => _workerAssigmentsController;
        public TileModel TileModel => _tileView.TileModel;
        public TileView View => _tileView;

        public TileController(TileList tileList, TileUIView uiView, BaseCenterText centerText, UIController uiController,
            BuildGenerator buildGenerator, GlobalStock stock, BuildingController buildingController)
        {
            _workerAssigmentsController = new WorkerAssigmentsController(this);
            _centerText = centerText;
            _list = tileList;
            _uiView = uiView;
            _generator = buildGenerator;
            _stock = stock;
            _uiController = uiController;
            _resourcesLayoutUIView = _uiController.ResourcesLayoutUIView;
            _stock.AddResourceToStock(ResourceType.Wood, 100);
            _buildingController = buildingController;
        }

        /// <summary>
        /// Загрузка информации о тайле
        /// </summary>
        public void LoadInfo(TileView view)
        {
            _uiView.Upgrade.onClick.RemoveAllListeners();
            _tileView = view;
            _tileModel = view.TileModel;
            _uiController.WarsView.SetDefenders(view.TileModel.DefenderUnits);
            ADDBuildUI(view.TileModel);
            view.LoadButtonsUIBuy(this, _uiController);
            _uiView.Upgrade.onClick.AddListener(() => view.LVLUp(this));
            UpdateInfo(_tileModel.TileConfig);

            _tileResourceController = new TileResourceUIController(_resourcesLayoutUIView);
            _tileResouceUIFactory = new TileResouceUIFactory(_resourcesLayoutUIView, _tileResourceController, _tileModel);
        }
        /// <summary>
        /// Загрузка всей информации на тайл
        /// </summary>
        public void UpdateInfo(TileConfig config)
        {
            _uiView.LvlText.text = config.TileLvl.GetHashCode().ToString() + " LVL";
            _eightQuantity = _tileModel.EightQuantity;
            _units = config.MaxUnits;
            _currentlvl = config.TileLvl.GetHashCode();
            _uiView.UnitMax.text = _eightQuantity + "/" + config.MaxUnits + " Units";
            _uiView.Icon.sprite = config.IconTile;
        }
        public void ADDBuildUI(TileModel model)
        {
            _uiController.Deinit();
            _buildingConfigs = model.CurrBuildingConfigs;
            _uiController.Init(_buildingConfigs);

            foreach (var kvp in _uiController.ButtonsInMenu)
            {
                kvp.Value.onClick.AddListener(() => BuildBuilding(kvp.Key, model));
            }
        }
        /// <summary>
        /// Проверяет на наличие ресурса если он есть ставим здание.
        /// </summary>
        private void BuildBuilding(BuildingConfig buildingConfig, TileModel model)
        {
            if (!IsResourcesEnough(buildingConfig))
            {
                return;
            }

            foreach (var resourcePrice in buildingConfig.BuildingCost)
            {
                _stock.GetResourceFromStock(resourcePrice.ResourceType, resourcePrice.Cost);
            }
            var building = _buildingController.StartBuilding(model, buildingConfig);
            if (building)
            {
                var info = _uiController.CreateBuildingInfo(buildingConfig, this, building);
                building.Icon = info.Icon;
                building.BuildingTypes = info.Types;
                building.InitBuilding();
                _uiController.ButtonsBuy.Add(buildingConfig);
                _tileModel.FloodedBuildings.Add(building);
            }
        }

        public void DestroyBuilding(List<Building> buildingConfigs, BuildingUIInfo Button, TileModel model)
        {
            foreach (var kvp in buildingConfigs)
            {
                if (kvp.BuildingTypes == Button.Types)
                {
                    Button.DestroyBuildingInfo.onClick.RemoveAllListeners();
                    Button.PlusUnit.onClick.RemoveAllListeners();
                    Button.MinusUnit.onClick.RemoveAllListeners();
                    buildingConfigs.Remove(kvp);
                    _uiController.DestroyBuildingInfo.Remove(Button.gameObject);
                    _tileModel.FloodedBuildings.Remove(kvp);
                    _workerAssigmentsController.RemoveAllWorkerAssigment(Button.Types, kvp, this);
                    _buildingController.RemoveTypeDots(model, kvp);
                    GameObject.Destroy(kvp.gameObject);
                    GameObject.Destroy(Button.gameObject);
                    break;
                }

            }
        }

        private bool IsResourcesEnough(BuildingConfig buildingConfig)
        {
            foreach (ResourcePriceModel resourcePriceModel in buildingConfig.BuildingCost)
            {
                if (_stock.CheckResourceInStock(resourcePriceModel.ResourceType, resourcePriceModel.Cost))
                {
                    _centerText.NotificationUI("you do not have enough resources to buy", 1000);
                    return false;
                }
            }
            return true;
        }
        #region Other

        /// <summary>
        /// Этот метод для того чтобы взять юнита из тайла
        /// </summary>
        /// <param name="EightQuantity">кол юнитов</param>
        public void hiringUnits(int EightQuantity)
        {
            if (EightQuantity <= _units)
            {
                _eightQuantity += EightQuantity;
                _uiView.UnitMax.text = _eightQuantity.ToString() + "/" + _units.ToString() + " Units";
            }
            else
            {
                _centerText.NotificationUI("you have hired the maximum number of units", 2000);
            }

        }

        /// <summary>
        /// Метод для того чтобы вернуть юнита для найма
        /// </summary>
        /// <param name="EightQuantity">кол юнитов</param>
        public void RemoveFromHiringUnits(int EightQuantity)
        {
            if (_eightQuantity > 0)
            {
                _eightQuantity -= EightQuantity;
                _uiView.UnitMax.text = _eightQuantity.ToString() + "/" + _units.ToString() + " Units";
            }
        }

        public void Dispose()
        {
            foreach (var kvp in _uiController.ButtonsInMenu)
                kvp.Value.onClick.RemoveAllListeners();
            _uiController.BuildingsUIView.CloseMenuButton.onClick.RemoveAllListeners();
            _uiController.Deinit();
            _uiView.Upgrade.onClick.RemoveAllListeners();
        }

        public void LevelCheck()
        {
            if (CurrentLVL > _uiController.DestroyBuildingInfo.Count)
            {
                _uiController.BuildingsUIView.PrefabButtonClear.gameObject.SetActive(true);
            }
            else
            {
                _uiController.BuildingsUIView.PrefabButtonClear.gameObject.SetActive(false);
            }
        }


        public void OnUpdate(float deltaTime)
        {
            LevelCheck();
        }

        #endregion
    }
}