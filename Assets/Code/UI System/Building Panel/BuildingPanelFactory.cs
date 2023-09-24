using UnityEngine;

namespace Code.UI
{
    public class BuildingPanelFactory : UIViewFactory<BuildingPanel>
    {
        public BuildingPanelFactory(UIElementsConfig uIElementsConfig, Canvas mainCanvas) : base(uIElementsConfig, mainCanvas){}
    }
}