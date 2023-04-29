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
        private List<Mineral> _minerals;

        public TileResouceUIFactory(ResourcesLayoutUIView layoutView, TileResourceUIController tileResourceController, TileView tileView)
        {
            _layoutTransform = layoutView.LayoutRectTransform;
            _resourcesLayoutUIView = layoutView;
            _tileResourceController = tileResourceController;
            _minerals = tileView.FloodedMinerals;
            Init();
        }
        private void Init()
        {
            foreach (Mineral mineral in _minerals)
            {
                AddNewLayoutElement(mineral.MineralConfig);
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
