using Core;
using UnityEngine;
using Utils;

namespace Code.UI
{
    public class TilePanelFactory : UIViewFactory<TilePanel>
    {
        public TilePanelFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas){}
    }
}