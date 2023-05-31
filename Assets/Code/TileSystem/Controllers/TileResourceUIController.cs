using System;
using System.Collections.Generic;
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
            // _resourceValueList = new List<int>();
            foreach (ResourceView res in _uiController.Resources)
            {
                AddNewLayoutElement(res);
            }
            _factory.LoadInfoToTheUI(tile);
        }

        public void Cancel()
        {
            foreach (ResourceView res in _uiController.Resources)
            {
                res.ResourceAddButton.onClick.RemoveAllListeners();
                res.ResourceRemoveButton.onClick.RemoveAllListeners();
            }
            _factory.Cancel();
        }

        public void AddNewLayoutElement(ResourceView res)
        {
            // _resourceValueList.Add(Convert.ToInt32(res.ResourceCurrentValueString));
            res.ResourceAddButton.onClick.AddListener(() => AddResource(res));
            res.ResourceRemoveButton.onClick.AddListener(() => RemoveResource(res));
        }

        private void AddResource(ResourceView resourceView)
        {
            int resourceValue = resourceView.ResourceCurrentValueInt;
            if (resourceView.ResourceCurrentValueInt < MAX_RESOURCES)
            {
                // var hire = _controller.WorkerAssignmentsController.UpdateWorkerAssigment(resourceView.Building.ResourceType, resourceView.Building);
                // if(hire) resourceValue++;
                
            }
            resourceView.Building.MineralConfig.CurrentMineValue = resourceValue;
            resourceView.ResourceCurrentValueString = $"{resourceValue}";
            
        }
        private void RemoveResource(ResourceView resourceView)
        {
            int resourceValue = resourceView.ResourceCurrentValueInt;
            if (resourceValue > 0)
            {
                // var hire = _controller.WorkerAssignmentsController.RemoveWorkerAssigment(resourceView.Building.ResourceType, resourceView.Building);
                // if(hire) resourceValue--;
            }
            resourceView.Building.MineralConfig.CurrentMineValue = resourceValue;
            resourceView.ResourceCurrentValueString = $"{resourceValue}";
        }
    }
}
