using UnityEngine;

namespace Code.UI
{
    public class InfoPanelFactory : UIViewFactory<InfoPanel>
    {
        public InfoPanelFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas){}
    }
}