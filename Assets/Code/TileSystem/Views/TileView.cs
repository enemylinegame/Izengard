using System;
using System.Collections.Generic;
using Code.BuildingSystem;
using Code.BuldingsSystem.ScriptableObjects;
using Code.UI;
using CombatSystem;
using ResourceSystem;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using Views.BuildBuildingsUI;

namespace Code.TileSystem
{
    public class TileView : MonoBehaviour
    {
        [SerializeField] private TileConfig _tileConfig;
        [SerializeField] private List<Dot> _dotSpawns;
        [SerializeField] private List<Building> _floodedBuildings;
        // [SerializeField] private List<Mineral> _floodedMinerals;
        public TileModel TileModel;
        //
        // private List<BuildingConfig> _curBuildingConfigs;
        // private List<WorkerView> _workerViews;
        // private List<DefenderUnit> _defenderUnits;
        // private List<WorkersAssigments> _workersAssigmentses;
        // private TileConfig _saveTileConfig;
        // private int _eightQuantity;
        //
        // public List<WorkerView> Workers => _workerViews;
        // public TileConfig TileConfig => _tileConfig;
        // public List<BuildingConfig> CurrBuildingConfigs => _curBuildingConfigs;
        // public List<Building> FloodedBuildings => _floodedBuildings;
        // public List<Mineral> FloodedMinerals => _floodedMinerals;
        // public List<WorkersAssigments> WorkersAssigments => _workersAssigmentses; 
        // public int EightQuantity
        // {
        //     get => _eightQuantity;
        //     set => _eightQuantity = value;
        // }
        //
        // public List<Dot> DotSpawns => _dotSpawns;
        // public TileConfig SaveTileConfig => _saveTileConfig;
        // private void Awake()
        // {
        //     _saveTileConfig = new TileConfig();
        //     _curBuildingConfigs = new List<BuildingConfig>(_tileConfig.BuildingTirs);
        //     _floodedBuildings = new List<Building>();
        //     _floodedMinerals = new List<Mineral>();
        //     _workerViews = new List<WorkerView>();
        //     _workersAssigmentses = new List<WorkersAssigments>();
        //
        //     _saveTileConfig = _tileConfig;
        //     // FillWorkerList();
        // }

        private void Awake()
        {
            TileModel = new TileModel();
            TileModel.TileConfig = _tileConfig;
            TileModel.DotSpawns = _dotSpawns;
            TileModel.Init();
            _floodedBuildings = TileModel.FloodedBuildings;
            // _floodedMinerals = TileModel.FloodedMinerals;
        }

        /// <summary>
        /// Увеличение уровня тайла
        /// </summary>
        public void LVLUp(TileController controller)
        {
            if (TileModel.SaveTileConfig.TileLvl.GetHashCode() < 5)
            {
                TileModel.SaveTileConfig = controller.List.LVLList[TileModel.SaveTileConfig.TileLvl.GetHashCode()];
                TileModel.TileConfig = TileModel.SaveTileConfig;
                TileModel.CurrBuildingConfigs.AddRange(TileModel.SaveTileConfig.BuildingTirs);
                controller.UpdateInfo(TileModel.SaveTileConfig);
                controller.ADDBuildUI(TileModel);
                controller.WorkerAssignmentsController.FillWorkerList();
            }else controller.CenterText.NotificationUI("Max LVL", 1000);
        }
        /// <summary>
        /// Загрузка сохраненного блока информации определеного здания и загрузка иго в UI
        /// </summary>
        public void LoadButtonsUIBuy(TileController controller, UIController uiController)
        {
            controller.WorkerAssignmentsController.FillWorkerList();
            foreach (var building in TileModel.FloodedBuildings)
            {
                if (building.MineralConfig == null)
                {
                    uiController.LoadBuildingInfo(building, controller.WorkerAssignmentsController.GetAssignedWorkers(building), controller);
                }
            }
        }
    }
}