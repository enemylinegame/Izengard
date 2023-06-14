﻿using Code.UI;
using Code.Player;
using UnityEngine;

namespace Code.TileSystem
{
    public class TileResourceUIController : IOnTile, ITileLoadInfo
    {
        private const int MAX_RESOURCES = 5;
        private ResourcesLayoutUIView _uiController;
        private TileController _controller;
        private TileResouceUIFactory _factory;

        public TileResourceUIController(UIController uiController, InputController inputController, TileController controller)
        {
            _uiController = uiController.BottonUI.ResourcesLayoutUIView;
            _controller = controller;
            _factory = new TileResouceUIFactory(uiController, this, controller);
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
            Vector3 workPlace = Vector3.forward * 10.0f;
            IWorkerPreparation workerPreparation = null;
            Debug.LogWarning("workPlace is not defined");
            Debug.LogWarning("workerPreparation is not defined");

            int resourceValue = resourceView.ResourceCurrentValueInt;
            if (resourceValue < MAX_RESOURCES && 
                _controller.WorkerMenager.StartProduction(
                    resourceView.Building, workPlace, workerPreparation))
            {
                resourceValue++;
            }
            resourceView.Building.MineralConfig.CurrentMineValue = resourceValue;
            resourceView.ResourceCurrentValueString = $"{resourceValue}";
        }
        
        private void RemoveResource(ResourceView resourceView)
        {
            int resourceValue = resourceView.ResourceCurrentValueInt;

            if (resourceValue > 0 && _controller.WorkerMenager.
                StopFirstFindedProduction(resourceView.Building))
            {
                resourceValue--;
            }

            resourceView.Building.MineralConfig.CurrentMineValue = resourceValue;
            resourceView.ResourceCurrentValueString = resourceValue.ToString();
        }
    }
}
