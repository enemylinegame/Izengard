using System;
using System.Collections.Generic;
using Code.BuldingsSystem;
using Code.TileSystem.Interfaces;
using Code.UI;
using Controllers;
using ResourceSystem;
using UnityEngine;

namespace Code.TileSystem
{
    public class TileResourceUIController : IOnTile, ITileLoadInfo
    {
        private const int MAX_RESOURCES = 5;
        private ResourcesLayoutUIView _uiController;
        private TileController _controller;
        private TileResouceUIFactory _factory;
        private GlobalStock _globalStock;
        private TopResUiVew _topUIView;
        // private List<int> _resourceValueList;

        public TileResourceUIController(UIController uiController, InputController inputController,
            TileController controller, GlobalStock globalStock, TopResUiVew topUIView)
        {
            _uiController = uiController.BottonUI.ResourcesLayoutUIView;
            _controller = controller;
            _factory = new TileResouceUIFactory(uiController, this, inputController, controller);
            _globalStock = globalStock;
            _topUIView = topUIView;
            _globalStock.ResourceValueChanged += _topUIView.UpdateResursesCount;
            _globalStock.TopPanelUIBind();
            inputController.Add(this);
            
        }
        
        public void LoadInfoToTheUI(TileView tile)
        {
            _uiController.Resources.ForEach(res => AddNewLayoutElement(res));
            _factory.LoadInfoToTheUI(tile);
        }

        public void Cancel()
        {
            if(_uiController.Resources == null) return;
            
            foreach (ResourceView res in _uiController.Resources)
            {
                res.ResourceAddButton.onClick.RemoveAllListeners();
                res.ResourceRemoveButton.onClick.RemoveAllListeners();
            }
            _factory.Cancel();
        }

        public void AddNewLayoutElement(ResourceView res)
        {
            res.ResourceAddButton.onClick.AddListener(() => AddResource(res));
            res.ResourceRemoveButton.onClick.AddListener(() => RemoveResource(res));
        }
        
        private void AddResource(ResourceView resourceView)
        {
            int resourceValue = resourceView.ResourceCurrentValueInt;
            if (resourceValue < MAX_RESOURCES && _controller.WorkerMenager.UpdateWorkerAssignment(resourceView, resourceView.Building))
            {
                resourceValue++;
            }
            resourceView.Building.MineralConfig.CurrentMineValue = resourceValue;
            resourceView.ResourceCurrentValueString = $"{resourceValue}";
        }
        
        private void RemoveResource(ResourceView resourceView)
        {
            int resourceValue = resourceView.ResourceCurrentValueInt;

            if (resourceValue > 0 && _controller.WorkerMenager.RemoveWorkerAssignment(resourceView, resourceView.Building))
            {
                resourceValue--;
            }

            resourceView.Building.MineralConfig.CurrentMineValue = resourceValue;
            resourceView.ResourceCurrentValueString = resourceValue.ToString();
        }

        ~TileResourceUIController()
        {
            _globalStock.ResourceValueChanged -= _topUIView.UpdateResursesCount;
        }
    }
}
