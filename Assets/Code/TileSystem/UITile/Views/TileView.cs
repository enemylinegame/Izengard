using System;
using System.Collections.Generic;
using Controllers.BuildBuildingsUI;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using Views.BuildBuildingsUI;

namespace Code.TileSystem
{
    public class TileView : MonoBehaviour
    {
        [SerializeField] private TileConfig _tileConfig;
        [SerializeField] private DotSpawns _dotSpawns;
        [SerializeField] private List<BuildingConfig> _curBuildingConfigs;
        [SerializeField] private List<Building> _floodedBuildings = new List<Building>();
        private List<BuildingConfig> _buttonsUIBuy = new List<BuildingConfig>();
        public List<WorkerView> _workers = new List<WorkerView>();
        private int _eightQuantity;
        private TileConfig _saveTileConfig;

        public TileConfig TileConfig => _tileConfig;
        public List<BuildingConfig> CurrBuildingConfigs => _curBuildingConfigs;
        public List<Building> FloodedBuildings => _floodedBuildings;
        public int EightQuantity
        {
            get => _eightQuantity;
            set => _eightQuantity = value;
        }

        public DotSpawns DotSpawns => _dotSpawns;

        private void Start()
        {
            _saveTileConfig = new TileConfig();
            _saveTileConfig = _tileConfig;
            _curBuildingConfigs = new List<BuildingConfig>(_tileConfig.BuildingTirs);
        }
        
        
        public void LVLUp(TileUIController controller)
        {
            if (_saveTileConfig.TileLvl.GetHashCode() < 5)
            {
                _saveTileConfig = controller.List.LVLList[_saveTileConfig.TileLvl.GetHashCode()];
                _tileConfig = _saveTileConfig;
                _curBuildingConfigs.AddRange(_saveTileConfig.BuildingTirs);
                controller.UpdateInfo(_saveTileConfig);
                controller.ADDBuildUI(_curBuildingConfigs, this);
            }else controller.CenterText.NotificationUI("Max LVL", 1000);
        }

        public void CreateButtonsUIBuy(TileUIController controller)
        {
            _buttonsUIBuy.AddRange(controller.BuildingsUIView.ButtonsBuy);
            controller.BuildingsUIView.ClearButtonsUIBuy();
            foreach (var kvp in _buttonsUIBuy)
            {
                var info = controller.BuildingsUIView.CreateBuildingInfo(kvp, controller.DestroyBuilding, controller);
                // _workers.AddRange(info.Type);
            }
        }
    }
}