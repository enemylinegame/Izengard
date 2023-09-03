using Core;
using UnityEngine;
using Utils;

namespace Code.UI
{
    public class InGameMenuPanelFactory : UIViewFactory<InGameMenuPanel>
    {
        public InGameMenuPanelFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas){}
    }
}