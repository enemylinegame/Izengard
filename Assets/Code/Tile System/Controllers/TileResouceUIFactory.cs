using ResourceSystem;
using System.Collections.Generic;
using Code.BuildingSystem;
using Code.Game;
using Code.UI;
using UnityEngine;

namespace Code.TileSystem
{
    public class TileResouceUIFactory
    {
        private RectTransform _layoutTransform;
        private TileResourceUIController _tileResourceController;
        private TileController _tileController;
        private List<ICollectable> _buildings;
        private PrefabsHolder _prefabsHolder;

        private List<ResourceView> Resources => _tileResourceController.Resources;
        public TileResouceUIFactory(TilePanelController tilePanel, TileResourceUIController tileResourceController
            , TileController tileController, PrefabsHolder prefabsHolder)
        {
            _layoutTransform = tilePanel.TileResourcesPanel.GetLayoutTransform();
            _tileResourceController = tileResourceController;
            _tileController = tileController;
            _prefabsHolder = prefabsHolder;
        }
        
        public void LoadInfoToTheUI(TileView tile)
        {
            _buildings = tile.TileModel.FloodedBuildings;
            var minerals = tile.TileModel.FloodedBuildings.FindAll(x => x.MineralConfig != null);
            foreach (var mineral in minerals)
            {
                AddNewLayoutElement(mineral);
            }
        }

        public void Cancel()
        {
            if(_buildings == null) return;
            
            var minerals = _buildings.FindAll(x => x.MineralConfig != null);
            foreach (var mineral in minerals)
            {
                RemoveLayoutElement();
            }
        }

        private void CreateResourceUIOnLayout(GameObject resourceUIElement, ICollectable mineralConfig)
        {
            ResourceView resourceUIObjectView;
            GameObject resourceUIObject = GameObject.Instantiate(resourceUIElement, _layoutTransform);
            resourceUIObjectView = resourceUIObject.GetComponent<ResourceView>();
            resourceUIObjectView.InitViewData(mineralConfig.ResourceType.ToString(),
                mineralConfig.WorkersCount, mineralConfig);

            Resources.Add(resourceUIObjectView);
            _tileResourceController.AddNewLayoutElement(resourceUIObjectView);
        }

        private void AddNewLayoutElement(ICollectable mineralConfig)
        {
            CreateResourceUIOnLayout(_prefabsHolder.Res, mineralConfig);
        }
        private void RemoveLayoutElement()
        {
            foreach (var res in Resources)
            {
                Object.Destroy(res.gameObject);
                Resources.Remove(res);
                break;
            }
        }
    }
}
