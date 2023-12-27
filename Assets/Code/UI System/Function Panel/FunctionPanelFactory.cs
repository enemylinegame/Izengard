using UnityEngine;

namespace UI
{
    public class FunctionPanelFactory : UIViewFactory<FunctionPanel>
    {
        public FunctionPanelFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas){}
    }
}