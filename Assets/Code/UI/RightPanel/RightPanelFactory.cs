using Core;
using UnityEngine;
using Utils;

namespace Code.UI
{
    public class RightPanelFactory : UIViewFactory<RightPanel>
    {
        public RightPanelFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas){}
    }
}