using Code.UI;
using Code.Player;


namespace Code.TileSystem
{
    public class TileResourceUIController : IOnTile, ITileLoadInfo
    {
        private ResourcesLayoutUIView _resourceUiView;
        private TileController _tileController;
        private TileResouceUIFactory _resourceFactory;

        public TileResourceUIController(UIController uiController, 
            InputController inputController, TileController controller, GameConfig gameConfig)
        {
            _resourceUiView = uiController.BottomUI.ResourcesLayoutUIView;
            _tileController = controller;
            _resourceFactory = new TileResouceUIFactory(uiController, this, controller, gameConfig);
            inputController.Add(this);
            
        }
        public void LoadInfoToTheUI(TileView tile)
        {
            _resourceUiView.Resources.ForEach(res => AddNewLayoutElement(res));
            _resourceFactory.LoadInfoToTheUI(tile);
        }

        public void Cancel()
        {
            if(_resourceUiView.Resources == null) 
                return;
            
            foreach (ResourceView res in _resourceUiView.Resources)
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
