using System;
using System.Collections.Generic;
using Code.BuildingSystem;
using Code.BuldingsSystem;
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
        public TileModel TileModel;
        private void Awake()
        {
            TileModel = new TileModel();
            TileModel.TileConfig = _tileConfig;
            TileModel.DotSpawns = _dotSpawns;
            TileModel.Init();
        }

        /// <summary>
        /// Увеличение уровня тайла
        /// </summary>
        public void LVLUp(TileController controller)
        {
            int currentLevel = TileModel.SaveTileConfig.TileLvl.GetHashCode();
            if (currentLevel < 5)
            {
                TileModel.SaveTileConfig = controller.List.LVLList[currentLevel];
                TileModel.TileConfig = TileModel.SaveTileConfig;
                TileModel.CurrBuildingConfigs.AddRange(TileModel.SaveTileConfig.BuildingTirs);
                
                controller.UpdateInfo(TileModel.SaveTileConfig);
                controller.LoadBuildings(TileModel);
                controller.LevelCheck();
            }
            else
            {
                controller.CenterText.NotificationUI("Max LVL", 1000);
            }
        }

    }
}