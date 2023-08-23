using Core;
using UnityEngine;
using Utils;

namespace Code.UI
{
    public class TileUIFactory : UIViewFactory<TileUI>
    {
        public TileUIFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas){}
    }
}