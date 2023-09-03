using System.Collections.Generic;
using Code.BuildingSystem;
using Code.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.WSA;
using Views.BuildBuildingsUI;

namespace Code.TileSystem
{
    public class buildingsPanelController
    {
        private readonly ProductionManager _productionManager;
        private readonly BuildingFactory _buildingFactory;
        private readonly ProductionManager _workerManager;
        private List<BuildingConfig> _buildingConfigs;
        
        private readonly CenterPanelController _centerPanelController;
        private readonly TilePanelController _tilePanelController;
        
        public buildingsPanelController(BuildingFactory factory, ProductionManager workerManager, UIPanelsInitialization panelsInitialization)
        {
            _centerPanelController = panelsInitialization.CenterPanelController;
            _tilePanelController = panelsInitialization.TilePanelController;
            
            _buildingFactory = factory;
            _workerManager = workerManager;
        }
        
        public void LoadBuildings(TileModel model,  UnityAction<BuildingHUD, ICollectable> workerHiring, 
            UnityAction<BuildingHUD, ICollectable> workerDismissal, UnityAction levelCheck, UnityAction<ICollectable> resetWorkers)
        {
            List<BuildingConfig> buildingConfigs = null;
            _tilePanelController.Deinit();
            _buildingConfigs = model.CurrBuildingConfigs;

            switch (model.TileType)
            {
                case TileType.All:
                    buildingConfigs = _buildingConfigs;
                    break;
                case TileType.war:
                    buildingConfigs = _buildingConfigs.FindAll(building => building.TileType.Exists(x => x == TileType.war));
                    break;
                case TileType.Eco:
                    buildingConfigs = _buildingConfigs.FindAll(building => building.TileType.Exists(x => x == TileType.Eco));
                    break;
            }

            buildingConfigs.ForEach(building =>
            {
                var button = Object.Instantiate(_tilePanelController.TileMenu.GetBuyPrefabButton(), 
                    _centerPanelController.TransformBuildButtonsHolder());
                _tilePanelController.ButtonsInMenu.Add(building, button);
                
                CreateBuidlingButtonUI(building, button);
            });
            
            foreach (var kvp in _tilePanelController.ButtonsInMenu)
            {
                kvp.Value.onClick.AddListener(() =>
                {
                    ICollectable building = _buildingFactory.BuildBuilding(kvp.Key, model);
                    if (building == null)
                    {
                        _centerPanelController.DeactivateBuildingBuyUI();
                        return; 
                    }
                    
                    //TODO: данные мектоды перенести, в другой класс
                    BuildingHUD info = CreateBuildingInfo(kvp.Key, model,building,workerHiring, workerDismissal, levelCheck, resetWorkers);
                    LoadBuildingHUD(info, building, kvp.Key);
                    model.FloodedBuildings.Add(building);
                    building.InitBuilding();
                    levelCheck();
                });
            }
        }
        private void CreateBuidlingButtonUI(BuildingConfig buildingConfig, Button button)
        {
            if (!button.TryGetComponent(out BuildButtonView view))
            {
                Debug.LogError("Button field is empty");
                return;
            }

            view.BuildingName.text = buildingConfig.BuildingType.ToString();

            string costText = "";
            foreach (var cost in buildingConfig.BuildingCost)
            {
                costText += $"{cost.ResourceType}:{cost.Cost} ";
            }
            view.CostForBuildingsUI.text = costText;

            view.Description.text = buildingConfig.Description;
            view.Icon.sprite = buildingConfig.Icon;
        }
        
