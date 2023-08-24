using System.Collections.Generic;
using Code.UI;
using Code.Player;


namespace Code.TileSystem
{
    public class TileResourceUIController : IOnTile, ITileLoadInfo
    {
        private TileController _tileController;
        private TileResouceUIFactory _resourceFactory;

        public List<ResourceView> Resources = new List<ResourceView>();

        public TileResourceUIController(TilePanelController tilePanel, 
            InputController inputController, TileController controller, GameConfig gameConfig)
        {
            _tileController = controller;
            _resourceFactory = new TileResouceUIFactory(tilePanel, this, controller, gameConfig);
            inputController.Add(this);
            
        }
        public void LoadInfoToTheUI(TileView tile)
        {
            Resources.ForEach(res => AddNewLayoutElement(res));
            _resourceFactory.LoadInfoToTheUI(tile);
        }

        public void Cancel()
        {
            if(Resources == null) 
                return;
            
            foreach (ResourceView res in Resources)
            {
                res.ResourceAddButton.onClick.RemoveAllListeners();
                res.ResourceRemoveButton.onClick.RemoveAllListeners();
            }
            _resourceFactory.Cancel();
        }

        public void AddNewLayoutElement(ResourceView res)
        {
            res.ResourceAddButton.onClick.AddListener(() => AddResourceWorker(res));
            res.ResourceRemoveButton.onClick.AddListener(() => RemoveResourceWorker(res));
        }

        private void AddResourceWorker(ResourceView resourceView)
        {
            var building = resourceView.Building;
            if (!_tileController.IsThereFreeWorkers(building))
                return;

            _tileController.WorkerMenager.StartMiningProduction(_tileController.View.transform.position, building);

            _tileController.IncrementWorkersAccount(building);

            resourceView.WorkersCount = building.WorkersCount;
        }

        private void RemoveResourceWorker(ResourceView resourceView)
        {
            var building = resourceView.Building;
            if (!_tileController.IsThereBusyWorkers(building))
                return;

            _tileController.WorkerMenager.StopFirstFindedWorker(building);
            _tileController.DecrementWorkersAccount(building);

            resourceView.WorkersCount = building.WorkersCount;
        }
    }
}
