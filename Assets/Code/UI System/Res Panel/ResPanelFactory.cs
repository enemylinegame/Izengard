using UnityEngine;

namespace UI
{
    public class ResPanelFactory : UIViewFactory<ResPanel>
    {
        public ResPanelFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas){}
    }
}