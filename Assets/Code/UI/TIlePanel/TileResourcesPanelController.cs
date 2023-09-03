using Code.TileSystem;
using UnityEngine;

namespace Code.UI
{
    public class TileResourcesPanelController
    {
        private readonly TileResourcesPanel _view;

        public TileResourcesPanelController(TileResourcesPanel view)
        {
            _view = view;
        }

        public RectTransform GetLayoutTransform()
        {
            return _view.LayoutRectTransform;
        }
    }
}