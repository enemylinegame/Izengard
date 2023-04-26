

namespace Code.TileSystem
{
    public class TileResourceController
    {
        private TileResourcesView _tileResourcesView;

        private int _kResourceValue = 0;
        private int _kMaxResourceValue = 5;

        private int _dResourceValue = 0;
        private int _dMaxResourceValue = 5;

        public TileResourceController(TileUIView uiView)
        {
            _tileResourcesView = uiView.TileResourcesView;

            Init();
        }

        private void Init()
        {
            _tileResourcesView.KResource.ResourceAddButton.onClick.AddListener(() => _kResourceValue = AddResource(_tileResourcesView.KResource, _kResourceValue, _kMaxResourceValue));
            _tileResourcesView.KResource.ResourceRemoveButton.onClick.AddListener(() => _kResourceValue = RemoveResource(_tileResourcesView.KResource, _kResourceValue, _kMaxResourceValue));

            _tileResourcesView.DResource.ResourceAddButton.onClick.AddListener(() => _dResourceValue = AddResource(_tileResourcesView.DResource, _dResourceValue, _dMaxResourceValue));
            _tileResourcesView.DResource.ResourceRemoveButton.onClick.AddListener(() => _dResourceValue = RemoveResource(_tileResourcesView.DResource, _dResourceValue, _dMaxResourceValue));
        }

        private int AddResource(ResourceView resourceView, int resourceValue, int resourceMaxValue)
        {
            if (resourceValue < resourceMaxValue)
            {
                resourceValue++;
            }
            resourceView.ResourceValueString = $"{resourceValue}/{resourceMaxValue}";
            return resourceValue;
        }
        private int RemoveResource(ResourceView resourceView, int resourceValue, int resourceMaxValue)
        {
            if (resourceValue > 0)
            {
                resourceValue--;
            }
            resourceView.ResourceValueString = $"{resourceValue}/{resourceMaxValue}";
            return resourceValue;
        }

    }
}
