using ResourceSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Code.TileSystem
{
    public class TileResouceUIFactory
    {
        private RectTransform _layoutTransform;
        private ResourcesLayoutUIView _resourcesLayoutUIView;
        private TileResourceUIController _tileResourceController;
        private List<Building> _buildings;

        public TileResouceUIFactory(ResourcesLayoutUIView layoutView, TileResourceUIController tileResourceController, TileModel tileModel)
        {
            _layoutTransform = layoutView.LayoutRectTransform;
            _resourcesLayoutUIView = layoutView;
            _tileResourceController = tileResourceController;
            _buildings = tileModel.FloodedBuildings;
            Init();
        }
        private void Init()
        {
            foreach (Building building in _buildings)
            {
                AddNewLayoutElement(building.MineralConfig);
            }
        }

        public void CreateResourceUIOnLayout(GameObject resourceUIElement, MineralConfig mineralConfig)
        {
            ResourceView resourceUIObjectView;
            GameObject resourceUIObject = GameObject.Instantiate(resourceUIElement, _layoutTransform);
            resourceUIObjectView = resourceUIObject.GetComponent<ResourceView>();
            resourceUIObjectView.InitViewData(mineralConfig.ResourceType.ToString(), mineralConfig.CurrentMineValue, mineralConfig);

            _resourcesLayoutUIView.Resources.Add(resourceUIObjectView);
            _tileResourceController.AddNewLayoutElement(resourceUIObjectView);
        }

        private void AddNewLayoutElement(MineralConfig mineralConfig)
        {
            GameObject resourceUIElement = Resources.Load<GameObject>("UI/ResourceUI/Res");

            CreateResourceUIOnLayout(resourceUIElement, mineralConfig);
        }
    }
}
