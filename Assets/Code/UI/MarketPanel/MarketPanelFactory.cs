using Core;
using ResourceMarket;
using UnityEngine;
using Utils;

namespace Code.UI.MarketPanel
{
    public class MarketPanelFactory : UIViewFactory<MarketView>
    {
        public MarketPanelFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas){}
    }
}