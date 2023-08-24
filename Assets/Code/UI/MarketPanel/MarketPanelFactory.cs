using Core;
using ResourceMarket;
using UnityEngine;
using Utils;

namespace Code.UI
{
    public class MarketPanelFactory : UIViewFactory<MarketPanel>
    {
        public MarketPanelFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas){}
    }
}