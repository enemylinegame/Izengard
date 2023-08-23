using ResourceSystem;

namespace Code.UI
{
    public class ResourcesPanelController
    {
        private ResourcesPanelView _view;
        
        public ResourcesPanelController(TopPanelFactory factory)
        {
            _view = factory.GetView(factory.UIElementsConfig.TopPanel);
        }
        
        public void UpdateResursesCount(ResourceType type, int count)
        {
            switch(type)
            {
                case ResourceType.Wood:
                    _view.woodCount.text = $"{count}";
                    break;
                case ResourceType.Iron:
                    _view.ironCount.text = $"{count}";
                    break;
                case ResourceType.Deer:
                    _view.deercount.text = $"{count}";
                    break;
                case ResourceType.Horse:
                    _view.horsecount.text = $"{count}";
                    break;
                case ResourceType.Textile:
                    _view.textilecount.text = $"{count}";
                    break;
                case ResourceType.Steel:
                    _view.steelcount.text = $"{count}";
                    break;
                case ResourceType.MagicStones:
                    _view.magikStonescount.text = $"{count}";
                    break;
                case ResourceType.Gold:
                    _view.goldCount.text = $"{count}";
                    break;
            }
        }
        
        public void UpdatePeopleCount(int maxValue)
        {
            _view.workerCount.text = $"{maxValue}";
        }
    }
}