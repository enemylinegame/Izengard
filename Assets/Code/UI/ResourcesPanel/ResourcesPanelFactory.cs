using Core;
using UnityEngine;
using Utils;

namespace Code.UI
{
    public class ResourcesPanelFactory : UIViewFactory<ResourcesPanelView>
    {
        public ResourcesPanelFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas){}
    }
}