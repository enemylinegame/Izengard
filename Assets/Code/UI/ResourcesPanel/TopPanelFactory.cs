using Core;
using UnityEngine;
using Utils;

namespace Code.UI
{
    public class TopPanelFactory : UIViewFactory<ResourcesPanelView>
    {
        public TopPanelFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas){}
    }
}