        public BuildingHUD CreateBuildingInfo(BuildingConfig config, TileModel model,ICollectable building, UnityAction<BuildingHUD, ICollectable> workerHiring, 
            UnityAction<BuildingHUD, ICollectable> workerDismissal, UnityAction levelCheck, UnityAction<ICollectable> resetWorkers)
        {
            var buildingHud = Object.Instantiate(_tilePanelController.TileMenu.GetPrefabBuildingInfo().GetComponent<BuildingHUD>(), 
                _tilePanelController.TileMenu.GetByBuildButtonsHolder());

            buildingHud.Icon.sprite = config.Icon;
            buildingHud.Type.text = config.BuildingType.ToString();
            buildingHud.BuildingType = config.BuildingType;

            buildingHud.MaxWorkers = model.MaxWorkers;
            buildingHud.WorkersСount = building.WorkersCount;


            _tilePanelController.DestroyBuildingInfo.Add(buildingHud.gameObject, buildingHud);

            buildingHud.DestroyBuildingInfo.onClick.AddListener(() => DestroyBuilding(buildingHud, building, model,levelCheck, resetWorkers));

            buildingHud.PlusUnitButton.onClick.AddListener(() => workerHiring(buildingHud, building));
            buildingHud.MinusUnitButton.onClick.AddListener(() => workerDismissal(buildingHud, building));

            _centerPanelController.DeactivateBuildingBuyUI();
            return buildingHud;
        }
        private void LoadBuildingInfo(ICollectable building, TileModel model,UnityAction<BuildingHUD, ICollectable> workerHiring, 
            UnityAction<BuildingHUD, ICollectable> workerDismissal, UnityAction levelCheck, UnityAction<ICollectable> resetWorkers)
        {
            var button = Object.Instantiate(_tilePanelController.TileMenu.GetPrefabBuildingInfo(), 
                _tilePanelController.TileMenu.GetByBuildButtonsHolder());
            var buildingHud = button.GetComponent<BuildingHUD>();

            buildingHud.Icon.sprite = building.Icon;
            buildingHud.Type.text = building.BuildingTypes.ToString();
            buildingHud.BuildingType = building.BuildingTypes;
            buildingHud.BuildingID = building.BuildingID;

            buildingHud.MaxWorkers = building.MaxWorkers;
            buildingHud.WorkersСount = building.WorkersCount;

            var destroyButton = buildingHud.DestroyBuildingInfo;
            _tilePanelController.DestroyBuildingInfo.Add(button, buildingHud);
            destroyButton.onClick.AddListener(() => DestroyBuilding(buildingHud, building, model,levelCheck, resetWorkers));

            buildingHud.PlusUnitButton.onClick.AddListener(() => workerHiring(buildingHud, building));

            buildingHud.MinusUnitButton.onClick.AddListener(() => workerDismissal(buildingHud, building));

            levelCheck();
        }

        private void DestroyBuilding(BuildingHUD buildingHud, ICollectable building, TileModel model,UnityAction leveCheck, UnityAction<ICollectable> resetWorkers)
        {
            _productionManager.StopAllProductions(building);
            UnLoadBuildingHUD(buildingHud);
            _buildingFactory.DestroyBuilding(building, buildingHud, model);
            _workerManager.StopAllProductions(building);
            resetWorkers(building);
            leveCheck();
        }

        public void LoadFloodedBuildings(TileModel model,UnityAction<BuildingHUD, ICollectable> workerHiring, 
            UnityAction<BuildingHUD, ICollectable> workerDismissal, UnityAction levelCheck, UnityAction<ICollectable> resetWorkers)
        {
            var buildings = model.FloodedBuildings.FindAll(x => x.MineralConfig == null);

            foreach (var building in buildings)
            {
                LoadBuildingInfo(building, model,workerHiring, workerDismissal, levelCheck, resetWorkers);
            }
        }
        
        private void LoadBuildingHUD(BuildingHUD info, ICollectable building, BuildingConfig buildingConfig)
        {
            building.Icon = info.Icon.sprite;
            building.BuildingTypes = info.BuildingType;
            info.BuildingID = building.BuildingID;
            info.MaxWorkers = building.MaxWorkers;
        }

        private void UnLoadBuildingHUD(BuildingHUD info)
        {
            info.DestroyBuildingInfo.onClick.RemoveAllListeners();
            info.PlusUnitButton.onClick.RemoveAllListeners();
            info.MinusUnitButton.onClick.RemoveAllListeners();
        }
    }
}