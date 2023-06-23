using ResourceSystem;
using System.Collections.Generic;
using Code.BuildingSystem;
using Code.UI;
using UnityEngine;

namespace Code.TileSystem
{
    public class TileResouceUIFactory
    {
        private RectTransform _layoutTransform;
        private ResourcesLayoutUIView _resourcesLayoutUIView;
        private TileResourceUIController _tileResourceController;
        private TileController _tileController;
        private List<ICollectable> _buildings;

        public TileResouceUIFactory(UIController uiController, TileResourceUIController tileResourceController
            , TileController tileController)
        {
            _layoutTransform = uiController.BottomUI.ResourcesLayoutUIView.LayoutRectTransform;
            _resourcesLayoutUIView = uiController.BottomUI.ResourcesLayoutUIView;
            _tileResourceController = tileResourceController;
            _tileController = tileController;
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
                RemoveLayoutElement(mineral.MineralConfig);
            }
        }

        private void CreateResourceUIOnLayout(GameObject resourceUIElement, ICollectable mineralConfig)
        {
            ResourceView resourceUIObjectView;
            GameObject resourceUIObject = GameObject.Instantiate(resourceUIElement, _layoutTransform);
            resourceUIObjectView = resourceUIObject.GetComponent<ResourceView>();
            resourceUIObjectView.InitViewData(mineralConfig.ResourceType.ToString(),
                _tileController.WorkerMenager.GetAssignedWorkers(mineralConfig), mineralConfig);

            _resourcesLayoutUIView.Resources.Add(resourceUIObjectView);
            _tileResourceController.AddNewLayoutElement(resourceUIObjectView);
        }

        private void AddNewLayoutElement(ICollectable mineralConfig)
        {
            GameObject resourceUIElement = Resources.Load<GameObject>("UI/ResourceUI/Res");//TODO PLS DEL

            CreateResourceUIOnLayout(resourceUIElement, mineralConfig);
        }
        private void RemoveLayoutElement(MineralConfig mineralConfig)
        {
            foreach (var res in _resourcesLayoutUIView.Resources)
            {
                Object.Destroy(res.gameObject);
                _resourcesLayoutUIView.Resources.Remove(res);
                break;
            }
        }
    }
}
