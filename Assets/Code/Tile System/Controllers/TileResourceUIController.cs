using Code.UI;
using Code.Player;
using UnityEngine;

namespace Code.TileSystem
{
    public class TileResourceUIController : IOnTile, ITileLoadInfo
    {
        private ResourcesLayoutUIView _uiController;
        private TileController _controller;
        private TileResouceUIFactory _factory;

        public TileResourceUIController(UIController uiController, 
            InputController inputController, TileController controller, GameConfig gameConfig)
        {
            _uiController = uiController.BottomUI.ResourcesLayoutUIView;
            _controller = controller;
            _factory = new TileResouceUIFactory(uiController, this, controller, gameConfig);
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
            if (_controller.WorkerMenager.IsThereFreeWorkers(resourceView.Building) && 
                _controller.TileModel.CurrentWorkersUnits < _controller.TileModel.MaxWorkers)
            {
                IWorkerPreparation workerPreparation = null;

                _controller.WorkerMenager.StartProduction(
                    _controller.View.transform.position,
                    resourceView.Building, workerPreparation);

                resourceValue++;
            }
            resourceView.Building.MineralConfig.CurrentMineValue = resourceValue;
            resourceView.ResourceCurrentValueString = $"{resourceValue}";
        }
        
        private void RemoveResource(ResourceView resourceView)
        {
            int resourceValue = resourceView.ResourceCurrentValueInt;

            if (resourceValue > 0 && 
                _controller.WorkerMenager.IsThereBusyWorkers(
                    resourceView.Building))
            {
                _controller.WorkerMenager.StopFirstFindedWorker(
                    resourceView.Building);
                resourceValue--;
            }

            resourceView.Building.MineralConfig.CurrentMineValue = resourceValue;
            resourceView.ResourceCurrentValueString = resourceValue.ToString();
        }
    }
}
