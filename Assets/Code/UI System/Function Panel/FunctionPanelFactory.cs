using UnityEngine;

namespace Code.UI
{
    public class FunctionPanelFactory : UIViewFactory<FunctionPanel>
    {
        public FunctionPanelFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas){}
    }
}